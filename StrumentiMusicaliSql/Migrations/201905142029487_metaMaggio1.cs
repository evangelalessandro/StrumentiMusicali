namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class metaMaggio1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articoli", "Strumento_Rivenditore", c => c.String(maxLength: 100));
            Sql("Update Articoli set Strumento_Rivenditore=isnull(Rivenditore,'') ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articoli", "Strumento_Rivenditore");
        }
    }
}
