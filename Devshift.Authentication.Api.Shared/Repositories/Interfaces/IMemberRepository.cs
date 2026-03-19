using System;
using System.Threading;
using System.Threading.Tasks;
using Devshift.Authentication.Api.Shared.Models;

namespace Devshift.Authentication.Api.Shared.Repositories;

public interface IMemberRepository
{
    Task<RegisterResult> RegisterAsync(RegisterRequest request, CancellationToken ct);
    Task<bool> ExistsUserNameAsync(string userName, CancellationToken ct);
    Task<bool> ExistsUserNameInSystemAsync(string userName, string systemCode, CancellationToken ct);
    Task<string> GetPassword(string userName);
    Task<UserTokenProfile> GetUserById(Guid userId);
    Task<Guid> GetUserIdByUserName(string userName);
}