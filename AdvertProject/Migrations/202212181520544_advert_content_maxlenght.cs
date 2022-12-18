namespace AdvertProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class advert_content_maxlenght : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Adverts", "Content", c => c.String(nullable: false, maxLength: 1500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Adverts", "Content", c => c.String(nullable: false));
        }
    }
}
