namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SottoScorta : DbMigration
    {
        public override void Up()
        {
            Sql(Properties.Resource1.SP_NG_SottoScorta);


            Sql("if not exists(select 1 from TipiDocumentiFiscali)" +
                " insert into TipiDocumentiFiscali(IDEnum,Codice,Descrizione,DataCreazione,DataUltimaModifica)" +
              "" +
              "values( 0,'-','NonSpecificato',getdate(),getdate())," +
              "(1, 'F', 'FatturaDiCortesia', getdate(), getdate()), " +
              "(2, 'NC', 'NotaDiCredito', getdate(), getdate()), " +
              "(4, 'F', 'RicevutaFiscale', getdate(), getdate()), " +
              "(3, 'D', 'DDT', getdate(), getdate()), " +
               "(5, 'ODQ', 'OrdineAlFornitore', getdate(), getdate()), " +
               "(6, 'ODC', 'OrdineDiCarico', getdate(), getdate())  ");



            Sql("INSERT INTO TipiPagamentoDocumenti (  PreCodice, TPP_DESCRIZIONE, TPP_ABILITATO, DataCreazione, DataUltimaModifica) VALUES " +
                "(N'', N'Bonifico Bancario', CONVERT(bit, 'True'), getdate(), getdate()),     " +
                "(N'', N'Rimessa Diretta', CONVERT(bit, 'True'), getdate(), getdate()),       " +
                "(N'', N'CONTRASSEGNO CONTANTI', CONVERT(bit, 'True'), getdate(), getdate())");
        }

        public override void Down()
        {
        }
    }
}
