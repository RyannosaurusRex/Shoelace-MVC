namespace ShoelaceMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Account : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subdomain = c.String(),
                        VanityDomain = c.String(),
                        Owner_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "Owner_Id", "dbo.Users");
            DropIndex("dbo.Accounts", new[] { "Owner_Id" });
            DropTable("dbo.Accounts");
        }
    }
}
