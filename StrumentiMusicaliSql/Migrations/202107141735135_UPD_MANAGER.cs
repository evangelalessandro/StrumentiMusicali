namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UPD_MANAGER : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UPD_Postazioni",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    NomePostazione = c.String(nullable: false, maxLength: 20),
                    Versione = c.Int(nullable: false),
                    ForceUpdate = c.Boolean(nullable: false),
                    Note = c.String(),
                    DataCreazione = c.DateTime(nullable: false),
                    DataUltimaModifica = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ID)
                .Index(t => t.NomePostazione, unique: true);

            CreateTable(
                "dbo.UPD_SettingPostazione",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Versione = c.Int(nullable: false),
                    PostazioneServer = c.String(),
                    DataCreazione = c.DateTime(nullable: false),
                    DataUltimaModifica = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            Sql("INSERT INTO UPD_SettingPostazione(Versione, PostazioneServer, DataCreazione, DataUltimaModifica)" +
                "VALUES(10, N'SERVERSM', GETDATE(), GETDATE())");


            Sql("INSERT INTO[dbo].[UPD_Postazioni]" +
                "([NomePostazione],[Versione],[ForceUpdate],[Note],[DataCreazione],[DataUltimaModifica])" +
             "VALUES " +
                   "('SERVERSM' " +
                   ", 10        " +
                   ", 0         " +
                   ", ''        " +
                   ", getdate() " +
                   ", getdate())" +
                    "");

            SqlFile(@"Script\\CheckUpdate.sql");

        }

        public override void Down()
        {
            DropIndex("dbo.UPD_Postazioni", new[] { "NomePostazione" });
            DropTable("dbo.UPD_SettingPostazione");
            DropTable("dbo.UPD_Postazioni");
        }
    }
}
