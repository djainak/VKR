namespace VKR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _0705182 : DbMigration
    {
        public override void Up()
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
            
            AddColumn("dbo.MenuItems", "Picture_Id", c => c.Int());
            AddColumn("dbo.Menus", "Picture_Id", c => c.Int());
            AddColumn("dbo.Users", "Picture_Id", c => c.Int());
            CreateIndex("dbo.MenuItems", "Picture_Id");
            CreateIndex("dbo.Menus", "Picture_Id");
            CreateIndex("dbo.Users", "Picture_Id");
            AddForeignKey("dbo.Menus", "Picture_Id", "dbo.Pictures", "Id");
            AddForeignKey("dbo.MenuItems", "Picture_Id", "dbo.Pictures", "Id");
            AddForeignKey("dbo.Users", "Picture_Id", "dbo.Pictures", "Id");
            DropColumn("dbo.MenuItems", "Picture");
            DropColumn("dbo.Menus", "Picture");
            DropColumn("dbo.Users", "Picture");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Picture", c => c.String());
            AddColumn("dbo.Menus", "Picture", c => c.String());
            AddColumn("dbo.MenuItems", "Picture", c => c.String());
            DropForeignKey("dbo.Users", "Picture_Id", "dbo.Pictures");
            DropForeignKey("dbo.MenuItems", "Picture_Id", "dbo.Pictures");
            DropForeignKey("dbo.Menus", "Picture_Id", "dbo.Pictures");
            DropIndex("dbo.Users", new[] { "Picture_Id" });
            DropIndex("dbo.Menus", new[] { "Picture_Id" });
            DropIndex("dbo.MenuItems", new[] { "Picture_Id" });
            DropColumn("dbo.Users", "Picture_Id");
            DropColumn("dbo.Menus", "Picture_Id");
            DropColumn("dbo.MenuItems", "Picture_Id");
            DropTable("dbo.Pictures");
        }
    }
}
