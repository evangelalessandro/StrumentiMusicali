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
                        BoxProposte = c.Boolean(nullable: false),
                        CaricainEcommerce = c.Boolean(nullable: false),
                        CaricainMercatino = c.Boolean(nullable: false),
                        Categoria = c.Int(nullable: false),
                        CodiceAbarre = c.String(maxLength: 100),
                        Condizione = c.Int(nullable: false),
                        ImmaginiDaCaricare = c.Boolean(nullable: false),
                        Marca = c.String(maxLength: 100),
                        Pinned = c.Boolean(nullable: false),
                        Prezzo = c.Decimal(nullable: false, precision: 19, scale: 2),
                        PrezzoARichiesta = c.Boolean(nullable: false),
                        PrezzoBarrato = c.Decimal(nullable: false, precision: 19, scale: 2),
                        Testo = c.String(nullable: false, maxLength: 2000),
                        Titolo = c.String(nullable: false, maxLength: 100),
                        UrlSchedaProdotto = c.String(),
                        UrlSchedaProdottoTurbo = c.String(),
                        UsaAnnuncioTurbo = c.Boolean(nullable: false),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
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
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Clienti",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PIVA = c.String(),
                        RagioneSociale = c.String(),
                        Via = c.String(),
                        Citta = c.String(),
                        Cap = c.Int(nullable: false),
                        Telefono = c.String(),
                        Fax = c.String(),
                        Cellulare = c.String(),
                        LuogoDestinazione = c.String(),
                        NomeBanca = c.String(),
                        BancaAbi = c.Int(nullable: false),
                        BancaCab = c.Int(nullable: false),
                        CodiceClienteOld = c.Int(nullable: false),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DDT",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Codice = c.String(nullable: false),
                        Data = c.DateTime(nullable: false),
                        RagioneSociale = c.String(),
                        PIVA = c.String(),
                        ClienteID = c.Int(nullable: false),
                        TipoDocumento = c.Int(nullable: false),
                        TrasportoACura = c.String(),
                        CausaleTrasporto = c.String(),
                        Porto = c.String(),
                        Vettore = c.String(),
                        AspettoEsterno = c.String(),
                        NumeroColli = c.Int(nullable: false),
                        PesoKg = c.Int(nullable: false),
                        DataTrasporto = c.DateTime(),
                        OraTrasporto = c.DateTime(),
                        Note1 = c.String(),
                        Note2 = c.String(),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clienti", t => t.ClienteID, cascadeDelete: true)
                .Index(t => t.ClienteID);
            
            CreateTable(
                "dbo.DDTRighe",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DDTID = c.Int(nullable: false),
                        CodiceArticoloOld = c.String(),
                        ArticoloID = c.String(maxLength: 50),
                        Descrizione = c.String(),
                        Qta = c.Int(nullable: false),
                        OrdineVisualizzazione = c.Int(nullable: false),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articoli", t => t.ArticoloID)
                .ForeignKey("dbo.DDT", t => t.DDTID, cascadeDelete: true)
                .Index(t => t.DDTID)
                .Index(t => t.ArticoloID);
            
            CreateTable(
                "dbo.Depositi",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NomeDeposito = c.String(),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Fatture",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Pagamento = c.String(),
                        ImportoTotale = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotaleIva = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotaleFattura = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Codice = c.String(nullable: false),
                        Data = c.DateTime(nullable: false),
                        RagioneSociale = c.String(),
                        PIVA = c.String(),
                        ClienteID = c.Int(nullable: false),
                        TipoDocumento = c.Int(nullable: false),
                        TrasportoACura = c.String(),
                        CausaleTrasporto = c.String(),
                        Porto = c.String(),
                        Vettore = c.String(),
                        AspettoEsterno = c.String(),
                        NumeroColli = c.Int(nullable: false),
                        PesoKg = c.Int(nullable: false),
                        DataTrasporto = c.DateTime(),
                        OraTrasporto = c.DateTime(),
                        Note1 = c.String(),
                        Note2 = c.String(),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Clienti", t => t.ClienteID, cascadeDelete: true)
                .Index(t => t.ClienteID);
            
            CreateTable(
                "dbo.FattureRighe",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FatturaID = c.Int(nullable: false),
                        PrezzoUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IvaApplicata = c.String(),
                        CodiceArticoloOld = c.String(),
                        ArticoloID = c.String(maxLength: 50),
                        Descrizione = c.String(),
                        Qta = c.Int(nullable: false),
                        OrdineVisualizzazione = c.Int(nullable: false),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articoli", t => t.ArticoloID)
                .ForeignKey("dbo.Fatture", t => t.FatturaID, cascadeDelete: true)
                .Index(t => t.FatturaID)
                .Index(t => t.ArticoloID);
            
            CreateTable(
                "dbo.FotoArticoli",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        ArticoloID = c.String(maxLength: 50),
                        UrlFoto = c.String(nullable: false),
                        Ordine = c.Int(nullable: false),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
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
                        ID = c.Int(nullable: false, identity: true),
                        ArticoloID = c.String(nullable: false, maxLength: 50),
                        DepositoID = c.Int(nullable: false),
                        Qta = c.Int(nullable: false),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
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
            DropForeignKey("dbo.FattureRighe", "FatturaID", "dbo.Fatture");
            DropForeignKey("dbo.FattureRighe", "ArticoloID", "dbo.Articoli");
            DropForeignKey("dbo.Fatture", "ClienteID", "dbo.Clienti");
            DropForeignKey("dbo.DDTRighe", "DDTID", "dbo.DDT");
            DropForeignKey("dbo.DDTRighe", "ArticoloID", "dbo.Articoli");
            DropForeignKey("dbo.DDT", "ClienteID", "dbo.Clienti");
            DropIndex("dbo.Magazzino", new[] { "DepositoID" });
            DropIndex("dbo.Magazzino", new[] { "ArticoloID" });
            DropIndex("dbo.FotoArticoli", new[] { "ArticoloID" });
            DropIndex("dbo.FattureRighe", new[] { "ArticoloID" });
            DropIndex("dbo.FattureRighe", new[] { "FatturaID" });
            DropIndex("dbo.Fatture", new[] { "ClienteID" });
            DropIndex("dbo.DDTRighe", new[] { "ArticoloID" });
            DropIndex("dbo.DDTRighe", new[] { "DDTID" });
            DropIndex("dbo.DDT", new[] { "ClienteID" });
            DropTable("dbo.Magazzino");
            DropTable("dbo.EventLogs");
            DropTable("dbo.FotoArticoli");
            DropTable("dbo.FattureRighe");
            DropTable("dbo.Fatture");
            DropTable("dbo.Depositi");
            DropTable("dbo.DDTRighe");
            DropTable("dbo.DDT");
            DropTable("dbo.Clienti");
            DropTable("dbo.Categorie");
            DropTable("dbo.Articoli");
        }
    }
}
