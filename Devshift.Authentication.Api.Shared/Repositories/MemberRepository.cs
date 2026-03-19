using System;
using System.Threading;
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
        public MemberRepository(string connectionString, bool sslMode, Certificate certs)
            : base(connectionString, sslMode, certs)
        {
        }

        public async Task<bool> ExistsUserNameAsync(string userName, CancellationToken ct)
        {
            using var sqlConnection = OpenDbConnection();

            const string sql = @"
                SELECT EXISTS (
                    SELECT 1
                    FROM devshift_auth.users
                    WHERE lower(user_name) = lower(@UserName)
                      AND is_deleted = false
                );
            ";

            return await sqlConnection.ExecuteScalarAsync<bool>(
                new CommandDefinition(sql, new { UserName = userName }, cancellationToken: ct)
            );
        }

        public async Task<bool> ExistsUserNameInSystemAsync(string userName, string systemCode, CancellationToken ct)
        {
            using var sqlConnection = OpenDbConnection();

            const string sql = @"
                SELECT EXISTS (
                    SELECT 1
                    FROM devshift_auth.users u
                    INNER JOIN devshift_auth.user_authens ua
                        ON ua.user_id = u.id
                    WHERE lower(u.user_name) = lower(@UserName)
                      AND ua.system_code = @SystemCode
                      AND u.is_deleted = false
                      AND ua.is_deleted = false
                );
            ";

            return await sqlConnection.ExecuteScalarAsync<bool>(
                new CommandDefinition(sql, new
                {
                    UserName = userName,
                    SystemCode = systemCode
                }, cancellationToken: ct)
            );
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequest request, CancellationToken ct)
        {
            using var sqlConnection = OpenDbConnection();
            sqlConnection.Open();
            using var tx = sqlConnection.BeginTransaction();

            try
            {
                const string validateSystemSql = @"
            SELECT EXISTS (
                SELECT 1
                FROM devshift_auth.systems
                WHERE code = @SystemCode
                  AND is_active = true
            );
        ";

                var systemExists = await sqlConnection.ExecuteScalarAsync<bool>(
                    new CommandDefinition(
                        validateSystemSql,
                        new { request.SystemCode },
                        transaction: tx,
                        cancellationToken: ct
                    )
                );

                if (!systemExists)
                    throw new Exception($"SystemCode '{request.SystemCode}' does not exist.");

                const string existsUserSql = @"
            SELECT EXISTS (
                SELECT 1
                FROM devshift_auth.users
                WHERE lower(user_name) = lower(@UserName)
                  AND is_deleted = false
            );
        ";

                var userExists = await sqlConnection.ExecuteScalarAsync<bool>(
                    new CommandDefinition(
                        existsUserSql,
                        new { request.UserName },
                        transaction: tx,
                        cancellationToken: ct
                    )
                );

                if (userExists)
                    throw new Exception($"UserName '{request.UserName}' already exists.");

                var userId = Guid.NewGuid();
                var userAuthenId = Guid.NewGuid();
                var now = DateTime.UtcNow;

                const string insertUserSql = @"
            INSERT INTO devshift_auth.users
            (
                id, user_name, email, display_name, phone_number,
                is_active, is_deleted, created_on, created_by
            )
            VALUES
            (
                @Id, @UserName, @Email, @DisplayName, @PhoneNumber,
                true, false, @CreatedOn, @CreatedBy
            );
        ";

                await sqlConnection.ExecuteAsync(
                    new CommandDefinition(
                        insertUserSql,
                        new
                        {
                            Id = userId,
                            request.UserName,
                            request.Email,
                            request.DisplayName,
                            request.PhoneNumber,
                            CreatedOn = now,
                            request.CreatedBy
                        },
                        transaction: tx,
                        cancellationToken: ct
                    )
                );

                const string insertUserAuthenSql = @"
            INSERT INTO devshift_auth.user_authens
            (
                id, user_id, system_code, auth_type,
                password_hash, password_salt, external_user_id,
                is_primary, actived, is_deleted, created_on, created_by
            )
            VALUES
            (
                @Id, @UserId, @SystemCode, @AuthType,
                @PasswordHash, @PasswordSalt, @ExternalUserId,
                true, true, false, @CreatedOn, @CreatedBy
            );
        ";

                await sqlConnection.ExecuteAsync(
                    new CommandDefinition(
                        insertUserAuthenSql,
                        new
                        {
                            Id = userAuthenId,
                            UserId = userId,
                            request.SystemCode,
                            request.AuthType,
                            request.PasswordHash,
                            request.PasswordSalt,
                            request.ExternalUserId,
                            CreatedOn = now,
                            request.CreatedBy
                        },
                        transaction: tx,
                        cancellationToken: ct
                    )
                );

                tx.Commit();

                return new RegisterResult
                {
                    UserId = userId,
                    UserAuthenId = userAuthenId,
                    UserName = request.UserName,
                    SystemCode = request.SystemCode,
                    AuthType = request.AuthType
                };
            }
            catch
            {
                tx.Rollback();
                throw;
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
        public async Task<UserTokenProfile> GetUserById(Guid userId)
        {
            using (var sqlConnection = OpenDbConnection())
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<UserTokenProfile>
                (QueryString.GetUserLogin, new { UserId = userId });
            }
        }
        public async Task<Guid> GetUserIdByUserName(string userName)
        {
            using (var sqlConnection = OpenDbConnection())
            {
                return await sqlConnection.QueryFirstOrDefaultAsync<Guid>
                (QueryString.GetUserIdByUserName, new { UserName = userName });
            }
        }
    }
}