using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_FirstApplication.Models.DbModel.WebSite;

namespace Web_FirstApplication.Repository.Implementation.WebSite
{
    public class WebSiteDbBase : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                Name = "Admin",
                NormalizedName = "ADMIN".ToUpper() 
            });

            var hasher = new PasswordHasher<IdentityUser>();

            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    PasswordHash = hasher.HashPassword(null, "123456789")
                }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
                });
        }
        public WebSiteDbBase(DbContextOptions<WebSiteDbBase> options)
            : base(options)
        {
        }
        public DbSet<DownloadLink> DownloadLinks { get; set; }
        public DbSet<DownloadRequiredment> DownloadRequiredments { get; set; }
        public DbSet<HomeNew> HomeNews { get; set; }
        public DbSet<Activity> Activites { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Donation.Coffe> Coffes { get; set; }
    }
}
