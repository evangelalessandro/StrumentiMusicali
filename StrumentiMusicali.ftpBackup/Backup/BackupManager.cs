using NLog;
using StrumentiMusicali.App.Core;
using StrumentiMusicali.Library.Entity.Setting;
using StrumentiMusicali.Library.Repo;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace StrumentiMusicali.ftpBackup.Backup
{
	public class BackupManager : IDisposable
	{

		internal readonly ILogger _logger = LogManager.GetCurrentClassLogger();
		private const string BACKUP_NAME = "Negozio.bak";

		/// <summary>
		/// Gestisce la copia verso l'ftp del backup che si deve trovare nella cartella specifica
		/// </summary>
		public bool Manage()
		{
			_logger.Warn("Inizio lettura parametri setting backup ");

			var setting = Library.Core.Settings.SettingBackupFtpValidator.ReadSetting();
			string message = "";
			try
			{
				/*legge il file e lo rinomina con data creazione Backup_yyyymmdd.bak */
				if (!System.IO.Directory.Exists(setting.BackupSetting.NetworkFolder))
				{
					message = "La cartella di backup di rete non è accessibile: " + setting.BackupSetting.NetworkFolder;
					_logger.Warn(message);
					if (Environment.UserInteractive)
					{
						MessageManager.NotificaWarnig(message);
					}
					return false;
				}
				/*se presente lo rinomina*/
				var file = Path.Combine(setting.BackupSetting.NetworkFolder, BACKUP_NAME);
				var fileNewFile = "";
				if (!System.IO.File.Exists(file))
				{

					message = "file non trovato : " + file;
					_logger.Warn(message);
					if (Environment.UserInteractive)
					{
						MessageManager.NotificaWarnig(message);
					}
					return false;
				}
				else
				{
					message = "file trovato : " + file;
					_logger.Warn(message);

					var data = new DateTime();
					var fileTest = new FileInfo(file);
					{
						data = fileTest.LastWriteTime;
					}
					fileNewFile = Path.Combine(setting.BackupSetting.NetworkFolder,
							"Backup_" + data.ToString("yyyyMMdd") + ".bak");
					if (System.IO.File.Exists(fileNewFile))
					{
						System.IO.File.Delete(fileNewFile);
					}

					System.IO.File.Move(file, Path.Combine(setting.BackupSetting.NetworkFolder, fileNewFile));

					message = "file spostato : " + Path.Combine(setting.BackupSetting.NetworkFolder, fileNewFile);
					_logger.Warn(message);

					if (Environment.UserInteractive)
					{
						MessageManager.NotificaInfo(message);
					}

					setting.UltimoBackup = DateTime.Now;
					using (var uof = new UnitOfWork())
					{
						uof.SettingBackupFtpRepository.Update(setting);
						uof.Commit();
					}

					/*cancella i file di backup sql locali più vecchi di 7 giorni*/
					ClearOldLocalFiles(setting);

					var fileZip = CreaZip(fileNewFile);

					if (FtpSend(fileZip))
					{
						System.IO.File.Delete(fileNewFile);
						ClearOldFile(setting);
						return true;
					}
				}

			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Errore nella gestione dei backup");

				ClearOldFile(setting);
				ClearOldLocalFiles(setting);
			}
			return false;
		}

		public bool FtpSend(string fileNewFile)
		{
			/*se presente, copia il file e lo invia all'ftp e lo cancella in locale*/
			using (var ftp = new Ftp.FtpManager())
			{
				if (ftp.ConnessioneOk())
				{
					if (ftp.Upload(fileNewFile))
					{
						_logger.Info("Upload riuscito del backup : " + fileNewFile);
						System.IO.File.Delete(fileNewFile);
						return true;
					}
					else
					{
						_logger.Error("Upload non riuscito del backup : " + fileNewFile);
					}
				}
			}
			return false;
		}

		private string CreaZip(string fileBak)
		{
			var fileZip = Path.ChangeExtension(fileBak, ".zup");
			if (System.IO.File.Exists(fileZip))
				System.IO.File.Delete(fileZip);

			using (var zipStream = new FileStream(fileZip, FileMode.Create))
			using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create))
			{
				var entry = zip.CreateEntry(Path.GetFileName(fileBak));
				using (var entryStream = entry.Open())
				using (var fileStream = new FileStream(fileBak, FileMode.Open, FileAccess.Read))
				{
					fileStream.CopyTo(entryStream);
				}
			}

			_logger.Info("Creato archivio zip del backup: " + fileZip);
			return fileZip;
		}

		/// <summary>
		/// se ci sono più di x file cancella quelli in eccedenza
		/// </summary>
		private void ClearOldFile(SettingBackupFtp setting)
		{
			if (setting.MaxMbFileInFtp < 30)
			{
				_logger.Warn("Occorre impostare una qta maggiore di mb per il backup, " + setting.MaxMbFileInFtp.ToString() + " sono troppo pochi");
				return;
			}
			using (var ftp = new Ftp.FtpManager())
			{
				if (ftp.ConnessioneOk())
				{
					while (true)
					{
						var listFile = ftp.FileList();
						if (listFile == null)
						{
							_logger.Info("Non si riesce a prelevare la lista dei backup ");
							return;
						}
						var backList = ftp.FileList().Where(a => a.IsDirectory == false && a.Name.Contains("Backup_")
							  && a.Name.EndsWith(".zip")).ToList();
						var size = ConvertBytesToMegabytes(backList.Sum(a => a.Size));

						if (size > setting.MaxMbFileInFtp && backList.Count > 1)
						{
							var first = listFile.OrderBy(a => a.Name).First();
							ftp.Delete(first.FullPath);

							_logger.Info("Cancello file di backup vecchio : " + first.Name);

							setting.UltimaCancellazioneBackup = DateTime.Now;
							using (var uof = new UnitOfWork())
							{
								uof.SettingBackupFtpRepository.Update(setting);
								uof.Commit();
							}
						}
						else
						{
							return;
						}
					}
				}
			}
		}

		/// <summary>
		/// Cancella i file di backup .bak locali (cartella FolderLocalServer) più vecchi di 7 giorni
		/// </summary>
		private void ClearOldLocalFiles(SettingBackupFtp setting)
		{
			var folder = setting.BackupSetting.FolderLocalServer;
			if (string.IsNullOrWhiteSpace(folder) || !System.IO.Directory.Exists(folder))
			{
				_logger.Warn("Cartella di backup locale non trovata (FolderLocalServer): " + folder);
				return;
			}

			var scadenza = DateTime.Now.AddDays(-7);
			foreach (var file in System.IO.Directory.GetFiles(folder, "*.bak"))
			{
				try
				{
					var info = new FileInfo(file);
					if (info.LastWriteTime < scadenza)
					{
						System.IO.File.Delete(file);
						_logger.Info("Cancellato file di backup locale vecchio : " + file);
					}
				}
				catch (Exception ex)
				{
					_logger.Error(ex, "Errore cancellazione file di backup locale : " + file);
				}
			}
		}

		private static double ConvertBytesToMegabytes(long bytes)
		{
			return (bytes / 1024f) / 1024f;
		}
		public void Dispose(bool dispose)
		{

		}
		public void Dispose()
		{
		}
	}
}