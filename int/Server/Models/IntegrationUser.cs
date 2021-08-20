using Microsoft.AspNetCore.Identity;
using System;

namespace Integration.Server.Models
{
    public class IntegrationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
