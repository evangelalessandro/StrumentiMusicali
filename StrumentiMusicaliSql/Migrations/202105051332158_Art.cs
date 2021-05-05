namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Art : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Articoli", "BoxProposte");
            DropColumn("dbo.Articoli", "PrezzoARichiesta");
            DropColumn("dbo.Articoli", "PrezzoBarrato");
            DropColumn("dbo.Articoli", "UsaAnnuncioTurbo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articoli", "UsaAnnuncioTurbo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articoli", "PrezzoBarrato", c => c.Decimal(nullable: false, precision: 19, scale: 2));
            AddColumn("dbo.Articoli", "PrezzoARichiesta", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articoli", "BoxProposte", c => c.Boolean(nullable: false));
        }
    }
}
