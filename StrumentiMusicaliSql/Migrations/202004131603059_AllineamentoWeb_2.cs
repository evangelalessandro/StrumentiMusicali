namespace StrumentiMusicali.Library.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllineamentoWeb_2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Articoli", "DataUltimoAggMagazzino");
            DropColumn("dbo.Articoli", "DataUltimoAggFoto");
            Sql(@"CREATE OR ALTER TRIGGER [dbo].[tr_FotoArticoli]
   ON  [dbo].[FotoArticoli]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  declare @fIns as bit,
        @fDel as bit,
        @errore nvarchar(255),
        @nDummy int

SELECT * INTO #fotoArticoli FROM INSERTED
set @fIns = @@ROWCOUNT
set @fDel = CASE WHEN exists (SELECT ID FROM DELETED) THEN 1 ELSE 0 END
if @fIns = 0 and @fDel = 0
 return
    -- Insert statements for trigger here
  update Articoli
  set DataUltimoAggFoto=getdate()
  where ID in (select ArticoloID from #fotoArticoli a)

END
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

 update Articoli

 set DataUltimoAggMagazzino = getdate()

 where ID in (select ArticoloID from #Magazzino a)

END
GO


ALTER TABLE[dbo].[Magazzino] ENABLE TRIGGER[tr_Magazzino]
GO
");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articoli", "DataUltimoAggFoto", c => c.DateTime(nullable: false));
            AddColumn("dbo.Articoli", "DataUltimoAggMagazzino", c => c.DateTime(nullable: false));
        }
    }
}
