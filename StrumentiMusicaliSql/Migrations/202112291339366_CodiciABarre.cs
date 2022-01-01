namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodiciABarre : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CONF_CodiciABarre",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CodiceABarre = c.String(nullable: false, maxLength: 50),
                        Azione = c.String(maxLength: 50),
                        Descrizione = c.String(maxLength: 50),
                        CodiceIva = c.Int(nullable: false),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.CodiceABarre, unique: true);

            Sql(Properties.Resource1.sqlCodiciABarre);
            Sql(Properties.Resource1.SpBackup);   
        }
        
        public override void Down()
        {
            DropIndex("dbo.CONF_CodiciABarre", new[] { "CodiceABarre" });
            DropTable("dbo.CONF_CodiciABarre");
        }
    }
}
