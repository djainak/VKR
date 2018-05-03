namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DB_0305183 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DayWorks", "DinningRoomID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DayWorks", "DinningRoomID");
        }
    }
}
