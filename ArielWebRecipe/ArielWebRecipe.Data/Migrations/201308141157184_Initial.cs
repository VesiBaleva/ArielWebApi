namespace ArielWebRecipe.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "User_Id", c => c.Int());
            AddForeignKey("dbo.Comments", "User_Id", "dbo.Users", "Id");
            CreateIndex("dbo.Comments", "User_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropForeignKey("dbo.Comments", "User_Id", "dbo.Users");
            DropColumn("dbo.Comments", "User_Id");
        }
    }
}
