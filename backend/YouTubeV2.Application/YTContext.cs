using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YouTubeV2.Application.EntityConfiguration;
using YouTubeV2.Application.Model;

namespace YouTubeV2.Application
{
    public class YTContext : IdentityDbContext<
        User,
        Role,
        string>
    {
        public DbSet<Subscription> Subscriptions { get; set; }

        public YTContext(DbContextOptions<YTContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityConfiguration).Assembly);

            var roles = new[]
            {
                new Role
                {
                    Id = "39cc2fe2-d00d-4f48-a49d-005d8e983c72",
                    Name = Role.Simple,
                    NormalizedName = Role.Simple.ToUpper(),
                },
                new Role
                {
                    Id = "63798117-72aa-4bc5-a1ef-4e771204d561",
                    Name = Role.Creator,
                    NormalizedName = Role.Creator.ToUpper(),
                },
                new Role
                {
                    Id = "b3a48a48-1a74-45da-a179-03b298bc53bc",
                    Name = Role.Administrator,
                    NormalizedName = Role.Administrator.ToUpper(),
                }
            };

            modelBuilder.Entity<Role>().HasData(roles);          

            var user = new User
            {
                Id = "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                Email = "test@mail.com",
                NormalizedEmail = "test@mail.com".ToUpper(),
                Name = "Prime",
                Surname = "Test",
                UserName = "Example",
                NormalizedUserName = "Example".ToUpper(),
            };

            PasswordHasher<User> ph = new PasswordHasher<User>();
            user.PasswordHash = ph.HashPassword(user, "123!@#asdASD");

            modelBuilder.Entity<User>().HasData(user);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roles[0].Id,
                UserId = user.Id
            });
        }
    }
}
