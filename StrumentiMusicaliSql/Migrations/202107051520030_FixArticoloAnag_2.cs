namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixArticoloAnag_2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Articoli", "ArticoloWeb_Iva");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articoli", "ArticoloWeb_Iva", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
