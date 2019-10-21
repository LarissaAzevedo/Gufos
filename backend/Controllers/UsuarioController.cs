using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers {
     //definindo a rota do controller e informando que é um controle de API
    [Route ("api/[Controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase {
        //instanciando o contexto
        GufosContext _contexto = new GufosContext ();

        //iniciando o método REST (CRUD)

        //GET: api/Usuario
        //método assincrono executa vários processos simultaneamente
        /// <summary>
        /// Pega os usuários cadastrados
        /// </summary>
        /// <returns>Lista de usuários</returns>
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get () {
            var Usuarios = await _contexto.Usuario.ToListAsync ();

            if (Usuarios == null) {
                return NotFound ();
            }
            return Usuarios;

        }

        //GET: api/Usuario/2
        /// <summary>
        /// Pega um usuário específico 
        /// </summary>
        /// <param name="id">id do usuário</param>
        /// <returns>usuário específico</returns>
        [HttpGet ("{id}")]
        public async Task<ActionResult<Usuario>> Get (int id) {
            var Usuario = await _contexto.Usuario.FindAsync (id);

            if (Usuario == null) {
                return NotFound ();
            }
            return Usuario;

        }

        //POST api/Usuario
        /// <summary>
        /// Adiciona um novo usuário
        /// </summary>
        /// <param name="Usuario">nome do usuário</param>
        /// <returns>tabela atualizada</returns>
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post (Usuario Usuario) {
            try {
                //adicionando o objeto Usuario dentro do contexto
                //salva as mudanças feitas
                //tratamento de ataques de Sql Injection
                await _contexto.AddAsync (Usuario);
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return Usuario;
        }

        /// <summary>
        /// Atualiza a tabela
        /// </summary>
        /// <param name="id">id do usuário</param>
        /// <param name="Usuario">nome do usuário</param>
        /// <returns>tabela atualizada</returns>
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Usuario Usuario) {

            if (id != Usuario.UsuarioId) {
                return BadRequest ();
            }

            //dps de verificada a entrada, faz uma comparação dos atributos que foram mudados
            _contexto.Entry (Usuario).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {

                //verificando se o objeto inserido realmente existe no banco
                var Usuario_valida = await _contexto.Usuario.FindAsync (id);

                if (Usuario_valida == null) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        //DELETE api/Usuario/id
        /// <summary>
        /// Deleta um usuário específico
        /// </summary>
        /// <param name="id">id do usuário</param>
        /// <returns>Tabela atualizada</returns>
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Usuario>> Delete (int id) {

            var Usuario = await _contexto.Usuario.FindAsync (id);
            if (Usuario == null) {
                return NotFound();
            }

            //remove o conteúdo
            _contexto.Usuario.Remove(Usuario);
            await _contexto.SaveChangesAsync();

            return Usuario;
        }

    }
}