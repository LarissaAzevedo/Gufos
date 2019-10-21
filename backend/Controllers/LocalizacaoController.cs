using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers {
    //definindo a rota do controller e informando que é um controle de API
    [Route ("api/[Controller]")]
    [ApiController]
    public class LocalizacaoController : ControllerBase {
        //instanciando o contexto
        GufosContext _contexto = new GufosContext ();

        //iniciando o método REST (CRUD)

        //GET: api/Localizacao
        //método assincrono executa vários processos simultaneamente
        /// <summary>
        /// Pega as localizações cadastradas
        /// </summary>
        /// <returns>Lista de localizações</returns>
        [HttpGet]
        public async Task<ActionResult<List<Localizacao>>> Get () {
            var Localizacaos = await _contexto.Localizacao.ToListAsync ();

            if (Localizacaos == null) {
                return NotFound ();
            }
            return Localizacaos;

        }

        //GET: api/Localizacao/2
        /// <summary>
        /// Pega uma localização específica pelo id
        /// </summary>
        /// <param name="id">id da localização</param>
        /// <returns>localização espefícica</returns>
        [HttpGet ("{id}")]
        public async Task<ActionResult<Localizacao>> Get (int id) {
            var Localizacao = await _contexto.Localizacao.FindAsync (id);

            if (Localizacao == null) {
                return NotFound ();
            }
            return Localizacao;

        }

        //POST api/Localizacao
        /// <summary>
        /// Adiciona uma nova localização
        /// </summary>
        /// <param name="Localizacao">Nome da localização</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Localizacao>> Post (Localizacao Localizacao) {
            try {
                //adicionando o objeto Localizacao dentro do contexto
                //salva as mudanças feitas
                //tratamento de ataques de Sql Injection
                await _contexto.AddAsync (Localizacao);
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return Localizacao;
        }

        /// <summary>
        /// Atualiza a tabela
        /// </summary>
        /// <param name="id">id da localização</param>
        /// <param name="Localizacao">Nome da localização</param>
        /// <returns>Tabela atualizada</returns>
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Localizacao Localizacao) {

            if (id != Localizacao.LocalizacaoId) {
                return BadRequest ();
            }

            //dps de verificada a entrada, faz uma comparação dos atributos que foram mudados
            _contexto.Entry (Localizacao).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {

                //verificando se o objeto inserido realmente existe no banco
                var Localizacao_valida = await _contexto.Localizacao.FindAsync (id);

                if (Localizacao_valida == null) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        //DELETE api/Localizacao/id
        /// <summary>
        /// deleta uma localização
        /// </summary>
        /// <param name="id">id da localizacao</param>
        /// <returns>tabela atualizada</returns>
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Localizacao>> Delete (int id) {

            var Localizacao = await _contexto.Localizacao.FindAsync (id);
            if (Localizacao == null) {
                return NotFound();
            }

            //remove o conteúdo
            _contexto.Localizacao.Remove(Localizacao);
            await _contexto.SaveChangesAsync();

            return Localizacao;
        }

    }
}