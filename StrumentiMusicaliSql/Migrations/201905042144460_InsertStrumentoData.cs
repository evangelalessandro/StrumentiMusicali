namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertStrumentoData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articoli", "Strumento_Marca", c => c.String(maxLength: 100));
            AddColumn("dbo.Articoli", "Strumento_Modello", c => c.String(maxLength: 100));
            AddColumn("dbo.Articoli", "Strumento_Colore", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articoli", "Strumento_Colore");
            DropColumn("dbo.Articoli", "Strumento_Modello");
            DropColumn("dbo.Articoli", "Strumento_Marca");
        }
    }
}
