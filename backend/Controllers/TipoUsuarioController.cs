using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers {
     //definindo a rota do controller e informando que é um controle de API
    [Route ("api/[Controller]")]
    [ApiController]
    public class TipoUsuarioController : ControllerBase {
        //instanciando o contexto
        GufosContext _contexto = new GufosContext ();

        //iniciando o método REST (CRUD)

        //GET: api/TipoUsuario
        //método assincrono executa vários processos simultaneamente
        /// <summary>
        /// Pega os tipos de usuário cadastrados
        /// </summary>
        /// <returns>Lista de usuários</returns>
        [HttpGet]
        public async Task<ActionResult<List<TipoUsuario>>> Get () {
            var TipoUsuarios = await _contexto.TipoUsuario.ToListAsync ();

            if (TipoUsuarios == null) {
                return NotFound ();
            }
            return TipoUsuarios;

        }

        //GET: api/TipoUsuario/2
        /// <summary>
        /// Pega um tipo de usuário específico pelo id
        /// </summary>
        /// <param name="id">id do tipo do usuário</param>
        /// <returns>tipo de usuário específiico</returns>
        [HttpGet ("{id}")]
        public async Task<ActionResult<TipoUsuario>> Get (int id) {
            var TipoUsuario = await _contexto.TipoUsuario.FindAsync (id);

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
                //adicionando o objeto TipoUsuario dentro do contexto
                //salva as mudanças feitas
                //tratamento de ataques de Sql Injection
                await _contexto.AddAsync (TipoUsuario);
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return TipoUsuario;
        }

        /// <summary>
        /// Atualiza a tabela
        /// </summary>
        /// <param name="id">id do tipo de usuário</param>
        /// <param name="TipoUsuario">nome do tipo de usuário</param>
        /// <returns>tabela atualizada</returns>
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, TipoUsuario TipoUsuario) {

            if (id != TipoUsuario.TipoUsuarioId) {
                return BadRequest ();
            }

            //dps de verificada a entrada, faz uma comparação dos atributos que foram mudados
            _contexto.Entry (TipoUsuario).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {

                //verificando se o objeto inserido realmente existe no banco
                var TipoUsuario_valida = await _contexto.TipoUsuario.FindAsync (id);

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

            var TipoUsuario = await _contexto.TipoUsuario.FindAsync (id);
            if (TipoUsuario == null) {
                return NotFound();
            }

            //remove o conteúdo
            _contexto.TipoUsuario.Remove(TipoUsuario);
            await _contexto.SaveChangesAsync();

            return TipoUsuario;
        }

    }
}