namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _150518 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        OrderItemsId = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        Sum = c.Double(nullable: false),
                        MenuItemId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderItemsId)
                .ForeignKey("dbo.MenuItems", t => t.MenuItemId, cascadeDelete: true)
                .Index(t => t.MenuItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "MenuItemId", "dbo.MenuItems");
            DropIndex("dbo.OrderItems", new[] { "MenuItemId" });
            DropTable("dbo.OrderItems");
        }
    }
}
