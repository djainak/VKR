namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Ingredients = c.String(),
                        Price = c.Int(nullable: false),
                        MenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Menus", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.MenuId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        WhereEat = c.Boolean(nullable: false),
                        OrderTime = c.DateTime(nullable: false),
                        ReadyTime = c.DateTime(nullable: false),
                        Notes = c.String(),
                        Status = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        FirstName = c.String(),
                        Name = c.String(),
                        Patronymic = c.String(),
                        Email = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuItems", "MenuId", "dbo.Menus");
            DropIndex("dbo.MenuItems", new[] { "MenuId" });
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.MenuItems");
            DropTable("dbo.Menus");
        }
    }
}
