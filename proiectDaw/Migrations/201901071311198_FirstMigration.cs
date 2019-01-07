namespace proiectDaw.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "ImagePath", c => c.String());
            DropColumn("dbo.Pictures", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pictures", "Image", c => c.Binary(nullable: false));
            DropColumn("dbo.Pictures", "ImagePath");
        }
    }
}
