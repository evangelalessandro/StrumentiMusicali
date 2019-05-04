namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStrumento : DbMigration
    {
        public override void Up()
        {
            Sql("Update Articoli set Strumento_Marca=isnull(Marca,'') " +
                ",Strumento_Modello=isnull(Modello,'')" +
                ",Strumento_Colore=isnull(Colore,'')");
        }
        
        public override void Down()
        {
        }
    }
}
