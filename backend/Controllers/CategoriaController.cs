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
    public class CategoriaController : ControllerBase {
        //instanciando o contexto
        GufosContext _contexto = new GufosContext ();

        //iniciando o método REST (CRUD)

        //GET: api/Categoria
        //método assincrono executa vários processos simultaneamente
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get () {
            var categorias = await _contexto.Categoria.ToListAsync ();

            if (categorias == null) {
                return NotFound ();
            }
            return categorias;

        }

        //GET: api/Categoria/2
        [HttpGet ("{id}")]
        public async Task<ActionResult<Categoria>> Get (int id) {
            var categoria = await _contexto.Categoria.FindAsync (id);

            if (categoria == null) {
                return NotFound ();
            }
            return categoria;

        }

        //POST api/categoria
        [HttpPost]
        public async Task<ActionResult<Categoria>> Post (Categoria categoria) {
            try {
                //adicionando o objeto categoria dentro do contexto
                //salva as mudanças feitas
                //tratamento de ataques de Sql Injection
                await _contexto.AddAsync (categoria);
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
            return categoria;
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult> Put (int id, Categoria categoria) {

            if (id != categoria.CategoriaId) {
                return BadRequest ();
            }

            //dps de verificada a entrada, faz uma comparação dos atributos que foram mudados
            _contexto.Entry (categoria).State = EntityState.Modified;

            try {
                await _contexto.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException) {

                //verificando se o objeto inserido realmente existe no banco
                var categoria_valida = await _contexto.Categoria.FindAsync (id);

                if (categoria_valida == null) {
                    return NotFound ();
                } else {
                    throw;
                }
            }

            return NoContent ();
        }

        //DELETE api/categoria/id
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Categoria>> Delete (int id) {

            var categoria = await _contexto.Categoria.FindAsync (id);
            if (categoria == null) {
                return NotFound();
            }

            //remove o conteúdo
            _contexto.Categoria.Remove(categoria);
            await _contexto.SaveChangesAsync();

            return categoria;
        }

    }
}