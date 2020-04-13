namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllineamentoWeb_4 : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[AggiornamentoWebArticoloes]
           ([ArticoloID]
           ,[DataUltimoAggMagazzino]
           ,[DataUltimoAggMagazzinoWeb]
           ,[DataUltimoAggFoto]
           ,[DataUltimoAggFotoWeb]
           ,[DataCreazione]
           ,[DataUltimaModifica])
     select ID
           ,'01/01/1900'
           ,'01/01/1900'
           ,'01/01/1900'
           ,'01/01/1900'
           ,GETDATE()
           ,GETDATE()
      from Articoli");
        }
        
        public override void Down()
        {
        }
    }
}
