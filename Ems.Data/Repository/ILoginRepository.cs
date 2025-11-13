using Ems.Data.Models.Domain;
using System.Threading.Tasks;

namespace Ems.Data.Repositories
{
    public interface ILoginRepository
    {
        Task<bool> ValidateUserAsync(string email, string password);
    }
}

