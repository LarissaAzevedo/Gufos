using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase {

        //instanciando o contexto
        GufosContext _contexto = new GufosContext ();

        //definindo variável para percorrer os métodos com as configurações obtidas no appsettings.json
        private IConfiguration _config;

        //definido método construtor para acessar essas configs
        public LoginController (IConfiguration config) {
            //ao instanciar a classe, a configuração de fora é recebida dentro
            _config = config;
        }
        //método para validar o usuário na aplicação

        private Usuario ValidaUsuario (Usuario login) {
            var usuario = _contexto.Usuario.FirstOrDefault (
                u => u.Email == login.Email && u.Senha == login.Senha
            );

            if (usuario != null) {
                usuario = login;
            }

            return usuario;
        }

        //gerando o token
        private string GeraToken (Usuario userInfo) {
            //definida a criptografia do token
            var securityKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt : Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //definindo as Claims (Dados da sessão)
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.NameId, userInfo.Nome),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //confurando o token e seu tempo de vida
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials : credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //usamos essa anotação para ignorar a autenticação nesse método
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] Usuario login){
            IActionResult response  = Unauthorized();
            var user = ValidaUsuario(login);

            if(user != null){
                var tokenString = GeraToken(user);
                response = Ok(new {token = tokenString});
            }

            return response;
        }
    }
}