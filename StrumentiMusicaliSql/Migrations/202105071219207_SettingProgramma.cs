namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SettingProgramma : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SettingProgrammas", newName: "SettingProgramma");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.SettingProgramma", newName: "SettingProgrammas");
        }
    }
}
