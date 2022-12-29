namespace AdvertProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "Category_ID", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "Category_ID" });
            DropColumn("dbo.Categories", "Category_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "Category_ID", c => c.Int());
            CreateIndex("dbo.Categories", "Category_ID");
            AddForeignKey("dbo.Categories", "Category_ID", "dbo.Categories", "ID");
        }
    }
}
