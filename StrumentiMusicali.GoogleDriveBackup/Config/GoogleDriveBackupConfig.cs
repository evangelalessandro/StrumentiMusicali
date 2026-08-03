namespace StrumentiMusicali.GoogleDriveBackup.Config
{
  /// <summary>
  /// Configurazione per l'upload del backup su Google Drive.
  /// Viene letta da un file JSON esterno (vedi GoogleDriveConfiguration).
  /// </summary>
  public class GoogleDriveBackupConfig
  {


	/// <summary>
	/// Id (o nome) della cartella di destinazione su Google Drive. Se vuoto viene usata la root.
	/// </summary>
	public string DestinationFolderId { get; set; } = "";

	/// <summary>
	/// Cartella locale in cui è presente il file di backup rinominato (Backup_yyyyMMdd.bak).
	/// </summary>
	public string LocalBackupFolder { get; set; } = "";

	/// <summary>
	/// Numero massimo di Megabyte complessivi di backup da mantenere su Drive (0 = nessun limite).
	/// </summary>
	public long MaxMbInDrive { get; set; } = 200;

	/// <summary>
	/// Se true, elimina il file locale dopo un upload riuscito.
	/// </summary>
	public bool DeleteLocalAfterUpload { get; set; } = true;

	/// <summary>
	/// Client ID dell'app OAuth (OAuth 2.0 Client) creato su Google Cloud Console.
	/// Usato per il login utente Google quando il file Service Account non è configurato.
	/// </summary>
	public string OAuthClientId { get; set; } = "";

	/// <summary>
	/// Client Secret dell'app OAuth (OAuth 2.0 Client) creato su Google Cloud Console.
	/// </summary>
	public string OAuthClientSecret { get; set; } = "";

	/// <summary>
	/// Cartella in cui salvare/leggere il file con i token OAuth (access/refresh) dopo il login.
	/// Se vuota, usa una cartella di default nella directory dell'applicazione.
	/// </summary>
	public string OAuthTokenFolder { get; set; } = "";
  }
}
