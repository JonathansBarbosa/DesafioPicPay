using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Application.DTOs;
using PicPaySimplificado.Domain.Entities;
using PicPaySimplificado.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace PicPaySimplificado.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResultadoCadastroUsuario> CadastrarUsuario(UsuarioCadastroDTO usuarioCadastro)
        {
            // Verificar se já existe um usuário com o mesmo e-mail
            if (await _context.Usuarios.AnyAsync(u => u.Email == usuarioCadastro.Email))
            {
                return new ResultadoCadastroUsuario
                {
                    Sucesso = false,
                    Mensagem = "E-mail já cadastrado."
                };
            }

            // Verificar se já existe um usuário com o mesmo CPF/CNPJ
            if (await _context.Usuarios.AnyAsync(u => u.CpfCnpj == usuarioCadastro.CpfCnpj))
            {
                return new ResultadoCadastroUsuario
                {
                    Sucesso = false,
                    Mensagem = "CPF/CNPJ já cadastrado."
                };
            }

            // Criar o novo usuário
            var novoUsuario = new Usuario
            {
                Nome = usuarioCadastro.Nome,
                Email = usuarioCadastro.Email,
                CpfCnpj = usuarioCadastro.CpfCnpj,
                Tipo = usuarioCadastro.TipoUsuario,
                Saldo = 0,
                IsLojista = usuarioCadastro.TipoUsuario.ToLower() == "lojista"
            };

            // Validar CPF/CNPJ usando o método da própria entidade
            if (!novoUsuario.ValidarCpfCnpj())
            {
                return new ResultadoCadastroUsuario
                {
                    Sucesso = false,
                    Mensagem = "CPF/CNPJ inválido."
                };
            }

            // Gerar o hash da senha com segurança
            novoUsuario.SetSenha(usuarioCadastro.Senha);

            // Salvar no banco de dados
            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            // Retornar DTO com sucesso
            return new ResultadoCadastroUsuario
            {
                Sucesso = true,
                Mensagem = "Usuário cadastrado com sucesso!",
                Usuario = new UsuarioDTO
                {
                    Id = novoUsuario.Id,
                    Nome = novoUsuario.Nome,
                    Email = novoUsuario.Email,
                    Tipo = novoUsuario.Tipo
                }
            };
        }
    }
}
