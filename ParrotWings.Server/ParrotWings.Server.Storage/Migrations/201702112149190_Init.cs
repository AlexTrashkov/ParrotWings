namespace ParrotWings.Server.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transfers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UserFromId = c.Guid(nullable: false),
                        UserToId = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 20, scale: 10),
                        UserFromBalanceBeforeTransfer = c.Decimal(nullable: false, precision: 20, scale: 10),
                        UserToBalanceBeforeTransfer = c.Decimal(nullable: false, precision: 20, scale: 10),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserFromId)
                .ForeignKey("dbo.Users", t => t.UserToId)
                .Index(t => t.UserFromId)
                .Index(t => t.UserToId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UserName = c.String(),
                        Email = c.String(),
                        HashedPass = c.String(),
                        Salt = c.String(),
                        CurrentBalance = c.Decimal(nullable: false, precision: 20, scale: 10),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transfers", "UserToId", "dbo.Users");
            DropForeignKey("dbo.Transfers", "UserFromId", "dbo.Users");
            DropIndex("dbo.Transfers", new[] { "UserToId" });
            DropIndex("dbo.Transfers", new[] { "UserFromId" });
            DropTable("dbo.Users");
            DropTable("dbo.Transfers");
        }
    }
}
