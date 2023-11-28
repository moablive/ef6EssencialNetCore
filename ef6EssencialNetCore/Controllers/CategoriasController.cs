using System.Text.Json;
using AutoMapper;
using ef6EssencialNetCore.DTO;
using ef6EssencialNetCore.Helpers.Pagination;
using ef6EssencialNetCore.Models;
using ef6EssencialNetCore.Repository;
using ef6EssencialNetCore.Services;
using Microsoft.AspNetCore.Mvc;


namespace ef6EssencialNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public CategoriasController(IUnitOfWork context, IConfiguration configuration, IMapper mapper, ILogger<CategoriasController> logger)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        //_configuration
        [HttpGet("author")]
        public string GetAuthor()
        {
            var author = _configuration["author"];
            var conexao = _configuration["ConnectionStrings:DefaultConnection"];

            return $"Author : {author} + conexao {conexao}";
        }

        //Servico
        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        } 

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            try
            {
                _logger.LogInformation("=============================GET api/categorias/produtos==============================");

                var categorias = await _context.CategoriaRepository.GetCategoriasProdutos();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            
                if (categoriasDto == null)
                {
                    return NotFound("Categorias de Produtos Não Encontradas");
                }

                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriaParameter categoriaParameter)
        {
            try
            {
                //Mapeamento Paginado
                var categorias = await _context.CategoriaRepository.GetCategorias(categoriaParameter);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };

                Response.Headers.Add("x-Pagination", JsonSerializer.Serialize(metadata));
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                
                if (categoriasDto == null)
                {
                    return NotFound("Categorias Não Encontradas");
                }

                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpGet("{id:int}", Name = "obterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                _logger.LogInformation($"=============================GET api/categorias/id = {id}==============================");

                var categoria = await _context.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (categoria == null)
                {
                    _logger.LogInformation($"============ GET api/categorias/id = {id}NOT FOUND =======================");
                    return NotFound("Categoria Não Localizada");
                }

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
                return Ok(categoriaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoriaDTO categoriaDto)
        {
            try
            {   
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                _context.CategoriaRepository.Add(categoria);
                
                if (categoria == null)
                {
                    return BadRequest();
                }

                await _context.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest();
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _context.CategoriaRepository.Update(categoria);
                await _context.Commit();

                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            try
            {
                var categoria = await _context.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if(categoria == null)
                {
                    return NotFound("Categoria Não Localizada");
                }

                _context.CategoriaRepository.Delete(categoria);
                await _context.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return Ok(categoriaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao Tratar a sua Solicitação. Favor tentar novamente mais tarde");
            }
        }
    }
}
