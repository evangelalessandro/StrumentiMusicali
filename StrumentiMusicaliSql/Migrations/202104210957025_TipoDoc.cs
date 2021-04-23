namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoDoc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipiDocumentiFiscali",
                c => new
                    {
                        TIP_TIPDOC_ID = c.Int(nullable: false, identity: true),
                        IDEnum = c.Int(nullable: false),
                        Codice = c.String(maxLength: 10),
                        Descrizione = c.String(maxLength: 50),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TIP_TIPDOC_ID);

            Sql("insert into TipiDocumentiFiscali(IDEnum,Codice,Descrizione,DataCreazione,DataUltimaModifica)" +
                "" +
                "values( 0,'-','NonSpecificato',getdate(),getdate())," +
                "(1, 'F', 'FatturaDiCortesia', getdate(), getdate()), " +
                "(2, 'NC', 'NotaDiCredito', getdate(), getdate()), " +
                "(4, 'F', 'RicevutaFiscale', getdate(), getdate()), " +
                "(3, 'D', 'DDT', getdate(), getdate()), " +
                 "(5, 'ODQ', 'OrdineAlFornitore', getdate(), getdate()), " +
                 "(6, 'ODC', 'OrdineDiCarico', getdate(), getdate())  ");

            SqlResource(Properties.Resource1.SP_NG_SottoScorta);
            
            AlterColumn("dbo.Fatture", "RagioneSociale", c => c.String(maxLength: 150));
            AlterColumn("dbo.Fatture", "PIVA", c => c.String(maxLength: 50));
            AlterColumn("dbo.Fatture", "TrasportoACura", c => c.String(maxLength: 150));
            AlterColumn("dbo.Fatture", "CausaleTrasporto", c => c.String(maxLength: 100));
            AlterColumn("dbo.Fatture", "Porto", c => c.String(maxLength: 150));
            AlterColumn("dbo.Fatture", "Vettore", c => c.String(maxLength: 150));
            AlterColumn("dbo.Fatture", "AspettoEsterno", c => c.String(maxLength: 150));
            AlterColumn("dbo.Fatture", "Note1", c => c.String(maxLength: 500));
            AlterColumn("dbo.Fatture", "Note2", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Fatture", "Note2", c => c.String());
            AlterColumn("dbo.Fatture", "Note1", c => c.String());
            AlterColumn("dbo.Fatture", "AspettoEsterno", c => c.String());
            AlterColumn("dbo.Fatture", "Vettore", c => c.String());
            AlterColumn("dbo.Fatture", "Porto", c => c.String());
            AlterColumn("dbo.Fatture", "CausaleTrasporto", c => c.String());
            AlterColumn("dbo.Fatture", "TrasportoACura", c => c.String());
            AlterColumn("dbo.Fatture", "PIVA", c => c.String());
            AlterColumn("dbo.Fatture", "RagioneSociale", c => c.String());
            DropTable("dbo.TipiDocumentiFiscali");
        }
    }
}
