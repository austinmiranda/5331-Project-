namespace SocialMarketplace.Models.DAL
{
    using SocialMarketplace.Models.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class ApplicationContext : DbContext
    {
        // Your context has been configured to use a 'ApplicationContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'SocialMarketplace.Models.DAL.ApplicationContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ApplicationContext' 
        // connection string in the application configuration file.
        public ApplicationContext()
            : base("name=ApplicationContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestItem> RequestItems { get; set; }
        public virtual DbSet<Response> Responses { get; set; }
        public virtual DbSet<ResponseItem> ResponseItems { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
    }
}