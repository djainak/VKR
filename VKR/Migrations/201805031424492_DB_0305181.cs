namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DB_0305181 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DayWorks", "StartDay", c => c.String());
            AlterColumn("dbo.DayWorks", "EndDay", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DayWorks", "EndDay", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DayWorks", "StartDay", c => c.DateTime(nullable: false));
        }
    }
}
