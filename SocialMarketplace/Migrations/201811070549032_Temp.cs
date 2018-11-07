namespace SocialMarketplace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Temp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Request", "Photo", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Request", "Photo", c => c.String(maxLength: 300));
        }
    }
}
