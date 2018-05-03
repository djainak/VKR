namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DB_0305182 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DayWorks", "StartDayHour", c => c.String());
            AddColumn("dbo.DayWorks", "StartDayMin", c => c.String());
            AddColumn("dbo.DayWorks", "EndDayHour", c => c.String());
            AddColumn("dbo.DayWorks", "EndDayMin", c => c.String());
            DropColumn("dbo.DayWorks", "StartDay");
            DropColumn("dbo.DayWorks", "EndDay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DayWorks", "EndDay", c => c.String());
            AddColumn("dbo.DayWorks", "StartDay", c => c.String());
            DropColumn("dbo.DayWorks", "EndDayMin");
            DropColumn("dbo.DayWorks", "EndDayHour");
            DropColumn("dbo.DayWorks", "StartDayMin");
            DropColumn("dbo.DayWorks", "StartDayHour");
        }
    }
}
