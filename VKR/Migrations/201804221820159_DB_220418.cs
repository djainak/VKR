namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DB_220418 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MenuItems", "Category_CategoryMenuItemID", "dbo.CategoryMenuItems");
            DropIndex("dbo.MenuItems", new[] { "Category_CategoryMenuItemID" });
            RenameColumn(table: "dbo.MenuItems", name: "Category_CategoryMenuItemID", newName: "CategoryMenuItemId");
            AlterColumn("dbo.MenuItems", "CategoryMenuItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.MenuItems", "CategoryMenuItemId");
            AddForeignKey("dbo.MenuItems", "CategoryMenuItemId", "dbo.CategoryMenuItems", "CategoryMenuItemID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuItems", "CategoryMenuItemId", "dbo.CategoryMenuItems");
            DropIndex("dbo.MenuItems", new[] { "CategoryMenuItemId" });
            AlterColumn("dbo.MenuItems", "CategoryMenuItemId", c => c.Int());
            RenameColumn(table: "dbo.MenuItems", name: "CategoryMenuItemId", newName: "Category_CategoryMenuItemID");
            CreateIndex("dbo.MenuItems", "Category_CategoryMenuItemID");
            AddForeignKey("dbo.MenuItems", "Category_CategoryMenuItemID", "dbo.CategoryMenuItems", "CategoryMenuItemID");
        }
    }
}
