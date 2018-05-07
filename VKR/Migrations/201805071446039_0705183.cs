namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0705183 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Menus", "Picture_Id", "dbo.Pictures");
            DropForeignKey("dbo.MenuItems", "Picture_Id", "dbo.Pictures");
            DropForeignKey("dbo.Users", "Picture_Id", "dbo.Pictures");
            DropIndex("dbo.MenuItems", new[] { "Picture_Id" });
            DropIndex("dbo.Menus", new[] { "Picture_Id" });
            DropIndex("dbo.Users", new[] { "Picture_Id" });
            AddColumn("dbo.MenuItems", "Picture", c => c.String());
            AddColumn("dbo.Menus", "Picture", c => c.String());
            AddColumn("dbo.Users", "Picture", c => c.String());
            DropColumn("dbo.MenuItems", "Picture_Id");
            DropColumn("dbo.Menus", "Picture_Id");
            DropColumn("dbo.Users", "Picture_Id");
            DropTable("dbo.Pictures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Picture_Id", c => c.Int());
            AddColumn("dbo.Menus", "Picture_Id", c => c.Int());
            AddColumn("dbo.MenuItems", "Picture_Id", c => c.Int());
            DropColumn("dbo.Users", "Picture");
            DropColumn("dbo.Menus", "Picture");
            DropColumn("dbo.MenuItems", "Picture");
            CreateIndex("dbo.Users", "Picture_Id");
            CreateIndex("dbo.Menus", "Picture_Id");
            CreateIndex("dbo.MenuItems", "Picture_Id");
            AddForeignKey("dbo.Users", "Picture_Id", "dbo.Pictures", "Id");
            AddForeignKey("dbo.MenuItems", "Picture_Id", "dbo.Pictures", "Id");
            AddForeignKey("dbo.Menus", "Picture_Id", "dbo.Pictures", "Id");
        }
    }
}
