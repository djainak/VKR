namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _230518 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DinningRooms", "Min_time", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DinningRooms", "Min_time");
        }
    }
}
