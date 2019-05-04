namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStrumentoClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articoli", "Modello", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articoli", "Modello");
        }
    }
}
