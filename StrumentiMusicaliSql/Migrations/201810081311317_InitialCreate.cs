namespace StrumentiMusicaliSql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articoli",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        Categoria = c.Int(nullable: false),
                        Condizione = c.Int(nullable: false),
                        Marca = c.String(nullable: false, maxLength: 100),
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
                "dbo.FotoArticoloes",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        UrlFoto = c.String(nullable: false),
                        Articolo_ID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articoli", t => t.Articolo_ID, cascadeDelete: true)
                .Index(t => t.Articolo_ID);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FotoArticoloes", "Articolo_ID", "dbo.Articoli");
            DropIndex("dbo.FotoArticoloes", new[] { "Articolo_ID" });
            DropTable("dbo.EventLogs");
            DropTable("dbo.FotoArticoloes");
            DropTable("dbo.Categorie");
            DropTable("dbo.Articoli");
        }
    }
}
