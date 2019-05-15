namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PagamentiDeleteArt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pagamenti", "ArticoloID", "dbo.Articoli");
            DropIndex("dbo.Pagamenti", new[] { "ArticoloID" });
            AddColumn("dbo.Pagamenti", "Indirizzo", c => c.String(maxLength: 120));
            AddColumn("dbo.Pagamenti", "Telefono", c => c.String(maxLength: 60));
            AddColumn("dbo.Pagamenti", "CartaIdentita", c => c.String(maxLength: 60));
            AddColumn("dbo.Pagamenti", "Note", c => c.String(maxLength: 500));
            AddColumn("dbo.Pagamenti", "ArticoloAcq", c => c.String(maxLength: 100));
            AlterColumn("dbo.Pagamenti", "Cognome", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Pagamenti", "Nome", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Pagamenti", "ArticoloID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pagamenti", "ArticoloID", c => c.Int(nullable: false));
            AlterColumn("dbo.Pagamenti", "Nome", c => c.String(nullable: false));
            AlterColumn("dbo.Pagamenti", "Cognome", c => c.String(nullable: false));
            DropColumn("dbo.Pagamenti", "ArticoloAcq");
            DropColumn("dbo.Pagamenti", "Note");
            DropColumn("dbo.Pagamenti", "CartaIdentita");
            DropColumn("dbo.Pagamenti", "Telefono");
            DropColumn("dbo.Pagamenti", "Indirizzo");
            CreateIndex("dbo.Pagamenti", "ArticoloID");
            AddForeignKey("dbo.Pagamenti", "ArticoloID", "dbo.Articoli", "ID");
        }
    }
}
