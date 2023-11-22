using AutoMapper;
using ef6EssencialNetCore.DTO;
using ef6EssencialNetCore.Filters;
using ef6EssencialNetCore.Models;
using ef6EssencialNetCore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ef6EssencialNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorPreco()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            var ProdutoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return ProdutoDTO;
        } 

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            try
            {
                //Mapeamento
                var produtos = _uof.ProdutoRepository.Get().ToList();
                var ProdutoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

                //Valida se tem item
                if (ProdutoDTO == null)
                {
                    return  NotFound("Produtos Não Encontrados");
                }

                //Retorna o item
                return ProdutoDTO;
            }
            catch (Exception ex)
            {   
                //mensagem de erro
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpGet("{id:int}", Name = "obterProduto")]
        public ActionResult<ProdutoDTO> Get([FromQuery]int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(
                    p => p.ProdutoId == id
                );

                if (produto == null)
                {
                    return NotFound("Produto Não Encontrado");
                }

                var ProdutoDTO = _mapper.Map<ProdutoDTO>(produto);
                return ProdutoDTO;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpPost]
        public ActionResult Post(ProdutoDTO produtoDto)
        {
            try
            {   var produto = _mapper.Map<Produto>(produtoDto);

                if (produto == null)
                    return BadRequest();

                _uof.ProdutoRepository.Add(produto);
                _uof.Commit(); 

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return new CreatedAtRouteResult("obterProduto", new { id = produto.ProdutoId }, produtoDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.ProdutoId)
                    return BadRequest();

                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Update(produto);
                _uof.Commit();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(
                    p => p.ProdutoId == id
                );

                if (produto == null)
                {
                    return NotFound("Produto Não Encontrado");
                }

                _uof.ProdutoRepository.Delete(produto);
                _uof.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);
                return Ok(produtoDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }
    }
}
