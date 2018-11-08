namespace SocialMarketplace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionAdjustment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Question", "Request_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "Description", c => c.String(nullable: false, maxLength: 4000));
            AlterColumn("dbo.Question", "Description", c => c.String(nullable: false, maxLength: 4000));
            CreateIndex("dbo.Question", "Request_Id");
            AddForeignKey("dbo.Question", "Request_Id", "dbo.Request", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Question", "Request_Id", "dbo.Request");
            DropIndex("dbo.Question", new[] { "Request_Id" });
            AlterColumn("dbo.Question", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Request", "Description", c => c.String(nullable: false));
            DropColumn("dbo.Question", "Request_Id");
        }
    }
}
