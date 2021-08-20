using System;

namespace Integration.Server.Models
{
    public class AdminOptions
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public Guid AdminRoleId { get; set; }

        public string AdminRoleName { get; set; }

        public string AdminRoleNormalizedName { get; set; }
    }
}
