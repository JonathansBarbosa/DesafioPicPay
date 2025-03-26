using PicPaySimplificado.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace PicPaySimplificado.Infrastructure.Services
{
    public class NotificadorServiceMock : INotificadorService
    {
        public async Task Notificar(string email)
        {
            // Simulação do envio de notificação
            await Task.Delay(100); // Simula um tempo de envio
            Console.WriteLine($"Notificação (Mock) enviada para: {email}");
        }
    }
}