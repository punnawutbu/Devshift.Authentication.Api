namespace Devshift.Authentication.Api.Shared.Queries
{
    public static class QueryString
    {
        public static readonly string GetPassword = @"select ua.password_hash
                                                        from devshift_auth.users u
                                                        left join devshift_auth.user_authens ua
                                                        on u.id = ua.user_id
                                                        where u.user_name = @UserName";

        public static readonly string GetUserLogin = @"
                                                        SELECT
                                                            u.id AS userId,
                                                            u.user_name AS userName,
                                                            ARRAY_AGG(DISTINCT r.name) AS roles,
                                                            ARRAY_AGG(DISTINCT p.code) AS permissions
                                                        FROM devshift_auth.users u
                                                        JOIN devshift_rbac.user_roles ur
                                                            ON ur.user_id = u.id
                                                        JOIN devshift_rbac.roles r
                                                            ON r.id = ur.role_id
                                                        JOIN devshift_rbac.role_permissions rp
                                                            ON rp.role_id = r.id
                                                        JOIN devshift_rbac.permissions p
                                                            ON p.code = rp.permission_code
                                                        WHERE u.id = @UserId
                                                        GROUP BY u.id, u.user_name;
                                                        ";

        public static readonly string GetUserIdByUserName = @"
                                                        SELECT
                                                            u.id AS userId
                                                        FROM devshift_auth.users u
                                                        WHERE u.user_name = @UserName;
                                                        ";
    }
}