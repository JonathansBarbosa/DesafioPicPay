using System;
using System.Threading.Tasks;

namespace PicPaySimplificado.Application.Interfaces
{
    public interface ITransacaoService
    {
        Task<string> RealizarTransferencia(Guid pagadorId, Guid recebedorId, decimal valor);
    }
}