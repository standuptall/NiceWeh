namespace NIceWeh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fir : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Cognome = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Reports", "EmployeeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Reports", "EmployeeId");
            AddForeignKey("dbo.Reports", "EmployeeId", "dbo.Employees", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reports", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Reports", new[] { "EmployeeId" });
            DropColumn("dbo.Reports", "EmployeeId");
            DropTable("dbo.Employees");
        }
    }
}
