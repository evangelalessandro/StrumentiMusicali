using System;
using System.Data.Entity.Migrations;


namespace StrumentiMusicali.Library.Migrations
{
    
    public partial class BackupFtpSetting1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SettingBackupFtp", "Porta", c => c.Int(nullable: false));
            AlterColumn("dbo.SettingBackupFtp", "BaseFolder", c => c.String());
            Sql("update Articoli   set Strumento_Nome ='' WHERE (Strumento_Nome IS NULL) ");
            Sql("update Articoli   set Strumento_Rivenditore = LTRIM(rtrim(Strumento_Rivenditore))" +
                " where Strumento_Rivenditore<> LTRIM(rtrim(Strumento_Rivenditore)) ");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SettingBackupFtp", "BaseFolder", c => c.String(maxLength: 100));
            DropColumn("dbo.SettingBackupFtp", "Porta");
        }
    }
}
