using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Domains;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers {
    [Route ("api/[Controller]")]
    [ApiController]
    public class PresencaController : ControllerBase {
        
        PresencaRepository _repositorio = new PresencaRepository();

        //iniciando o método REST (CRUD)

        //GET: api/Presenca
        //método assincrono executa vários processos simultaneamente
        /// <summary>
        /// Pega as presenças cadastradas
        /// </summary>
        /// <returns>Lista de presença</returns>
        [HttpGet]
        public async Task<ActionResult<List<Presenca>>> Get () {
            var Presencas = await _repositorio.Listar();

            if (Presencas == null) {
                return NotFound ();
            }
            return Presencas;
        }

        //GET: api/Presenca/2
        /// <summary>
        /// Pega uma presença específica
        /// </summary>
        /// <param name="id">id da presença</param>
        /// <returns>Presença específica</returns>
        [HttpGet ("{id}")]
        public async Task<ActionResult<Presenca>> Get (int id) {
            var Presenca = await _repositorio.BuscarPorId(id);

            if (Presenca == null) {
                return NotFound ();
            }
            return Presenca;
        }

        //POST api/Presenca
        /// <summary>
        /// Adiciona uma nova presença
        /// </summary>
        /// <param name="Presenca">nome da presença</param>
        /// <returns>presença adicionada</returns>
        [HttpPost]
        public async Task<ActionResult<Presenca>> Post (Presenca Presenca) {
            try {
                
                await _repositorio.Salvar (Presenca);
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return Presenca;
        }

        /// <summary>
        /// atualiza os dados de uma presença específica
        /// </summary>
        /// <param name="id">id da presença</param>
        /// <param name="Presenca">nome da presença</param>
        /// <returns>tabela atualizada</returns>
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Presenca Presenca) {

            if (id != Presenca.PresencaId) {
                return BadRequest ();
            }

            try {
                await _repositorio.Alterar(Presenca);
            } catch (DbUpdateConcurrencyException) {

                var Presenca_valida = await _repositorio.BuscarPorId(id);

                if (Presenca_valida == null) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        //DELETE api/Presenca/id
        /// <summary>
        /// Deleta uma presença específica
        /// </summary>
        /// <param name="id">id da presença</param>
        /// <returns>tabela atualizada</returns>
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Presenca>> Delete (int id) {

            var Presenca = await _repositorio.BuscarPorId(id);
            if (Presenca == null) {
                return NotFound();
            }
            await _repositorio.Excluir(Presenca);

            return Presenca;
        }

    }
}