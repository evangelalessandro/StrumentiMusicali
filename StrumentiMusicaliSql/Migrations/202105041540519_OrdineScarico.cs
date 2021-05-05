namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdineScarico : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.TipiDocumentiFiscali", "IDEnum", unique: true, clustered: false, name: "TipoDocumentoFiscale_IX_ENUM");
            DropColumn("dbo.Articoli", "PeriodoSottoScortaInizio");
            DropColumn("dbo.Articoli", "PeriodoSottoScortaFine");

            Sql("if not exists(select 1 from TipiDocumentiFiscali where IDEnum=7) " +
                " insert into TipiDocumentiFiscali(IDEnum,Codice,Descrizione,DataCreazione,DataUltimaModifica)" +
                "" +
                "values " +
                 "(7, 'ODS', 'Ordine di Scarico', getdate(), getdate())  ");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articoli", "PeriodoSottoScortaFine", c => c.DateTime(nullable: false));
            AddColumn("dbo.Articoli", "PeriodoSottoScortaInizio", c => c.DateTime(nullable: false));
            DropIndex("dbo.TipiDocumentiFiscali", "TipoDocumentoFiscale_IX_ENUM");
        }
    }
}
