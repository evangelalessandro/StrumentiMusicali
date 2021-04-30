namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoPagamento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipiPagamentoDocumentis",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PreCodice = c.String(maxLength: 5),
                        Descrizione = c.String(maxLength: 50),
                        Enable = c.Boolean(nullable: false),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Fatture", "IDTipiPagamentoDocumenti", c => c.Int(nullable: false));
            AddColumn("dbo.Fatture", "PagamentoTipo_ID", c => c.Int());
            CreateIndex("dbo.Fatture", "PagamentoTipo_ID");
            AddForeignKey("dbo.Fatture", "PagamentoTipo_ID", "dbo.TipiPagamentoDocumentis", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Fatture", "PagamentoTipo_ID", "dbo.TipiPagamentoDocumentis");
            DropIndex("dbo.Fatture", new[] { "PagamentoTipo_ID" });
            DropColumn("dbo.Fatture", "PagamentoTipo_ID");
            DropColumn("dbo.Fatture", "IDTipiPagamentoDocumenti");
            DropTable("dbo.TipiPagamentoDocumentis");
        }
    }
}
