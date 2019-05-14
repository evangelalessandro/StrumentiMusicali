namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpostatoRivenditore : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Articoli", "Rivenditore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articoli", "Rivenditore", c => c.String(maxLength: 100));
        }
    }
}
