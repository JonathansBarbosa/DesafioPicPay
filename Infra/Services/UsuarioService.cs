using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Application.DTOs;
using PicPaySimplificado.Domain;
using PicPaySimplificado.Domain.Entities;
using PicPaySimplificado.Infrastructure;
using PicPaySimplificado.Infrastructure.Persistence;
using System.Security.Cryptography;
using System.Text;

namespace PicPaySimplificado.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioDTO> CadastrarUsuario(UsuarioCadastroDTO usuarioCadastro)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuarioCadastro.Email))
                throw new Exception("E-mail já cadastrado.");

            string senhaHash = GerarHashSenha(usuarioCadastro.Senha);

            var usuario = new Usuario
            {
                Nome = usuarioCadastro.Nome,
                Email = usuarioCadastro.Email,
                Senha = senhaHash,
                Tipo = usuarioCadastro.TipoUsuario
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioDTO { Id = usuario.Id, Nome = usuario.Nome, Email = usuario.Email, Tipo = usuario.Tipo };
        }

        private string GerarHashSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
