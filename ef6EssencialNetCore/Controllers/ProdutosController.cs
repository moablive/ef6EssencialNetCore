using ef6EssencialNetCore.Context;
using ef6EssencialNetCore.Filters;
using ef6EssencialNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ef6EssencialNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            try
            {
                var produtos = _context.Produtos.AsNoTracking().ToListAsync();

                if (produtos == null)
                {
                    return  NotFound("Produtos Não Encontrados");
                }

                return await produtos;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpGet("{id:int}", Name = "obterProduto")]
        public async Task<ActionResult<Produto>> GetAsync([FromQuery]int id)
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound("Produto Não Encontrado");
                }

                return await produto;
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

                _context.Produtos.Add(produto);
                _context.SaveChanges();

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

                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();

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
                var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

                if (produto == null)
                {
                    return NotFound("Produto Não Encontrado");
                }

                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }
    }
}
