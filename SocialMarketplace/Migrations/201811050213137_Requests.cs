namespace SocialMarketplace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Requests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Subtitle = c.String(),
                        Description = c.String(),
                        Keywords = c.String(),
                        Photo = c.String(),
                        VideoURL = c.String(),
                        VisualizationCount = c.Int(nullable: false),
                        Progress = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateDue = c.DateTime(nullable: false),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Request", "Category_Id", "dbo.Category");
            DropIndex("dbo.Request", new[] { "Category_Id" });
            DropTable("dbo.Request");
        }
    }
}
