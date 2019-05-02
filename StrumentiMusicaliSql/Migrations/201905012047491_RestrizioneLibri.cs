namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RestrizioneLibri : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articoli", "Libro_TitoloDelLibro", c => c.String(maxLength: 100));
            AlterColumn("dbo.Articoli", "Libro_Autore", c => c.String(maxLength: 20));
            AlterColumn("dbo.Articoli", "Libro_Edizione", c => c.String(maxLength: 20));
            AlterColumn("dbo.Articoli", "Libro_Edizione2", c => c.String(maxLength: 20));
            AlterColumn("dbo.Articoli", "Libro_Genere", c => c.String(maxLength: 20));
            AlterColumn("dbo.Articoli", "Libro_Ordine", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articoli", "Libro_Ordine", c => c.String());
            AlterColumn("dbo.Articoli", "Libro_Genere", c => c.String());
            AlterColumn("dbo.Articoli", "Libro_Edizione2", c => c.String());
            AlterColumn("dbo.Articoli", "Libro_Edizione", c => c.String());
            AlterColumn("dbo.Articoli", "Libro_Autore", c => c.String());
            AlterColumn("dbo.Articoli", "Libro_TitoloDelLibro", c => c.String());
        }
    }
}
