namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixArticoloAnag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articoli", "Iva", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Articoli", "BoxProposte");
            DropColumn("dbo.Articoli", "CaricaInMercatino");
            DropColumn("dbo.Articoli", "PrezzoARichiesta");
            DropColumn("dbo.Articoli", "PrezzoBarrato");
            DropColumn("dbo.Articoli", "UsaAnnuncioTurbo");

            Sql("Update articoli set iva =ArticoloWeb_Iva ");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articoli", "UsaAnnuncioTurbo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articoli", "PrezzoBarrato", c => c.Decimal(nullable: false, precision: 19, scale: 2));
            AddColumn("dbo.Articoli", "PrezzoARichiesta", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articoli", "CaricaInMercatino", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articoli", "BoxProposte", c => c.Boolean(nullable: false));
            DropColumn("dbo.Articoli", "Iva");
        }
    }
}
