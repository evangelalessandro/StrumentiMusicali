namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeriodiRiordino : DbMigration
    {
        public override void Up()
        {
            
            Sql("" +
                "" +
                "SET DATEFORMAT ymd " +Environment.NewLine +
                "if not exists(select 1 from RiordinoPeriodi where TuttoAnno=1) " +
              " INSERT INTO[RiordinoPeriodi](Descrizione, TuttoAnno, PeriodoSottoScortaInizio, PeriodoSottoScortaFine, DataCreazione, DataUltimaModifica) VALUES " +
              "  (N'Tutto l''anno', 1, CONVERT(DATETIME, '1900-01-01 00:00:00.000', 121), CONVERT(DATETIME, '1900-12-31 00:00:00.000', 121), CONVERT(DATETIME, '2021-05-05 09:35:03.880', 121), CONVERT(DATETIME, '2021-05-05 09:35:14.063', 121)) ");


            AddColumn("dbo.Articoli", "RiordinoPeriodiID", c => c.Int(nullable:true));

            Sql("update articoli " +
                "set RiordinoPeriodiID =pr.ID " +
                "" +
                "from articoli join RiordinoPeriodi pr on TuttoAnno=1");

            AlterColumn("dbo.Articoli", "RiordinoPeriodiID", c => c.Int(nullable: false));


            CreateIndex("dbo.Articoli", "RiordinoPeriodiID");

            AddForeignKey("dbo.Articoli", "RiordinoPeriodiID", "dbo.RiordinoPeriodi", "ID");

        }

        public override void Down()
        {
            DropForeignKey("dbo.Articoli", "RiordinoPeriodiID", "dbo.RiordinoPeriodi");
            DropIndex("dbo.Articoli", new[] { "RiordinoPeriodiID" });
            DropColumn("dbo.Articoli", "RiordinoPeriodiID");
        }
    }
}
