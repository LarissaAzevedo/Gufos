using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Repositories;

namespace backend.Controllers {
    [Route ("api/[Controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase {
        
        TipoUsuarioRepository _repositorio = new TipoUsuarioRepository();

        
        /// <summary>
        /// Pega os tipos de usuário cadastrados
        /// </summary>
        /// <returns>Lista de tipos de usuários</returns>
        [HttpGet]
        public async Task<ActionResult<List<TipoUsuario>>> Get () {
            var TipoUsuarios = await _repositorio.Listar ();

            if (TipoUsuarios == null) {
                return NotFound ();
            }
            return TipoUsuarios;
        }

        //GET: api/TipoUsuario/2
        /// <summary>
        /// Pega um tipo de usuário específico
        /// </summary>
        /// <param name="id">id do tipo do usuário</param>
        /// <returns>tipo de usuário específiico</returns>
        [HttpGet ("{id}")]
        public async Task<ActionResult<TipoUsuario>> Get (int id) {
            var TipoUsuario = await _repositorio.BuscarPorId (id);

            if (TipoUsuario == null) {
                return NotFound ();
            }
            return TipoUsuario;
        }

        //POST api/TipoUsuario
        /// <summary>
        /// Adiciona um novo tipo de usuário
        /// </summary>
        /// <param name="TipoUsuario">nome do tipo de usuário</param>
        /// <returns>tabela tualizada</returns>
        [HttpPost]
        public async Task<ActionResult<TipoUsuario>> Post (TipoUsuario TipoUsuario) {
            try {
                await _repositorio.Salvar (TipoUsuario);
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return TipoUsuario;
        }

        /// <summary>
        /// Atualiza os dados de um tipo de usuário específico
        /// </summary>
        /// <param name="id">id do tipo de usuário</param>
        /// <param name="TipoUsuario">nome do tipo de usuário</param>
        /// <returns>tabela atualizada</returns>
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, TipoUsuario TipoUsuario) {

            if (id != TipoUsuario.TipoUsuarioId) {
                return BadRequest ();
            }

            try {
                await _repositorio.Alterar (TipoUsuario);
            } catch (DbUpdateConcurrencyException) {

                var TipoUsuario_valida = await _repositorio.BuscarPorId (id);

                if (TipoUsuario_valida == null) {
                    return NotFound ();
                } else {
                    throw;
                }
            }
            return NoContent ();
        }

        //DELETE api/TipoUsuario/id
        /// <summary>
        /// Deleta um tipo de usuário específico
        /// </summary>
        /// <param name="id">id do tipo de usuário</param>
        /// <returns>tabela atualizada</returns>
        [HttpDelete ("{id}")]
        public async Task<ActionResult<TipoUsuario>> Delete (int id) {

            var TipoUsuario = await _repositorio.BuscarPorId (id);
            if (TipoUsuario == null) {
                return NotFound();
            }
            await _repositorio.Excluir(TipoUsuario);
            return TipoUsuario;
        }

    }
}