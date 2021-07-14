CREATE or alter PROCEDURE UPD_CHECKUPDATE 
	-- Add the parameters for the stored procedure here
	@pNomePostazione nvarchar(max)  
AS
BEGIN
 

 IF NOT EXISTS(SELECT 1
  FROM UPD_Postazioni 
  WHERE NomePostazione=@pNomePostazione)
  BEGIN
    INSERT INTO UPD_Postazioni (NomePostazione, Versione, ForceUpdate, Note, DataCreazione, DataUltimaModifica)
    VALUES(@pNomePostazione, 0, 0, N'', GETDATE(), GETDATE());
  END
    -- Insert statements for procedure here
	DECLARE @toUpdate nvarchar(MAX) =''
  SELECT @toUpdate =CASE WHEN a.Versione <>b.Versione THEN 'TOUPDATE' ELSE 'UPDATED' end
  FROM UPD_Postazioni a INNER JOIN UPD_SettingPostazione b ON 1=1
  WHERE NomePostazione=@pNomePostazione

  SELECT @toUpdate


END
GO

CREATE or alter PROCEDURE UPD_UPDATED
	-- Add the parameters for the stored procedure here
	@pNomePostazione nvarchar(max)  
AS
BEGIN
 

 IF NOT EXISTS(SELECT 1
  FROM UPD_Postazioni 
  WHERE NomePostazione=@pNomePostazione)
  BEGIN
    INSERT INTO UPD_Postazioni (NomePostazione, Versione, ForceUpdate, Note, DataCreazione, DataUltimaModifica)
    VALUES(@pNomePostazione, 0, 0, N'', GETDATE(), GETDATE());
  END
    -- Insert statements for procedure here
	
  UPDATE UPD_Postazioni 
  SET Versione=b.Versione,
    DataUltimaModifica=getdate(),
    ForceUpdate=0
  FROM UPD_Postazioni a INNER JOIN UPD_SettingPostazione b ON 1=1
  WHERE NomePostazione=@pNomePostazione




END
GO