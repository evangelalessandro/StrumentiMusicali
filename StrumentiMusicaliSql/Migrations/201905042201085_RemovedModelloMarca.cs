namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedModelloMarca : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Articoli", "Marca");
            DropColumn("dbo.Articoli", "Modello");
            DropColumn("dbo.Articoli", "Colore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articoli", "Colore", c => c.String(maxLength: 50));
            AddColumn("dbo.Articoli", "Modello", c => c.String(maxLength: 100));
            AddColumn("dbo.Articoli", "Marca", c => c.String(maxLength: 100));
        }
    }
}
