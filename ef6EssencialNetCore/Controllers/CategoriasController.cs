using ef6EssencialNetCore.Context;
using ef6EssencialNetCore.Models;
using ef6EssencialNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ef6EssencialNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public CategoriasController(AppDbContext context, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("author")]
        public string GetAuthor()
        {
            var author = _configuration["author"];
            var conexao = _configuration["ConnectionStrings:DefaultConnection"];

            return $"Author : {author} + conexao {conexao}";
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                _logger.LogInformation("=============================GET api/categorias/produtos==============================");

                var categorias = _context.Categorias.Include(c => c.Produtos).ToList();

                if (categorias == null || categorias.Count == 0)
                {
                    return NotFound("Categorias de Produtos Não Encontradas");
                }

                return categorias;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }
        
        //Servico
        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        } 

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                var categorias = _context.Categorias.AsNoTracking().ToList();

                if (categorias == null || categorias.Count == 0)
                {
                    return NotFound("Categorias Não Encontradas");
                }

                return categorias;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpGet("{id:int}", Name = "obterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                _logger.LogInformation($"=============================GET api/categorias/id = {id}==============================");
                var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

                if (categoria == null)
                {
                    _logger.LogInformation($"============ GET api/categorias/id = {id}NOT FOUND =======================");
                    return NotFound("Categoria Não Localizada");
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            try
            {
                if (categoria == null)
                    return BadRequest();

                _context.Categorias.Add(categoria);
                _context.SaveChanges();

                return new CreatedAtRouteResult("obterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult Put(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest();
                }

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(categoria);
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
                var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound("Categoria Não Localizada");
                }

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }
    }
}
