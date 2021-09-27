namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegistratoreDiCassa1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistratoreDiCassaRepartis",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NomeCategoria = c.String(nullable: false, maxLength: 20),
                        Iva = c.Int(nullable: false),
                        CodicePerRegistratoreDiCassa = c.Int(nullable: false),
                        GruppoCodiceRegCassaID = c.Int(nullable: false),
                        DataCreazione = c.DateTime(nullable: false),
                        DataUltimaModifica = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.GruppoCodiceRegCassas", t => t.GruppoCodiceRegCassaID)
                .Index(t => t.NomeCategoria, unique: true)
                .Index(t => t.GruppoCodiceRegCassaID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegistratoreDiCassaRepartis", "GruppoCodiceRegCassaID", "dbo.GruppoCodiceRegCassas");
            DropIndex("dbo.RegistratoreDiCassaRepartis", new[] { "GruppoCodiceRegCassaID" });
            DropIndex("dbo.RegistratoreDiCassaRepartis", new[] { "NomeCategoria" });
            DropTable("dbo.RegistratoreDiCassaRepartis");
        }
    }
}
