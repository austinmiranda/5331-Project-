namespace SocialMarketplace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestItem_InitialQuantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RequestItem", "InitialQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RequestItem", "InitialQuantity");
        }
    }
}
