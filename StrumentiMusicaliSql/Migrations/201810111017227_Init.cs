namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articoli",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50),
                        Categoria = c.Int(nullable: false),
                        Condizione = c.Int(nullable: false),
                        Marca = c.String(maxLength: 100),
                        Titolo = c.String(nullable: false, maxLength: 100),
                        Testo = c.String(nullable: false, maxLength: 2000),
                        Prezzo = c.Decimal(nullable: false, precision: 19, scale: 2),
                        PrezzoBarrato = c.Decimal(nullable: false, precision: 19, scale: 2),
                        PrezzoARichiesta = c.Boolean(nullable: false),
                        UrlSchedaProdotto = c.String(),
                        UrlSchedaProdottoTurbo = c.String(),
                        BoxProposte = c.Boolean(nullable: false),
                        UsaAnnuncioTurbo = c.Boolean(nullable: false),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                        Pinned = c.Boolean(nullable: false),
                        Giacenza = c.Int(nullable: false),
                        CodiceAbarre = c.String(maxLength: 100),
                        CaricainEcommerce = c.Boolean(nullable: false),
                        CaricainMercatino = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Categorie",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Reparto = c.String(),
                        Categoria = c.String(),
                        CategoriaCondivisaCon = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Depositi",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NomeDeposito = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FotoArticoli",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        ArticoloID = c.String(maxLength: 50),
                        UrlFoto = c.String(nullable: false),
                        Ordine = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articoli", t => t.ArticoloID)
                .Index(t => t.ArticoloID);
            
            CreateTable(
                "dbo.EventLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Evento = c.String(),
                        TipoEvento = c.String(),
                        Errore = c.String(),
                        StackTrace = c.String(),
                        InnerException = c.String(),
                        Class = c.String(),
                        ThreadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Magazzino",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        ArticoloID = c.String(nullable: false, maxLength: 50),
                        DepositoID = c.Int(nullable: false),
                        Giacenza = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articoli", t => t.ArticoloID, cascadeDelete: true)
                .ForeignKey("dbo.Depositi", t => t.DepositoID, cascadeDelete: true)
                .Index(t => t.ArticoloID)
                .Index(t => t.DepositoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Magazzino", "DepositoID", "dbo.Depositi");
            DropForeignKey("dbo.Magazzino", "ArticoloID", "dbo.Articoli");
            DropForeignKey("dbo.FotoArticoli", "ArticoloID", "dbo.Articoli");
            DropIndex("dbo.Magazzino", new[] { "DepositoID" });
            DropIndex("dbo.Magazzino", new[] { "ArticoloID" });
            DropIndex("dbo.FotoArticoli", new[] { "ArticoloID" });
            DropTable("dbo.Magazzino");
            DropTable("dbo.EventLogs");
            DropTable("dbo.FotoArticoli");
            DropTable("dbo.Depositi");
            DropTable("dbo.Categorie");
            DropTable("dbo.Articoli");
        }
    }
}
