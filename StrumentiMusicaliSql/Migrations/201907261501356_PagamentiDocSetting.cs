namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PagamentiDocSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SettingDocumentiPagamenti",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CartellaReteDocumentiPagamenti = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.SettingSitoes", "CartellaReteDocumentiPagamenti");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SettingSitoes", "CartellaReteDocumentiPagamenti", c => c.String(maxLength: 200));
            DropTable("dbo.SettingDocumentiPagamenti");
        }
    }
}
