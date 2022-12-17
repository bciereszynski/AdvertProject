namespace AdvertProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class advertuser : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Adverts", name: "ApplicationUser_Id", newName: "UserID");
            RenameIndex(table: "dbo.Adverts", name: "IX_ApplicationUser_Id", newName: "IX_UserID");
            DropColumn("dbo.Adverts", "Autor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Adverts", "Autor", c => c.String(nullable: false));
            RenameIndex(table: "dbo.Adverts", name: "IX_UserID", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Adverts", name: "UserID", newName: "ApplicationUser_Id");
        }
    }
}
