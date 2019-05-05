namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodiceOrdine1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categorie", "Reparto", c => c.String(maxLength: 50));
            AlterColumn("dbo.Categorie", "Nome", c => c.String(maxLength: 50));

        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categorie", "Nome", c => c.String());
            AlterColumn("dbo.Categorie", "Reparto", c => c.String());
        }
    }
}
