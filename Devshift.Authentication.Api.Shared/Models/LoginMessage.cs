namespace Devshift.Authentication.Api.Shared.Models
{
    public class LoginMessage
    {

        public static readonly string LoginSuccess = "LOGIN_SUCCESS";
        public static readonly string UserOrPasswordIncorrect = "USER_OR_PASSWORD_INCORRECT";
        public static readonly string UserDisable = "USER_DISABLE";
        public static readonly string RefreshTokenExpired = "Refresh token expired please sign out and sing in again.";
        public static readonly string UserOrPasswordIsIncorrect = "User or Password is incorrect.";
    }
}