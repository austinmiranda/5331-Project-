namespace SocialMarketplace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablephoto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                        Request_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Request", t => t.Request_Id)
                .Index(t => t.Request_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photo", "Request_Id", "dbo.Request");
            DropIndex("dbo.Photo", new[] { "Request_Id" });
            DropTable("dbo.Photo");
        }
    }
}
