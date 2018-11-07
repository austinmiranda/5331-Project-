namespace SocialMarketplace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adjustments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Response", "Request_Id", "dbo.Request");
            DropIndex("dbo.Response", new[] { "Request_Id" });
            RenameColumn(table: "dbo.Request", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Notification", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Question", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Response", name: "UserId", newName: "User_Id");
            AlterColumn("dbo.Response", "Request_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Response", "Request_Id");
            AddForeignKey("dbo.Response", "Request_Id", "dbo.Request", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Response", "Request_Id", "dbo.Request");
            DropIndex("dbo.Response", new[] { "Request_Id" });
            AlterColumn("dbo.Response", "Request_Id", c => c.Int());
            RenameColumn(table: "dbo.Response", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Question", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Notification", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Request", name: "User_Id", newName: "UserId");
            CreateIndex("dbo.Response", "Request_Id");
            AddForeignKey("dbo.Response", "Request_Id", "dbo.Request", "Id");
        }
    }
}
