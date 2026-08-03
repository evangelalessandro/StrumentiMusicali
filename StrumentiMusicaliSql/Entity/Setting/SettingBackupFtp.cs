using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Setting
{
    public class SettingBackupFtp : Base.BaseEntity
    {
        [CustomUIView(Ordine = 10)]
        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(50)]
        [CustomUIView(Ordine = 11)]
        public string Password { get; set; }
        [MaxLength(100)]
        [CustomUIView(Ordine = 15)]
        public string UrlHost { get; set; }
        [CustomUIView(Ordine = 20, Titolo = "Porta", Width = 100)]
        public int Porta { get; set; } = 21;

        [CustomUIView(Ordine = 20)]
        public string BaseFolder { get; set; } = "/Backup";

        [CustomUIView(Ordine = 21, Titolo = "Numero massimo Megabyte in ftp")]
        public int MaxMbFileInFtp { get; set; } = 200;
        public BackupSetting BackupSetting { get; set; } = new BackupSetting();
        [CustomUIView(DateTimeView = true, Enable = false, Ordine = 40)]
        public DateTime UltimoBackup { get; set; } = new DateTime(1900, 1, 1);
        [CustomUIView(DateTimeView = true, Enable = false, Ordine = 41)]
        public DateTime UltimaCancellazioneBackup { get; set; } = new DateTime(1900, 1, 1);

        [CustomUIView(Ordine = 50, Titolo = "Controlla se non è stato effettuato il backup dopo x giorni (0 = mai)")]
        public int ControllaEsecuzioneBackup { get; set; } = 0;


    }

    public class BackupSetting
    {
        [MaxLength(200)]
        public string FolderLocalServer { get; set; } = "";

        [MaxLength(200)]
        public string NetworkFolder { get; set; } = "";

        /// <summary>
        /// Percorso completo del file di credenziali del Service Account
        /// (file .json generato da Google Cloud Console).
        /// </summary>
        [MaxLength(500)]
        public string ServiceAccountCredentialFile { get; set; } = "";

        /// <summary>
        /// Id (o nome) della cartella di destinazione su Google Drive.
        /// Se vuoto viene usata la root.
        /// </summary>
        [MaxLength(300)]
        public string DestinationFolderId { get; set; } = "";

        /// <summary>
        /// Se true, elimina il file locale dopo un upload riuscito.
        /// </summary>
        public bool DeleteLocalAfterUpload { get; set; } = true;

        /// <summary>
        /// Client ID dell'app OAuth (OAuth 2.0 Client) creato su Google Cloud Console.
        /// Usato per il login utente Google quando il Service Account non è configurato.
        /// </summary>
        [MaxLength(300)]
        public string OAuthClientId { get; set; } = "";

        /// <summary>
        /// Client Secret dell'app OAuth (OAuth 2.0 Client) creato su Google Cloud Console.
        /// </summary>
        [MaxLength(300)]
        public string OAuthClientSecret { get; set; } = "";
    }
}