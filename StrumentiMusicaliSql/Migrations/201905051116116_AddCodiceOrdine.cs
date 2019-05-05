namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodiceOrdine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articoli", "Strumento_CodiceOrdine", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articoli", "Strumento_CodiceOrdine");
        }
    }
}
