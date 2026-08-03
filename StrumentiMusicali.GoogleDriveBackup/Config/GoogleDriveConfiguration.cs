using Newtonsoft.Json;
using StrumentiMusicali.Library.Core.Settings;
using StrumentiMusicali.Library.Entity.Setting;
using System;
using System.IO;

namespace StrumentiMusicali.GoogleDriveBackup.Config
{
	/// <summary>
	/// Legge la configurazione per il backup su Google Drive ESCLUSIVAMENTE dal database
	/// (entità SettingBackupFtp → BackupSetting), che è l'unica fonte.
	/// Il file JSON non viene più utilizzato per la lettura.
	/// </summary>
	public static class GoogleDriveConfiguration
	{
		public const string DefaultFileName = "GoogleDriveBackup.config.json";

		/// <summary>
		/// Legge e restituisce la configurazione per il backup su Google Drive
		/// leggendo tutti i campi dal database (funzione SettingsBackupFtp → BackupSetting).
		/// </summary>
		public static GoogleDriveBackupConfig ReadConfig(string configFilePath = null)
		{
			var config = new GoogleDriveBackupConfig();

			SettingBackupFtp ftpSetting = null;
			try
			{
				ftpSetting = SettingBackupFtpValidator.ReadSetting();
			}
			catch (Exception)
			{
				// Se il database non è raggiungibile, restituisce la configurazione con i valori di default.
				ftpSetting = null;
			}

			if (ftpSetting != null && ftpSetting.BackupSetting != null)
			{
				var bs = ftpSetting.BackupSetting;

				config.LocalBackupFolder = bs.FolderLocalServer;
				config.MaxMbInDrive = ftpSetting.MaxMbFileInFtp > 0 ? ftpSetting.MaxMbFileInFtp : 0;
				config.ServiceAccountCredentialFile = bs.ServiceAccountCredentialFile;
				config.DestinationFolderId = bs.DestinationFolderId;
				config.DeleteLocalAfterUpload = bs.DeleteLocalAfterUpload;
				config.OAuthClientId = bs.OAuthClientId;
				config.OAuthClientSecret = bs.OAuthClientSecret;
			}

			return config;
		}

		public static void WriteConfig(GoogleDriveBackupConfig config, string configFilePath = null)
		{
			var path = string.IsNullOrEmpty(configFilePath)
				? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultFileName)
				: configFilePath;

			var json = JsonConvert.SerializeObject(config, Formatting.Indented);
			File.WriteAllText(path, json);
		}
	}
}
