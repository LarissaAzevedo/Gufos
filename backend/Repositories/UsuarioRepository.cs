using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Domains;
using backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class UsuarioRepository : IUsuario
    {
        public async Task<Usuario> Alterar(Usuario usuario)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Usuario> BuscarPorId(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Usuario> Excluir(Usuario usuario)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Usuario>> Listar()
        {
            using(GufosContext _contexto = new GufosContext()){
                return await _contexto.Usuario.ToListAsync();
            }
        }

        public async Task<Usuario> Salvar(Usuario usuario)
        {
            throw new System.NotImplementedException();
        }
    }
}