namespace AdvertProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class category : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "Advert_ID", "dbo.Adverts");
            DropIndex("dbo.Tags", new[] { "Advert_ID" });
            CreateTable(
                "dbo.AdvertCategories",
                c => new
                    {
                        AdvertCategoryID = c.Int(nullable: false, identity: true),
                        AdvertID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdvertCategoryID)
                .ForeignKey("dbo.Adverts", t => t.AdvertID, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.AdvertID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.Tags");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Advert_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.AdvertCategories", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.AdvertCategories", "AdvertID", "dbo.Adverts");
            DropIndex("dbo.AdvertCategories", new[] { "CategoryID" });
            DropIndex("dbo.AdvertCategories", new[] { "AdvertID" });
            DropTable("dbo.Categories");
            DropTable("dbo.AdvertCategories");
            CreateIndex("dbo.Tags", "Advert_ID");
            AddForeignKey("dbo.Tags", "Advert_ID", "dbo.Adverts", "ID");
        }
    }
}
