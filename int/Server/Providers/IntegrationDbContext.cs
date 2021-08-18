using Integration.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Integration.Server.Providers
{
    public class IntegrationDbContext : IdentityDbContext<IntegrationUser, IdentityRole<Guid>, Guid>
    {
        private readonly AdminOptions adminOptions = new();

        public IntegrationDbContext(DbContextOptions<IntegrationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            configuration.GetSection(nameof(AdminOptions)).Bind(adminOptions);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid>
                {
                    Id = adminOptions.AdminRoleId,
                    Name = adminOptions.AdminRoleName,
                    NormalizedName = adminOptions.AdminRoleNormalizedName
                });

            builder.Entity<IntegrationUser>().HasData(
                new IntegrationUser
                {
                    Id = adminOptions.Id,
                    FirstName = adminOptions.FirstName,
                    LastName = adminOptions.LastName,
                    UserName = adminOptions.UserName,
                    NormalizedUserName = adminOptions.NormalizedUserName,
                    Email = adminOptions.Email,
                    PasswordHash = new PasswordHasher<IntegrationUser>()
                        .HashPassword(null, adminOptions.Password)
                }
            ); ;

            builder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = adminOptions.AdminRoleId,
                    UserId = adminOptions.Id
                }
            );
        }
    }
}
