using PicPaySimplificado.Application.DTOs;

namespace PicPaySimplificado.Application.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO> CadastrarUsuario(UsuarioCadastroDTO usuarioCadastro);
    }
}