using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Repositories;

namespace backend.Controllers {
    [Route ("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase {
        
        UsuarioRepository _repositorio = new UsuarioRepository();

        /// <summary>
        /// Pega os usuários cadastrados
        /// </summary>
        /// <returns>Lista de usuários</returns>
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get () {
            var Usuarios = await _repositorio.Listar ();

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
            var Usuario = await _repositorio.BuscarPorId(id);

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
                
                await _repositorio.Salvar (Usuario);
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return Usuario;
        }

        /// <summary>
        /// Atualiza os dados de um usuário específico
        /// </summary>
        /// <param name="id">id do usuário</param>
        /// <param name="Usuario">nome do usuário</param>
        /// <returns>tabela atualizada</returns>
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Usuario Usuario) {

            if (id != Usuario.UsuarioId) {
                return BadRequest ();
            }

            try {
                await _repositorio.Alterar (Usuario);
            } catch (DbUpdateConcurrencyException) {

                var Usuario_valida = await _repositorio.BuscarPorId(id);

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

            var Usuario = await _repositorio.BuscarPorId (id);
            if (Usuario == null) {
                return NotFound();
            }
            await _repositorio.Excluir(Usuario);
            return Usuario;
        }

    }
}