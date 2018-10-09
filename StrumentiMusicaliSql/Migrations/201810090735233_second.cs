namespace StrumentiMusicaliSql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FotoArticoloes", newName: "FotoArticoli");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.FotoArticoli", newName: "FotoArticoloes");
        }
    }
}
