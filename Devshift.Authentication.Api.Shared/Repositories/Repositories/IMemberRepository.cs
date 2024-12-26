using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;

namespace Devshift.Authentication.Api.Shared.Repositories
{
    public interface IMemberRepository
    {
        Task<string> GetPassword(string userName);
        Task<RegisterActivate> CreateUser(Register dto);
        Task<User> FindUser(string userName);
    }
}