namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _070518 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartID = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.CartID)
                .ForeignKey("dbo.MenuItems", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
            AddColumn("dbo.Users", "Picture", c => c.String());
            AddColumn("dbo.Menus", "Picture", c => c.String());
            AddColumn("dbo.MenuItems", "Picture", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "Product_Id", "dbo.MenuItems");
            DropIndex("dbo.Carts", new[] { "Product_Id" });
            DropColumn("dbo.MenuItems", "Picture");
            DropColumn("dbo.Menus", "Picture");
            DropColumn("dbo.Users", "Picture");
            DropTable("dbo.Carts");
        }
    }
}
