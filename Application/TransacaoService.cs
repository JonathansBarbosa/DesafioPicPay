using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicPaySimplificado.Domain.Entities;
using PicPaySimplificado.Infrastructure.Persistence; // Namespace correto para o DbContext
using PicPaySimplificado.Application.Interfaces; // Para a interface de repositório (explicarei adiante)

namespace PicPaySimplificado.Application.Services // Namespace correto para serviços de aplicação
{
    public class TransacaoService : ITransacaoService // Implementa uma interface
    {
        private readonly ApplicationDbContext _context;
        private readonly IAutorizadorService _autorizadorService; // Injeção do serviço de autorização
        private readonly INotificadorService _notificadorService; // Injeção do serviço de notificação

        public TransacaoService(ApplicationDbContext context, IAutorizadorService autorizadorService, INotificadorService notificadorService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _autorizadorService = autorizadorService ?? throw new ArgumentNullException(nameof(autorizadorService));
            _notificadorService = notificadorService ?? throw new ArgumentNullException(nameof(notificadorService));
        }

        public async Task<string> RealizarTransferencia(Guid pagadorId, Guid recebedorId, decimal valor)
        {
            if (valor <= 0)
            {
                return "O valor da transferência deve ser maior que zero."; // Validação no início
            }

            using var transaction = _context.Database.BeginTransaction(); // Usando transação

            try
            {
                var pagador = await _context.Usuarios.FindAsync(pagadorId);
                var recebedor = await _context.Usuarios.FindAsync(recebedorId);

                if (pagador == null || recebedor == null)
                    return "Usuário(s) não encontrado(s).";

                if (pagador.IsLojista)
                    return "Lojistas não podem realizar transferências.";

                if (pagador.Saldo < valor)
                    return "Saldo insuficiente.";

                // Autorização externa (usando o serviço injetado)
                var autorizacao = await _autorizadorService.Autorizar();
                if (!autorizacao)
                    return "Transferência não autorizada.";

                // Realizar a transferência (dentro da transação)
                pagador.Saldo -= valor;
                recebedor.Saldo += valor;

                var transacao = new Transacao
                {
                    Id = Guid.NewGuid(),
                    PagadorId = pagadorId,
                    RecebedorId = recebedorId,
                    Valor = valor,
                    Data = DateTime.Now
                };

                _context.Transacoes.Add(transacao);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();//Commitando a transação

                // Notificação (usando o serviço injetado - tratamento de falhas)
                try
                {
                    await _notificadorService.Notificar(recebedor.Email);
                }
                catch (Exception ex)
                {
                    // Logar o erro de notificação (NÃO REVERTER A TRANSFERÊNCIA)
                    Console.WriteLine($"Erro ao enviar notificação: {ex.Message}");
                    //Continuar o fluxo mesmo com erro na notificação, pois a transferencia já foi feita
                }

                return "Transferência realizada com sucesso.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback em caso de erro
                // Logar a exceção (importante para depuração)
                Console.WriteLine($"Erro na transferência: {ex.Message}");
                return "Erro ao realizar transferência."; // Mensagem genérica para o usuário
            }
        }
    }
}