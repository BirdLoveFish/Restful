using System.Threading.Tasks;

namespace Restful.Core
{
    public interface IUnitOfWork
    {
        Task<bool> SaveAsync();
    }
}