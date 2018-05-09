namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _090518 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FreeTimes",
                c => new
                    {
                        FreeTimeID = c.Int(nullable: false, identity: true),
                        Time = c.String(),
                        max_amount = c.Int(nullable: false),
                        cur_amount = c.Int(nullable: false),
                        DinningRoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FreeTimeID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FreeTimes");
        }
    }
}
