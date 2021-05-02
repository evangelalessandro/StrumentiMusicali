namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ListinoPrezzi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ListinoPrezzi_Articoli",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArticoloID = c.Int(nullable: false),
                        ListinoPrezziVenditaNomeID = c.Int(nullable: false),
                        Imponibile = c.Decimal(precision: 18, scale: 2),
                        Ivato = c.Decimal(precision: 18, scale: 2),
                        RicaricoPercSuPrezzoAcquisto = c.Int(),
                        Sconto1 = c.Int(),
                        Sconto2 = c.Int(),
                        Sconto3 = c.Int(),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articoli", t => t.ArticoloID)
                .ForeignKey("dbo.ListiniPrezzi_VenditaNomi", t => t.ListinoPrezziVenditaNomeID)
                .Index(t => t.ArticoloID)
                .Index(t => t.ListinoPrezziVenditaNomeID);
            
            CreateTable(
                "dbo.ListiniPrezzi_VenditaNomi",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 15),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Nome, unique: true);

            Sql(Properties.Resource1.SP_NG_SottoScorta_rev1);


            DropColumn("dbo.Articoli", "ArticoloWeb_Iva");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListinoPrezzi_Articoli", "ListinoPrezziVenditaNomeID", "dbo.ListiniPrezzi_VenditaNomi");
            DropForeignKey("dbo.ListinoPrezzi_Articoli", "ArticoloID", "dbo.Articoli");
            DropIndex("dbo.ListiniPrezzi_VenditaNomi", new[] { "Nome" });
            DropIndex("dbo.ListinoPrezzi_Articoli", new[] { "ListinoPrezziVenditaNomeID" });
            DropIndex("dbo.ListinoPrezzi_Articoli", new[] { "ArticoloID" });
            DropTable("dbo.ListiniPrezzi_VenditaNomi");
            DropTable("dbo.ListinoPrezzi_Articoli");
        }
    }
}
