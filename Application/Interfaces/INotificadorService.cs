using System.Threading.Tasks;

namespace PicPaySimplificado.Application.Interfaces
{
    public interface INotificadorService
    {
        Task Notificar(string email);
    }
}