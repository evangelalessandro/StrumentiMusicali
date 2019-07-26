namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentiPagamenti : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SettingSitoes", "CartellaReteDocumentiPagamenti", c => c.String(maxLength: 200));
            AddColumn("dbo.Pagamenti", "IDPagamentoMaster", c => c.Guid(nullable: false));
            AlterColumn("dbo.SettingSitoes", "UrlSito", c => c.String(maxLength: 200));
            AlterColumn("dbo.SettingSitoes", "UrlCompletaImmagini", c => c.String(maxLength: 200));
            AlterColumn("dbo.SettingSitoes", "CartellaLocaleImmagini", c => c.String(maxLength: 200));
            AlterColumn("dbo.SettingSitoes", "SoloNomeFileMercatino", c => c.String(maxLength: 200));
            AlterColumn("dbo.SettingSitoes", "SoloNomeFileEcommerce", c => c.String(maxLength: 200));
            AlterColumn("dbo.SettingSitoes", "UrlCompletoFileMercatino", c => c.String(maxLength: 200));
            AlterColumn("dbo.SettingSitoes", "UrlCompletoFileEcommerce", c => c.String(maxLength: 200));

            Sql("Update Pagamenti set IDPagamentoMaster=IDPagamenti ");

            DropColumn("dbo.Pagamenti", "IDPagamenti");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pagamenti", "IDPagamenti", c => c.Guid(nullable: false));
            AlterColumn("dbo.SettingSitoes", "UrlCompletoFileEcommerce", c => c.String());
            AlterColumn("dbo.SettingSitoes", "UrlCompletoFileMercatino", c => c.String());
            AlterColumn("dbo.SettingSitoes", "SoloNomeFileEcommerce", c => c.String());
            AlterColumn("dbo.SettingSitoes", "SoloNomeFileMercatino", c => c.String());
            AlterColumn("dbo.SettingSitoes", "CartellaLocaleImmagini", c => c.String());
            AlterColumn("dbo.SettingSitoes", "UrlCompletaImmagini", c => c.String());
            AlterColumn("dbo.SettingSitoes", "UrlSito", c => c.String());
            DropColumn("dbo.Pagamenti", "IDPagamentoMaster");
            DropColumn("dbo.SettingSitoes", "CartellaReteDocumentiPagamenti");
        }
    }
}
