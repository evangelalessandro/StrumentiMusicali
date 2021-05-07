namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SettingProgramma : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SettingProgrammas", newName: "SettingProgramma");

            Sql("If not exists(select 1 from Depositi) " +
                "insert into depositi (NomeDeposito,DataCreazione,DataUltimaModifica,Principale) " +
                "select 'Deposito 1', getdate(),getdate(),1");

            Sql(Properties.Resource1.SQLInitCategorie);
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.SettingProgramma", newName: "SettingProgrammas");
        }
    }
}
