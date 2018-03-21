namespace UserApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    Email = c.String(nullable: false),
                    FirstName = c.String(nullable: false),
                    LastName = c.String(nullable: false),
                    Country = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
