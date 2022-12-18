namespace AdvertProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forbiden_words : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForbidenWords",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ForbidenWords");
        }
    }
}
