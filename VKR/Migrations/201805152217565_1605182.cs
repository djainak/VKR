namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1605182 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DinningRooms", "PasswordEmail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DinningRooms", "PasswordEmail", c => c.String());
        }
    }
}
