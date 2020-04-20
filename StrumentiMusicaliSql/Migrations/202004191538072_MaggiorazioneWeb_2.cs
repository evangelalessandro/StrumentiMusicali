namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaggiorazioneWeb_2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"update Categorie
                  set PercMaggNegozioRispettoWebSeAqtZero=-20");

            Sql(@"update Categorie
                  set PercMaggNegozioRispettoWebSeAqtZero=-30
                where Codice in (
                71
                ,309
                ,27
                ,302
                ,155
                ,24
                ,98
                ,27
                ,256
                )");

            Sql(@"update Categorie
                  set PercMaggNegozioRispettoWebSeAqtZero=-25
                where Codice in (
                83
                ,275
                ,62
                ,253
                ,277
                ,260
                ,306
                ,51
                ,154
                ,304
                ,28
                ,52
                ,252
                ,37
                ,9
                )");

            Sql(@"update Categorie
                  set PercMaggNegozioRispettoWebSeAqtZero=-15
                where Codice in (
                84
                ,5
                )");

            Sql(@"	update Articoli
                  set ArticoloWeb_PrezzoWeb= PrezzoAcquisto * (100+ c.PercMaggNegozioRispettoWeb)/100
	                from Articoli a inner join Categorie c on a.CategoriaID=c.ID
                    where PrezzoAcquisto >0
                      and c.Codice >0

                  update Articoli
                  set ArticoloWeb_PrezzoWeb= Prezzo * (100 + c.PercMaggNegozioRispettoWebSeAqtZero)/100
	                from Articoli a inner join Categorie c on a.CategoriaID=c.ID
                    where PrezzoAcquisto =0
                      and c.Codice >0
                ");

            Sql(@" 
            -- =============================================
            --Author:		Name
            -- Create date:
            --Description:	
            -- =============================================
            CREATE OR ALTER TRIGGER[dbo].[tr_Magazzino]
               ON[dbo].[Magazzino]
               AFTER INSERT, DELETE, UPDATE
            AS
            BEGIN
                -- SET NOCOUNT ON added to prevent extra result sets from
                -- interfering with SELECT statements.

                SET NOCOUNT ON;

                        declare @fIns as bit,
                    @fDel as bit,
                    @errore nvarchar(255),
                    @nDummy int

            SELECT* INTO #Magazzino FROM INSERTED
            set @fIns = @@ROWCOUNT
            set @fDel = CASE WHEN exists(SELECT ID FROM DELETED) THEN 1 ELSE 0 END
            if @fIns = 0 and @fDel = 0
             return
                --Insert statements for trigger here

             update AggiornamentoWebArticoloes

             set DataUltimoAggMagazzino = getdate()

             where ArticoloID in (select ArticoloID from #Magazzino a
                                    where a.OperazioneWeb=0)

            END
            GO


            ALTER TABLE[dbo].[Magazzino] ENABLE TRIGGER[tr_Magazzino]
            GO
            ");
        }

        public override void Down()
        {
        }
    }
}
