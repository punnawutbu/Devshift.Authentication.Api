using System.Threading.Tasks;
using Dapper;
using Devshift.Authentication.Api.Shared.Models;
using Devshift.Authentication.Api.Shared.Queries;
using Devshift.Dapper;
using Devshift.Dapper.Models;

namespace Devshift.Authentication.Api.Shared.Repositories
{
    public class MemberRepository : BaseRepository, IMemberRepository
    {
        public MemberRepository(string connectionString, bool sslMode, Certificate certs) : base(connectionString, sslMode, certs) { }
        public async Task<RegisterActivate> CreateUser(Register req)
        {
            using (var sqlConnection = OpenDbConnection())
            {
                var sql = @"
                    insert into user_authens (user_name,password,auth_type,user_id,actived) values (@UserName,@Password,'user',1,TRUE)
                ";
                await sqlConnection.ExecuteAsync(sql, req);
                var sqlGet = @"
                    select  uuid ::text as id from user_authens where user_name = @UserName
                ";
                return await sqlConnection.QueryFirstOrDefaultAsync<RegisterActivate>(sqlGet, new { userName = req.UserName });
            }
        }
        public async Task<User> FindUser(string userName)
        {
            using (var sqlConnection = OpenDbConnection())
            {
                var sql = @"
                    select  uuid ::text as id
                    from user_authens
                    where user_name = @userName
                ";

                return await sqlConnection.QueryFirstOrDefaultAsync<User>(sql, new { userName });
            }
        }
        public async Task<string> GetPassword(string userName)
        {
            using (var sqlConnection = OpenDbConnection())
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<string>
                (QueryString.GetPassword, new { UserName = userName });
            }
        }
    }
}