using System.Threading.Tasks;

namespace PicPaySimplificado.Application.Interfaces
{
    public interface IAutorizadorService
    {
        Task<bool> Autorizar();
    }
}