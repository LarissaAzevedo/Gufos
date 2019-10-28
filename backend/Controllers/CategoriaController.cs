using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Domains;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers {
    //definindo a rota do controller e informando que é um controle de API
    //informa que é um controller de API
    [Route ("api/[Controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase {
        //instanciando o contexto
        // GufosContext _contexto = new GufosContext ();
        CategoriaRepository _repositorio = new CategoriaRepository();
    
        //iniciando o método REST (CRUD)

        //GET: api/Categoria
        //método assincrono executa vários processos simultaneamente
        /// <summary>
        /// Pega as categorias cadastradas
        /// </summary>
        /// <returns>Lista de categorias</returns>
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get () {
            var categorias = await _repositorio.Listar();

            if (categorias == null) {
                return NotFound ();
            }
            return categorias;

        }

        //GET: api/Categoria/2
        /// <summary>
        /// Pega uma categoria específica
        /// </summary>
        /// <param name="id">id da categoria</param>
        /// <returns>Categoria específica do id</returns>
        [HttpGet ("{id}")]
        public async Task<ActionResult<Categoria>> Get (int id) {
            var categoria = await _repositorio.BuscarPorId (id);

            if (categoria == null) {
                return NotFound ();
            }
            return categoria;

        }

        //POST api/categoria
        /// <summary>
        /// Adiciona uma nova categoria
        /// </summary>
        /// <param name="categoria">Nome da categoria</param>
        /// <returns>Categoria adicionada</returns>
        [HttpPost]
        public async Task<ActionResult<Categoria>> Post (Categoria categoria) {
            try {
               await _repositorio.Salvar(categoria);

            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return categoria;
        }
        /// <summary>
        /// Atualiza os dados de uma categoria específica
        /// </summary>
        /// <param name="id">Id da categoria</param>
        /// <param name="categoria">Nome da categoria</param>
        /// <returns>Tabela atualizada</returns>
        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Categoria categoria) {

            if (id != categoria.CategoriaId) {
                return BadRequest ();
            }
    
            try {
                await _repositorio.Alterar(categoria);
                
            } catch (DbUpdateConcurrencyException) {

                //verificando se o objeto inserido realmente existe no banco
                var categoria_valida = await _repositorio.BuscarPorId (id);

                if (categoria_valida == null) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        //DELETE api/categoria/id
        /// <summary>
        /// Deleta um dado específico
        /// </summary>
        /// <param name="id">id da categoria</param>
        /// <returns>Dado excluído e tabela atualizada</returns>
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Categoria>> Delete (int id) {

            var categoria = await _repositorio.BuscarPorId(id);
            if (categoria == null) {
                return NotFound();
            }

            await _repositorio.Excluir(categoria);
            return categoria;
        }

    }
}