namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0905182 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FreeTimes", "DayWorkID", c => c.Int(nullable: false));
            DropColumn("dbo.FreeTimes", "DinningRoomId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FreeTimes", "DinningRoomId", c => c.Int(nullable: false));
            DropColumn("dbo.FreeTimes", "DayWorkID");
        }
    }
}
