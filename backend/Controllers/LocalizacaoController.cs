using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers {
    //definindo a rota do controller e informando que é um controle de API
    //informa que é um controller de API
    [Route ("api/[Controller]")]
    [ApiController]
    public class LocalizacaoController : ControllerBase {
        //instanciando o contexto
        GufosContext _contexto = new GufosContext ();

        //iniciando o método REST (CRUD)

        //GET: api/Localizacao
        //método assincrono executa vários processos simultaneamente
        [HttpGet]
        public async Task<ActionResult<List<Localizacao>>> Get () {
            var localizcoes = await _contexto.Localizacao.ToListAsync ();

            if (localizcoes == null) {
                return NotFound ();
            }
            return localizcoes;

        }

        //GET: api/Localizacao/2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Localizacao>> Get (int id) {
            var localizacao = await _contexto.Localizacao.FindAsync (id);

            if (localizacao == null) {
                return NotFound ();
            }
            return localizacao;

        }

        //POST api/localizacao
        [HttpPost]
        public async Task<ActionResult<Localizacao>> Post (Localizacao localizacao) {
            try {
                //adicionando o objeto localizacao dentro do contexto
                //salva as mudanças feitas
                //tratamento de ataques de Sql Injection
                await _contexto.AddAsync (localizacao);
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return localizacao;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Localizacao localizacao) {

            if (id != localizacao.LocalizacaoId) {
                return BadRequest ();
            }

            //dps de verificada a entrada, faz uma comparação dos atributos que foram mudados
            _contexto.Entry (localizacao).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {

                //verificando se o objeto inserido realmente existe no banco
                var localizacao_valida = await _contexto.Localizacao.FindAsync (id);

                if (localizacao_valida == null) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        //DELETE api/localizacao/id
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Localizacao>> Delete (int id) {

            var localizacao = await _contexto.Localizacao.FindAsync (id);
            if (localizacao == null) {
                return NotFound();
            }

            //remove o conteúdo
            _contexto.Localizacao.Remove(localizacao);
            await _contexto.SaveChangesAsync();

            return localizacao;
        }

    }
}