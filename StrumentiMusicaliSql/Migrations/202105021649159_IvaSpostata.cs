namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IvaSpostata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articoli", "Iva", c => c.Decimal(nullable: false, precision: 18, scale: 2));

            Sql("Update articoli set iva = ArticoloWeb_Iva ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articoli", "Iva");
        }
    }
}
