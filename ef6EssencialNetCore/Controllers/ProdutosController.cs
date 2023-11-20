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

        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        } 

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _uof.ProdutoRepository.Get().ToList();

                if (produtos == null)
                {
                    return  NotFound("Produtos Não Encontrados");
                }

                return produtos;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpGet("{id:int}", Name = "obterProduto")]
        public ActionResult<Produto> Get([FromQuery]int id)
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

                return produto;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            try
            {
                if (produto == null)
                    return BadRequest();

                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                return new CreatedAtRouteResult("obterProduto", new { id = produto.ProdutoId }, produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest();
                }

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
        public ActionResult Delete(int id)
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

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }
    }
}
