namespace Devshift.Authentication.Api.Shared.Utils
{
    public static class Constants
    {
        public static class Message
        {

            public static readonly string LoginSuccess = "LOGIN_SUCCESS";
            public static readonly string LoginFailed = "LOGIN_FAILED";
            public static readonly string UserOrPasswordIncorrect = "USER_OR_PASSWORD_INCORRECT";
            public static readonly string UserDisable = "USER_DISABLE";
            public static readonly string RefreshTokenExpired = "Refresh token expired please sign out and sing in again.";
            public static readonly string UserOrPasswordIsIncorrect = "User or Password is incorrect.";
            public static readonly string RegisterSuccess = "REGISTER_SUCCESS";
            public static readonly string UserAlreadyExists = "USER_ALREADY_EXISTS";
            public static readonly string RegisterFailed = "REGISTER_FAILED";
        }
    }
}