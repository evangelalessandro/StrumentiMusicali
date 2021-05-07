namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RimozioneMercatico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SettingProgramma", "AmbientePagamentiRateali", c => c.Boolean(nullable: false));
            DropColumn("dbo.SettingSitoes", "SoloNomeFileMercatino");
            DropColumn("dbo.SettingSitoes", "SoloNomeFileEcommerce");
            DropColumn("dbo.SettingSitoes", "UrlCompletoFileMercatino");
            DropColumn("dbo.SettingSitoes", "UrlCompletoFileEcommerce");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SettingSitoes", "UrlCompletoFileEcommerce", c => c.String(maxLength: 200));
            AddColumn("dbo.SettingSitoes", "UrlCompletoFileMercatino", c => c.String(maxLength: 200));
            AddColumn("dbo.SettingSitoes", "SoloNomeFileEcommerce", c => c.String(maxLength: 200));
            AddColumn("dbo.SettingSitoes", "SoloNomeFileMercatino", c => c.String(maxLength: 200));
            DropColumn("dbo.SettingProgramma", "AmbientePagamentiRateali");
        }
    }
}
