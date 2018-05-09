namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _080518 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Carts", "Product_Id", "dbo.MenuItems");
            DropIndex("dbo.Carts", new[] { "Product_Id" });
            RenameColumn(table: "dbo.Carts", name: "Product_Id", newName: "MenuItemId");
            AlterColumn("dbo.Carts", "MenuItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.Carts", "MenuItemId");
            AddForeignKey("dbo.Carts", "MenuItemId", "dbo.MenuItems", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "MenuItemId", "dbo.MenuItems");
            DropIndex("dbo.Carts", new[] { "MenuItemId" });
            AlterColumn("dbo.Carts", "MenuItemId", c => c.Int());
            RenameColumn(table: "dbo.Carts", name: "MenuItemId", newName: "Product_Id");
            CreateIndex("dbo.Carts", "Product_Id");
            AddForeignKey("dbo.Carts", "Product_Id", "dbo.MenuItems", "Id");
        }
    }
}
