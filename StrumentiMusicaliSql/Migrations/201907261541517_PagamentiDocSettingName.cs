namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PagamentiDocSettingName : DbMigration
    {
        public override void Up()
        {
            Sql(
                "IF object_id('[dbo.SettingSitoes]') IS NOT NULL BEGIN " +

    " EXECUTE sp_rename @objname = N'dbo.SettingSito', " +
   " @newname = N'SettingSitoes' " +
" end "+
"IF object_id('[PK_dbo.SettingSito]') IS NOT NULL BEGIN " +
"    EXECUTE sp_rename @objname = N'[PK_dbo.SettingSito]', @newname = N'PK_dbo.SettingSitoes' " +
"END");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.SettingSitoes", newName: "SettingSito");
        }
    }
}
