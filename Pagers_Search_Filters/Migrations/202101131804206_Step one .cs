namespace Pagers_Search_Filters.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stepone : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fighters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Ability = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Fighters");
        }
    }
}
