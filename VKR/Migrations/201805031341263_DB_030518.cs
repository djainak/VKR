namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DB_030518 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DayWorks",
                c => new
                    {
                        DayWorkID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDay = c.DateTime(nullable: false),
                        EndDay = c.DateTime(nullable: false),
                        IsWorkDay = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DayWorkID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DayWorks");
        }
    }
}
