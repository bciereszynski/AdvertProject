namespace AdvertProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoryFix : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Categories", name: "Category_ID", newName: "RootCategoryID");
            RenameIndex(table: "dbo.Categories", name: "IX_Category_ID", newName: "IX_RootCategoryID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Categories", name: "IX_RootCategoryID", newName: "IX_Category_ID");
            RenameColumn(table: "dbo.Categories", name: "RootCategoryID", newName: "Category_ID");
        }
    }
}
