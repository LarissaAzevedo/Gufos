using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// para adicionar a árvore do objeto adicionamos uma nova biblioteca JSON
//dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson

namespace backend.Controllers {
    //definindo a rota do controller e informando que é um controle de API
    //informa que é um controller de API
    [Route ("api/[Controller]")]
    [ApiController]
    public class EventoController : ControllerBase {
        //instanciando o contexto
        GufosContext _contexto = new GufosContext ();

        //iniciando o método REST (CRUD)

        //GET: api/Evento
        //método assincrono executa vários processos simultaneamente
        /// <summary>
        /// Pega os eventos cadastrado
        /// </summary>
        /// <returns>Lista de eventos</returns>
        [HttpGet]
        public async Task<ActionResult<List<Evento>>> Get () {
            var eventos = await _contexto.Evento.Include("Categoria").Include("Localizacao").ToListAsync();

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
            var evento = await _contexto.Evento.Include("Categoria").Include("Localizacao").FirstOrDefaultAsync(e => e.EventoId == id);

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
                //adicionando o objeto evento dentro do contexto
                //salva as mudanças feitas
                //tratamento de ataques de Sql Injection
                await _contexto.AddAsync (evento);
                await _contexto.SaveChangesAsync ();
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

            //dps de verificada a entrada, faz uma comparação dos atributos que foram mudados
            _contexto.Entry (evento).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {

                //verificando se o objeto inserido realmente existe no banco
                var evento_valida = await _contexto.Evento.FindAsync (id);

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

            var evento = await _contexto.Evento.FindAsync (id);
            if (evento == null) {
                return NotFound();
            }

            //remove o conteúdo
            _contexto.Evento.Remove(evento);
            await _contexto.SaveChangesAsync();

            return evento;
        }

    }
}