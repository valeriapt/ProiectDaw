namespace proiectDaw.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedByToStringMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Albums", "CreatedBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Albums", "CreatedBy", c => c.Int(nullable: false));
        }
    }
}
