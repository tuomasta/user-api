namespace UserApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailmaxlenght : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 254));
            CreateIndex("dbo.Users", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
            CreateIndex("dbo.Users", "Email", unique: true);
        }
    }
}
