namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DinningRooms",
                c => new
                    {
                        DinningRoomID = c.Int(nullable: false, identity: true),
                        Adress = c.String(),
                        PhoneNum = c.String(),
                        Email = c.String(),
                        Dishes = c.Int(nullable: false),
                        Interval = c.Int(nullable: false),
                        Manager_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.DinningRoomID)
                .ForeignKey("dbo.Users", t => t.Manager_UserID)
                .Index(t => t.Manager_UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DinningRooms", "Manager_UserID", "dbo.Users");
            DropIndex("dbo.DinningRooms", new[] { "Manager_UserID" });
            DropTable("dbo.DinningRooms");
        }
    }
}
