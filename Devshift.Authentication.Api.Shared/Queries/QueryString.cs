namespace Devshift.Authentication.Api.Shared.Queries
{
    public class QueryString
    {
        public static readonly string GetPassword = @"select
                                                        a.password
                                                    from user_authens a
                                                    where a.user_name = @UserName";
    }
}