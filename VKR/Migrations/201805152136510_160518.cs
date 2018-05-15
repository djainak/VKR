namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _160518 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DinningRooms", "PasswordEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DinningRooms", "PasswordEmail");
        }
    }
}
