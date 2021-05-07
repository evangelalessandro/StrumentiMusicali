namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testoArt : DbMigration
    {
        public override void Up()
        {

            DropIndex("dbo.Articoli", new[] { "CodiceABarre", "Titolo", "Testo" });
            AddColumn("dbo.Articoli", "ArticoloWeb_Testo", c => c.String(maxLength: 2000));
            AddColumn("dbo.SettingProgramma", "AbilitazioneEcommerce", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Articoli", new[] { "CodiceABarre", "Titolo", "CodiceInterno" });

            Sql("Update articoli set ArticoloWeb_Testo=Testo ");
            DropColumn("dbo.Articoli", "Testo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articoli", "Testo", c => c.String(maxLength: 2000));
            DropIndex("dbo.Articoli", new[] { "CodiceABarre", "Titolo", "CodiceInterno" });
            DropColumn("dbo.SettingProgramma", "AbilitazioneEcommerce");
            DropColumn("dbo.Articoli", "ArticoloWeb_Testo");
            CreateIndex("dbo.Articoli", new[] { "CodiceABarre", "Titolo", "Testo" });
        }
    }
}
