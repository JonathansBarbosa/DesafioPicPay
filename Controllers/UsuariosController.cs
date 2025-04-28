using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PicPaySimplificado.Application;
using PicPaySimplificado.Domain;
using PicPaySimplificado.Application.DTOs;
using PicPaySimplificado.Application.Services;


namespace PicPaySimplificado.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarUsuario([FromBody] UsuarioCadastroDTO usuarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var resultado = await _usuarioService.CadastrarUsuarioAsync(usuarioDto);

                if (!resultado.Sucesso)
                    return BadRequest(resultado.Mensagem);

                return CreatedAtAction(nameof(CadastrarUsuario), new { id = resultado.Usuario.Id }, resultado.Usuario);
            }
            catch (Exception ex)
            {
                // Log da exceção
                return StatusCode(500, "Ocorreu um erro interno ao cadastrar o usuário.");
            }
        }
    }
}
