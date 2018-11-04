using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SocialMarketplace.Models
{
    public class CustomUserLogin : IdentityUserLogin<int> { }
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomRole : IdentityRole<int, CustomUserRole> {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }

        public static implicit operator CustomRole(IdentityRole v)
        {
            throw new NotImplementedException();
        }
    }

    public class CustomUserStore : UserStore<User, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context) : base(context)
        {

        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context) : base(context)
        {

        }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        public string Address { get; set; }

        public string Company { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<User, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("IdentityDbContext")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // This needs to go before the other rules!

            modelBuilder.Entity<User>().Property(p => p.Id);
            modelBuilder.Entity<IdentityRole>().Property(p => p.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(p => new { p.RoleId, p.UserId });
            modelBuilder.Entity<IdentityUserLogin>().HasKey(p => new { p.LoginProvider,p.ProviderKey, p.UserId });
            modelBuilder.Entity<IdentityUserClaim>().HasKey(p => p.Id);
        }

    }
}