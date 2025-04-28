using PicPaySimplificado.Application.DTOs;
using System.Threading.Tasks;

namespace PicPaySimplificado.Application.Services
{
    public interface IUsuarioService
    {
        Task<ResultadoCadastroUsuario> CadastrarUsuario(UsuarioCadastroDTO usuarioCadastro);
    }
}

