namespace AdvertProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class htmltafs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HtmlTags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Tag = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HtmlTags");
        }
    }
}
