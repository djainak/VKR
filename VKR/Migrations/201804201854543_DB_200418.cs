namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DB_200418 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menus", "Status");
        }
    }
}
