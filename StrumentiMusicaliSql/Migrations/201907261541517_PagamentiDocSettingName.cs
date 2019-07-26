namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PagamentiDocSettingName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SettingSito", newName: "SettingSitoes");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.SettingSitoes", newName: "SettingSito");
        }
    }
}
