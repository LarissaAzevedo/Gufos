using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Domains;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers {
    [Route ("api/[Controller]")]
    [ApiController]
    public class LocalizacaoController : ControllerBase {
        
        LocalizacaoRepository _repositorio = new LocalizacaoRepository ();

        //GET: api/Localizacao
        /// <summary>
        /// Pega as localizações cadastradas
        /// </summary>
        /// <returns>Lista de localizações</returns>
        [HttpGet]
        public async Task<ActionResult<List<Localizacao>>> Get () {
            var Localizacaos = await _repositorio.Listar ();
            if (Localizacaos == null) {
                return NotFound ();
            }
            return Localizacaos;
        }

        //GET: api/Localizacao/2
        /// <summary>
        /// Pega os dados de uma localização específica
        /// </summary>
        /// <param name="id">id da localização</param>
        /// <returns>localização espefícica</returns>
        [HttpGet ("{id}")]
        public async Task<ActionResult<Localizacao>> Get (int id) {
            var Localizacao = await _repositorio.BuscarPorId (id);
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

                await _repositorio.Salvar (Localizacao);
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return Localizacao;
        }

        /// <summary>
        /// Atualiza os dados de uma determinada localização
        /// </summary>
        /// <param name="id">id da localização</param>
        /// <param name="Localizacao">Nome da localização</param>
        /// <returns>dados atualizados</returns>
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Localizacao Localizacao) {

            if (id != Localizacao.LocalizacaoId) {
                return BadRequest ();
            }
            try {
                await _repositorio.Alterar(Localizacao);
            } catch (DbUpdateConcurrencyException) {

                var Localizacao_valida = await _repositorio.BuscarPorId(id);
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
        /// deleta uma localização específica
        /// </summary>
        /// <param name="id">id da localizacao</param>
        /// <returns>dado deletado</returns>
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Localizacao>> Delete (int id) {

            var Localizacao = await _repositorio.BuscarPorId(id);
            if (Localizacao == null) {
                return NotFound ();
            }
           await _repositorio.Excluir (Localizacao);
            return Localizacao;
        }

    }
}