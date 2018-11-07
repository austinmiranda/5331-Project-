namespace SocialMarketplace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OtherTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Request", "Category_Id", "dbo.Category");
            DropIndex("dbo.Request", new[] { "Category_Id" });
            CreateTable(
                "dbo.Area",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedback",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Photo = c.String(maxLength: 300),
                        VideoURL = c.String(maxLength: 300),
                        Request_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Request", t => t.Request_Id, cascadeDelete: true)
                .Index(t => t.Request_Id);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DateRequested = c.DateTime(nullable: false),
                        InterestedInCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.InterestedInCategory_Id, cascadeDelete: true)
                .Index(t => t.InterestedInCategory_Id);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RequestItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Detail = c.String(nullable: false, maxLength: 300),
                        Type = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Request_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Request", t => t.Request_Id, cascadeDelete: true)
                .Index(t => t.Request_Id);
            
            CreateTable(
                "dbo.ResponseItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        RequestItem_Id = c.Int(),
                        Response_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RequestItem", t => t.RequestItem_Id)
                .ForeignKey("dbo.Response", t => t.Response_Id)
                .Index(t => t.RequestItem_Id)
                .Index(t => t.Response_Id);
            
            CreateTable(
                "dbo.Response",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        Request_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Request", t => t.Request_Id)
                .Index(t => t.Request_Id);
            
            AddColumn("dbo.Request", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Request", "Area_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Category", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Request", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Request", "Subtitle", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Request", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Request", "Keywords", c => c.String(maxLength: 300));
            AddColumn("dbo.Request", "Photo", c => c.Binary());
            AlterColumn("dbo.Request", "VideoURL", c => c.String(maxLength: 300));
            AlterColumn("dbo.Request", "Category_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Request", "Area_Id");
            CreateIndex("dbo.Request", "Category_Id");
            AddForeignKey("dbo.Request", "Area_Id", "dbo.Area", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Request", "Category_Id", "dbo.Category", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Request", "Category_Id", "dbo.Category");
            DropForeignKey("dbo.ResponseItem", "Response_Id", "dbo.Response");
            DropForeignKey("dbo.Response", "Request_Id", "dbo.Request");
            DropForeignKey("dbo.ResponseItem", "RequestItem_Id", "dbo.RequestItem");
            DropForeignKey("dbo.RequestItem", "Request_Id", "dbo.Request");
            DropForeignKey("dbo.Notification", "InterestedInCategory_Id", "dbo.Category");
            DropForeignKey("dbo.Feedback", "Request_Id", "dbo.Request");
            DropForeignKey("dbo.Request", "Area_Id", "dbo.Area");
            DropIndex("dbo.Response", new[] { "Request_Id" });
            DropIndex("dbo.ResponseItem", new[] { "Response_Id" });
            DropIndex("dbo.ResponseItem", new[] { "RequestItem_Id" });
            DropIndex("dbo.RequestItem", new[] { "Request_Id" });
            DropIndex("dbo.Notification", new[] { "InterestedInCategory_Id" });
            DropIndex("dbo.Request", new[] { "Category_Id" });
            DropIndex("dbo.Request", new[] { "Area_Id" });
            DropIndex("dbo.Feedback", new[] { "Request_Id" });
            AlterColumn("dbo.Request", "Category_Id", c => c.Int());
            AlterColumn("dbo.Request", "VideoURL", c => c.String());
            DropColumn("dbo.Request", "Photo");
            AlterColumn("dbo.Request", "Keywords", c => c.String());
            AlterColumn("dbo.Request", "Description", c => c.String());
            AlterColumn("dbo.Request", "Subtitle", c => c.String());
            AlterColumn("dbo.Request", "Title", c => c.String());
            AlterColumn("dbo.Category", "Name", c => c.String());
            DropColumn("dbo.Request", "Area_Id");
            DropColumn("dbo.Request", "UserId");
            DropTable("dbo.Response");
            DropTable("dbo.ResponseItem");
            DropTable("dbo.RequestItem");
            DropTable("dbo.Question");
            DropTable("dbo.Notification");
            DropTable("dbo.Feedback");
            DropTable("dbo.Area");
            CreateIndex("dbo.Request", "Category_Id");
            AddForeignKey("dbo.Request", "Category_Id", "dbo.Category", "Id");
        }
    }
}
