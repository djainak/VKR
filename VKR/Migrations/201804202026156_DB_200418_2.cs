namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DB_200418_2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryMenuItems",
                c => new
                    {
                        CategoryMenuItemID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryMenuItemID);
            
            AddColumn("dbo.MenuItems", "Category_CategoryMenuItemID", c => c.Int());
            CreateIndex("dbo.MenuItems", "Category_CategoryMenuItemID");
            AddForeignKey("dbo.MenuItems", "Category_CategoryMenuItemID", "dbo.CategoryMenuItems", "CategoryMenuItemID");
            DropColumn("dbo.MenuItems", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MenuItems", "Category", c => c.String());
            DropForeignKey("dbo.MenuItems", "Category_CategoryMenuItemID", "dbo.CategoryMenuItems");
            DropIndex("dbo.MenuItems", new[] { "Category_CategoryMenuItemID" });
            DropColumn("dbo.MenuItems", "Category_CategoryMenuItemID");
            DropTable("dbo.CategoryMenuItems");
        }
    }
}
