namespace NIceWeh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ActivityId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        Hours = c.Double(nullable: false),
                        Notes = c.String(),
                        ReportNotes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.ActivityId)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Reports", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Reports", new[] { "CustomerId" });
            DropIndex("dbo.Reports", new[] { "ActivityId" });
            DropTable("dbo.Reports");
            DropTable("dbo.Customers");
            DropTable("dbo.Activities");
        }
    }
}
