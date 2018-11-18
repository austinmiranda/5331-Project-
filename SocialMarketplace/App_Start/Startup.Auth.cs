using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using SocialMarketplace.Models;


namespace SocialMarketplace
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);






            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User, int>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                         getUserIdCallback: (id) => (id.GetUserId<int>())
                        )
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

           // Uncomment the following lines to enable logging in with third party login providers

            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "f3819c63-4a37-498f-8954-d8b4aa9a9917",
            //    clientSecret: "lcIJCOHY45)%}fmmblR653_");

          

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "300801301561-is7d2pt2p7v986u364drekafvt1u83k8.apps.googleusercontent.com",
                ClientSecret = "FjdLveE-1BQ2cT0Prb8lZd2R"
            });

            createRole();


        }



        //private void createRolesandUsers()
        //{
        //    var userManager = HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();

        //    var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

        //    const string name = "admin@admin.com";
        //    const string password = "Admin@123456";
        //    const string roleName = "Admin";
        //    const string fName = "Admin";
        //    const string lName = "Admin";

        //    //Create Role Admin if it does not exist
        //    var role = roleManager.FindByName(roleName);

        //    if (role == null)
        //    {
        //        role = new IdentityRole(roleName);
        //        var roleresult = roleManager.Create(role);
        //    }

        //    var user = userManager.FindByName(name);

        //    if (user == null)
        //    {
        //        user = new User { UserName = name, Email = name, FirstName = fName, LastName = lName };
        //        var result = userManager.Create(user, password);
        //        result = userManager.SetLockoutEnabled(user.Id, false);
        //    }

        //    // Add user admin to Role Admin if not already added
        //    var rolesForUser = userManager.GetRoles(user.Id);

        //    if (!rolesForUser.Contains(role.Name))
        //    {
        //        var result = userManager.AddToRole(user.Id, role.Name);
        //    }

        //}

        private void createRole()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<CustomRole, int>(new RoleStore<CustomRole, int, CustomUserRole>(context));
            var UserManager = new UserManager<User, int>(new UserStore<User, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>(context));

            const string name = "admin@admin.com";
            const string password = "Admin@123456";
            const string roleName = "Admin";
            const string fName = "Admin";
            const string lName = "Admin";

            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists(roleName))
            {
                //first we create Admin rool

                var role = new CustomRole();
                role.Name = roleName;
                roleManager.Create(role);
            }

            ////Here we create a Admin super user who will maintain the website     
            if (UserManager.FindByName("Admin") == null)
            {

                var user = new User { UserName = name, Email = name, FirstName = fName, LastName = lName };
                var chkUser = UserManager.Create(user, password);
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, roleName);
                }
            }
        }

    }
}