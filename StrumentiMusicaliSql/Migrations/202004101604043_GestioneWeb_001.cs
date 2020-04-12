namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GestioneWeb_001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articoli", "ArticoloWeb_PrezzoWeb", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Articoli", "ArticoloWeb_Iva", c => c.Decimal(nullable: false, precision: 18, scale: 2));

            Sql("Update Articoli set ArticoloWeb_PrezzoWeb=prezzo , Titolo=LTRIM(rtrim(Titolo)) ");
             
            Sql("Update Articoli set ArticoloWeb_Iva=22 ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articoli", "ArticoloWeb_Iva");
            DropColumn("dbo.Articoli", "ArticoloWeb_PrezzoWeb");
        }
    }
}
