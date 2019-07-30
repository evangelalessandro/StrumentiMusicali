namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BackupFtpSetting1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SettingBackupFtp", "Porta", c => c.Int(nullable: false));
            AlterColumn("dbo.SettingBackupFtp", "BaseFolder", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SettingBackupFtp", "BaseFolder", c => c.String(maxLength: 100));
            DropColumn("dbo.SettingBackupFtp", "Porta");
        }
    }
}
