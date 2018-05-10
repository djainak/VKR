namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _100518 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FreeTimes", "DayWorkID", c => c.Int(nullable: false));
            CreateIndex("dbo.FreeTimes", "DayWorkID");
            AddForeignKey("dbo.FreeTimes", "DayWorkID", "dbo.DayWorks", "DayWorkID", cascadeDelete: true);
            DropColumn("dbo.FreeTimes", "DinningRoomId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FreeTimes", "DinningRoomId", c => c.Int(nullable: false));
            DropForeignKey("dbo.FreeTimes", "DayWorkID", "dbo.DayWorks");
            DropIndex("dbo.FreeTimes", new[] { "DayWorkID" });
            DropColumn("dbo.FreeTimes", "DayWorkID");
        }
    }
}
