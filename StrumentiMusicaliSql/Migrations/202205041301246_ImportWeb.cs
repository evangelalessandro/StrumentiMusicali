namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportWeb : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Articoli", new[] { "CodiceABarre", "Titolo", "Testo" });
            CreateIndex("dbo.Articoli", new[] { "CodiceABarre", "Titolo" });
            DropColumn("dbo.Articoli", "Testo");
			Sql("update CATEGORIE  " +
				" set REPARTO = REPLACE(REPARTO, '/', '|')   " +
				",    NOME    = REPLACE(NOME   , '/', '|')");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articoli", "Testo", c => c.String(maxLength: 2000));
            DropIndex("dbo.Articoli", new[] { "CodiceABarre", "Titolo" });
            CreateIndex("dbo.Articoli", new[] { "CodiceABarre", "Titolo", "Testo" });
        }
    }
}
