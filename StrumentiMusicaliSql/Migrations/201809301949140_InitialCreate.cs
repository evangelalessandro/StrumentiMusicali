namespace StrumentiMusicaliSql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articoloes",
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
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Reparto = c.String(),
                        Categoria = c.String(),
                        CategoriaCondivisaCon = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Categories");
            DropTable("dbo.Articoloes");
        }
    }
}
