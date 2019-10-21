using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers {
     //definindo a rota do controller e informando que é um controle de API
    [Route ("api/[Controller]")]
    [ApiController]
    public class PresencaController : ControllerBase {
        //instanciando o contexto
        GufosContext _contexto = new GufosContext ();

        //iniciando o método REST (CRUD)

        //GET: api/Presenca
        //método assincrono executa vários processos simultaneamente
        /// <summary>
        /// Pega as presenças cadastradas
        /// </summary>
        /// <returns>Lista de presença</returns>
        [HttpGet]
        public async Task<ActionResult<List<Presenca>>> Get () {
            var Presencas = await _contexto.Presenca.ToListAsync ();

            if (Presencas == null) {
                return NotFound ();
            }
            return Presencas;

        }

        //GET: api/Presenca/2
        /// <summary>
        /// Pega uma presença específica pelo is
        /// </summary>
        /// <param name="id">id da presença</param>
        /// <returns>Presença específica</returns>
        [HttpGet ("{id}")]
        public async Task<ActionResult<Presenca>> Get (int id) {
            var Presenca = await _contexto.Presenca.FindAsync (id);

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
                //adicionando o objeto Presenca dentro do contexto
                //salva as mudanças feitas
                //tratamento de ataques de Sql Injection
                await _contexto.AddAsync (Presenca);
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return Presenca;
        }

        /// <summary>
        /// atualiza a tabela
        /// </summary>
        /// <param name="id">id da presença</param>
        /// <param name="Presenca">nome da presença</param>
        /// <returns>tabela atualizada</returns>
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Presenca Presenca) {

            if (id != Presenca.PresencaId) {
                return BadRequest ();
            }

            //dps de verificada a entrada, faz uma comparação dos atributos que foram mudados
            _contexto.Entry (Presenca).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {

                //verificando se o objeto inserido realmente existe no banco
                var Presenca_valida = await _contexto.Presenca.FindAsync (id);

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
        /// Deleta uma presença
        /// </summary>
        /// <param name="id">id da presença</param>
        /// <returns>tabela atualizada</returns>
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Presenca>> Delete (int id) {

            var Presenca = await _contexto.Presenca.FindAsync (id);
            if (Presenca == null) {
                return NotFound();
            }

            //remove o conteúdo
            _contexto.Presenca.Remove(Presenca);
            await _contexto.SaveChangesAsync();

            return Presenca;
        }

    }
}