using System;

namespace Devshift.Authentication.Api.Shared.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Actived { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SystemId { get; set; }
        public string SystemName { get; set; }
        public string SystemUrl { get; set; }
        public string RoleId { get; set; }
    }
    public class UserProfile
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Actived { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SystemId { get; set; }
        public string SystemName { get; set; }
        public string SystemUrl { get; set; }
        public string RoleId { get; set; }
    }
    public class UserResponse
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SystemId { get; set; }
        public string SystemName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class UserDataResponse
    {
        public int UserId { get; set; }
        public string UserUUId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public sealed class UserTokenProfile
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string SystemCode { get; set; } = string.Empty;
        public string[] Roles { get; set; } = new string[0];
        public string[] Permissions { get; set; } = new string[0];
    }

    public class Register
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class RegisterActivate
    {
        public string Id { get; set; }
        public string UserName { get; set; }

    }
}