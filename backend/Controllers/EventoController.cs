using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Domains;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// para adicionar a árvore do objeto adicionamos uma nova biblioteca JSON
//dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson

namespace backend.Controllers {
    
    [Route ("api/[Controller]")]
    [ApiController]
    public class EventoController : ControllerBase {
        EventoRepository _repositorio = new EventoRepository();

        //iniciando o método REST (CRUD)

        //GET: api/Evento
        //método assincrono executa vários processos simultaneamente
        /// <summary>
        /// Pega os eventos cadastrado
        /// </summary>
        /// <returns>Lista de eventos</returns>
        [HttpGet]
        public async Task<ActionResult<List<Evento>>> Get () {
            var eventos = await _repositorio.Listar();

            if (eventos == null) {
                return NotFound ();
            }
            return eventos;

        }

        //GET: api/Evento/2
        /// <summary>
        /// Pega um evento específico
        /// </summary>
        /// <param name="id">id do evento</param>
        /// <returns>Evento específico</returns>
        [HttpGet ("{id}")]
        public async Task<ActionResult<Evento>> Get (int id) {
            var evento = await _repositorio.BuscarPorId(id);
            if (evento == null) {
                return NotFound ();
            }
            return evento;

        }

        //POST api/evento
        /// <summary>
        /// Adiciona um novo evento
        /// </summary>
        /// <param name="evento">Nome do evento</param>
        /// <returns>Evento adicionado</returns>
        [HttpPost]
        public async Task<ActionResult<Evento>> Post (Evento evento) {
            try {
                await _repositorio.Salvar (evento);
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return evento;
        }

        /// <summary>
        /// Atualiza um dado de um evento específico
        /// </summary>
        /// <param name="id">id do evento</param>
        /// <param name="evento">Nome do evento</param>
        /// <returns>Evento atualizado</returns>
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Evento evento) {

            if (id != evento.EventoId) {
                return BadRequest ();
            }

            try {
                await _repositorio.Alterar (evento);
            } catch (DbUpdateConcurrencyException) {

                //verificando se o objeto inserido realmente existe no banco
                var evento_valida = await _repositorio.BuscarPorId (id);

                if (evento_valida == null) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        //DELETE api/evento/id
        /// <summary>
        /// Deleta um evento da tabela
        /// </summary>
        /// <param name="id">id do evento</param>
        /// <returns>Dado deletado</returns>
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Evento>> Delete (int id) {

            var evento = await _repositorio.BuscarPorId (id);
            if (evento == null) {
                return NotFound();
            }

            //remove o conteúdo
            await _repositorio.Excluir(evento);

            return evento;
        }

    }
}