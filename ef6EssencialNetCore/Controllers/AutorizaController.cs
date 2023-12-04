// System 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// Project
using ef6EssencialNetCore.DTO;

// Microsoft 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ef6EssencialNetCore.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly IConfiguration _configuration;

        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em : " + DateTime.Now.ToLongDateString();
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO userInfo)
        {
            var user = new IdentityUser
            {
                UserName = userInfo.Email,
                Email = userInfo.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userInfo.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);
            return Ok(BuildToken(userInfo));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return Ok(BuildToken(userInfo));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido...");
                return BadRequest(ModelState);
            }
        }

        //Metodo para Gerar o Token
        private UsuarioToken BuildToken(UsuarioDTO userInfo)
        {
            //define declarações do usuário
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("VALOR_A", "VALOR_B"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Gera Chave
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );
            
            //Gera a assinatura do token
            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            );

            //Define tempo de expiração
            var expiration = _configuration["TokenConfigurations:ExpireHours"];
            var expirationTime = DateTime.UtcNow.AddHours(double.Parse(expiration));
            
            //Gera o Toke JWT
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfigurations:Issuer"],
                audience: _configuration["TokenConfigurations:Audience"],
                claims: claims,
                expires: expirationTime,
                signingCredentials: credentials
            );

            //retorna o objeto UsuarioToken
            return new UsuarioToken()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expirationTime,
                Message = "Token JWT OK"
            };
        }
    }
