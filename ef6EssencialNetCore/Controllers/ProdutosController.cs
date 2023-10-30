using ef6EssencialNetCore.Context;
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
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.AsNoTracking().ToList();

            if (produtos is null) 
            {
                return NotFound("Produtos Não Encontrados");
            }

            return produtos;
        }

        [HttpGet("{id:int}", Name="obterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(
                p => p.ProdutoId == id
            );

            if (produto is null)
            {
                return NotFound("Produto Não Encontrado");
            }

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if(produto is null)
                return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("obterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if(id != produto.ProdutoId)
            { 
                return BadRequest(); 
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(
                p => p.ProdutoId == id
            );

            if (produto is null)
            {
                return NotFound("Produto Não Encontrado");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}