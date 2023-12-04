//System
using System.Text.Json;

//External
using AutoMapper;

//Project
using ef6EssencialNetCore.DTO;
using ef6EssencialNetCore.Filters;
using ef6EssencialNetCore.Helpers.Pagination;
using ef6EssencialNetCore.Models;
using ef6EssencialNetCore.Repository;

//Microsoft
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ef6EssencialNetCore.Controllers
{   
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorPreco()
        {
            var produtos = _context.ProdutoRepository.GetProdutosPorPreco();
            var ProdutoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return ProdutoDTO;
        } 

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutoParameters produtoParameters) 
        {
            try
            {
                //Mapeamento Paginado
                var produtos = await _context.ProdutoRepository.GetProdutos(produtoParameters);

                var metadata = new
                {
                    produtos.TotalCount,
                    produtos.PageSize,
                    produtos.CurrentPage,
                    produtos.TotalPages,
                    produtos.HasNext,
                    produtos.HasPrevious
                };

                Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(metadata));
                var ProdutoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

                if (ProdutoDTO == null)
                {
                    return  NotFound("Produtos Não Encontrados");
                }

                return ProdutoDTO;
            }
            catch (Exception)
            {   
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpGet("{id:int}", Name = "obterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get([FromQuery]int id)
        {
            try
            {
                var produto = await _context.ProdutoRepository.GetById(
                    p => p.ProdutoId == id
                );

                if (produto == null)
                {
                    return NotFound("Produto Não Encontrado");
                }

                var ProdutoDTO = _mapper.Map<ProdutoDTO>(produto);
                return ProdutoDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProdutoDTO produtoDto)
        {
            try
            {   var produto = _mapper.Map<Produto>(produtoDto);

                if (produto == null)
                    return BadRequest();

                _context.ProdutoRepository.Add(produto);
                await _context.Commit(); 

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return new CreatedAtRouteResult("obterProduto", new { id = produto.ProdutoId }, produtoDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.ProdutoId)
                    return BadRequest();

                var produto = _mapper.Map<Produto>(produtoDto);

                _context.ProdutoRepository.Update(produto);
                await _context.Commit();

                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            try
            {
                var produto = await _context.ProdutoRepository.GetById(
                    p => p.ProdutoId == id
                );

                if (produto == null)
                {
                    return NotFound("Produto Não Encontrado");
                }

                _context.ProdutoRepository.Delete(produto);
                await _context.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);
                return Ok(produtoDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }
    }
}
