namespace proiectDaw.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileAdded : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Pictures", new[] { "User_Id" });
            DropColumn("dbo.Pictures", "UserId");
            RenameColumn(table: "dbo.Albums", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Pictures", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Albums", name: "IX_User_Id", newName: "IX_UserId");
            AlterColumn("dbo.Pictures", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comments", "CreatedBy", c => c.String());
            CreateIndex("dbo.Pictures", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Pictures", new[] { "UserId" });
            AlterColumn("dbo.Comments", "CreatedBy", c => c.Int(nullable: false));
            AlterColumn("dbo.Pictures", "UserId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Albums", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Pictures", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Albums", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Pictures", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Pictures", "User_Id");
        }
    }
}
