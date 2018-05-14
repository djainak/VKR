namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1405182 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Sum", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Sum");
        }
    }
}
