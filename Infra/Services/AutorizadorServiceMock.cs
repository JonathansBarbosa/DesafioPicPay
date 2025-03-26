using PicPaySimplificado.Application.Interfaces;
using System.Threading.Tasks;

namespace PicPaySimplificado.Infrastructure.Services
{
    public class AutorizadorServiceMock : IAutorizadorService
    {
        public async Task<bool> Autorizar()
        {
            // Simulação da autorização (sempre autoriza por enquanto)
            await Task.Delay(100); // Simula um tempo de resposta
            return true;
        }
    }
}