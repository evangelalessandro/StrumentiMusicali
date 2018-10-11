using NLog;
using NLog.Targets;
using StrumentiMusicaliApp.Core.Events;
using StrumentiMusicaliSql.Core;
using StrumentiMusicaliSql.Entity;
using StrumentiMusicaliSql.Repo;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StrumentiMusicaliApp.Core
{
	public class Controller : IDisposable
	{
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		public Controller()
		{
			ConfigureNLog();

			EventAggregator.Instance().Subscribe<ArticoloAdd>(AggiungiArticolo);
			EventAggregator.Instance().Subscribe<ArticoloDelete>(DeleteArticolo);
			EventAggregator.Instance().Subscribe<ArticoloDuplicate>(DuplicaArticolo);
			EventAggregator.Instance().Subscribe<ArticoloSelected>(ArticoloSelectedChange);
			EventAggregator.Instance().Subscribe<ImportArticoli>(ImportaCsvArticoli);
			Application.ThreadException += Application_ThreadException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new fmrMain());
		}

		private void ImportaCsvArticoli(ImportArticoli obj)
		{
			try
			{
				using (OpenFileDialog res = new OpenFileDialog())
				{
					res.Title = "Seleziona file da importare";
					//Filter
					res.Filter = "File csv|*.csv;";

					//When the user select the file
					if (res.ShowDialog() == DialogResult.OK)
					{
						//Get the file's path
						var fileName = res.FileName;

						using (var curs = new CursorHandler())
						{

							using (StreamReader sr = new StreamReader(fileName, Encoding.Default, true))
							{
								String line;
								bool firstLine = true;
								int progress = 1;
								// Read and display lines from the file until the end of 
								// the file is reached.
								using (var uof = new UnitOfWork())
								{

									while ((line = sr.ReadLine()) != null)
									{
										if (!firstLine)
										{
											progress = ImportLine(line, progress, uof);
										}
										firstLine = false;
									}
									uof.Commit();
								}
							}
						}
						EventAggregator.Instance().Publish<ArticoliToUpdate>(new ArticoliToUpdate());
						MessageManager.NotificaInfo("Terminata importazione articoli");
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);

			}

		}

		private int ImportLine(string line, int progress, UnitOfWork uof)
		{
			var dat = line.Split('§');
			var cond = enCondizioneArticolo.Nuovo;
			if (dat[2] == "N")
			{
				cond = enCondizioneArticolo.Nuovo;
			}
			else if (dat[2] == "U")
			{
				cond = enCondizioneArticolo.UsatoGarantito;
			}
			else if (dat[2] == "E")
			{
				cond = enCondizioneArticolo.ExDemo;
			}
			else
			{
				throw new Exception("Tipo dato non gestito o mancante nella condizione articolo.");
			}
			decimal prezzo = 0;
			decimal prezzoBarrato = 0;
			bool prezzoARichiesta = false;
			var strPrezzo = dat[6];
			if (strPrezzo == "NC")
			{
				prezzoARichiesta = true;
			}
			else if (strPrezzo.Contains(";"))
			{
				prezzo = decimal.Parse(strPrezzo.Split(';')[0]);
				prezzoBarrato = decimal.Parse(strPrezzo.Split(';')[1]);
			}
			else
			{
				if (strPrezzo.Trim().Length > 0)
				{
					prezzo = decimal.Parse(strPrezzo);
				}
			}
			var artNew = (new StrumentiMusicaliSql.Entity.Articolo()
			{
				ID = (dat[0]),
				Categoria = int.Parse(dat[1]),
				Condizione = cond,
				Giacenza = 1,
				Marca = dat[3],
				Titolo = dat[4],
				Testo = dat[5].Replace("<br>", Environment.NewLine),
				Prezzo = prezzo,
				PrezzoARichiesta = prezzoARichiesta,
				PrezzoBarrato = prezzoBarrato,
				BoxProposte = int.Parse(dat[9]) == 1 ? true : false

			});
			if (artNew.ID == "")
			{
				progress++;
				artNew.ID = "Luc_" + DateTime.Now.Ticks.ToString() + progress.ToString();
			}
			uof.ArticoliRepository.Add(artNew);
			var foto = dat[7];
			if (foto.Length > 0)
			{
				foreach (var item in foto.Split(';'))
				{
					var artFoto = new StrumentiMusicaliSql.Entity.FotoArticolo()
					{
						Articolo = artNew,
						UrlFoto = item
					};
					uof.FotoArticoloRepository.Add(artFoto);
				}
			}

			return progress;
		}

		private void DeleteArticolo(ArticoloDelete obj)
		{
			try
			{
				using (var uof = new UnitOfWork())
				{

					var item = uof.ArticoliRepository.Find(a => a.ID == obj.ItemSelected.ID).FirstOrDefault();
					_logger.Info(string.Format("Cancellazione articolo /r/n{0} /r/n{1}", item.Titolo, item.ID));
					uof.ArticoliRepository.Delete(item);
					uof.Commit();
				}
				MessageManager.NotificaInfo("Cancellazione avvenuta correttamente!");
				EventAggregator.Instance().Publish<ArticoliToUpdate>(new ArticoliToUpdate());
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private ArticoloSelected _ArticoloSelected;
		private void ArticoloSelectedChange(ArticoloSelected obj)
		{
			_ArticoloSelected = obj;
		}

		public static void LogMethod(string level, string message, string exception, string stacktrace, string classLine)
		{
			try
			{
				using (var uof = new UnitOfWork())
				{
					uof.EventLogRepository.Add(new StrumentiMusicaliSql.Entity.EventLog()
					{ TipoEvento = level, Errore = message, TimeStamp = DateTime.Now, InnerException = exception, StackTrace = stacktrace, Class = classLine });
					uof.Commit();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(@"Errore nel salvataggio log\errore! " + Environment.NewLine + ex.Message, "info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
		private static void ConfigureNLog()
		{
			MethodCallTarget target = new MethodCallTarget();
			target.ClassName = typeof(Controller).AssemblyQualifiedName;
			target.MethodName = "LogMethod";
			target.Parameters.Add(new MethodCallParameter("${level}"));
			target.Parameters.Add(new MethodCallParameter("${message}"));
			target.Parameters.Add(new MethodCallParameter("${exception:format=tostring,Data:maxInnerExceptionLevel=10}"));
			target.Parameters.Add(new MethodCallParameter("${stacktrace}"));
			target.Parameters.Add(new MethodCallParameter("${callsite}"));


			NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
		}

		private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			_logger.Error("Application_ThreadException", e.Exception);
		}

		public void Dispose()
		{

		}

		private void AggiungiArticolo(ArticoloAdd articoloAdd)
		{
			_logger.Info("AggiungiArticolo");

			using (var frm = new Forms.frmArticolo())
			{
				frm.ShowDialog();
			}
		}

		private void DuplicaArticolo(ArticoloDuplicate obj)
		{
			try
			{
				using (var uof = new UnitOfWork())
				{
					var itemCurrent = _ArticoloSelected.ItemSelected.ArticoloCS;
					uof.ArticoliRepository.Add(new StrumentiMusicaliSql.Entity.Articolo()
					{
						Categoria = itemCurrent.Categoria,
						Condizione = itemCurrent.Condizione,
						BoxProposte = itemCurrent.BoxProposte,
						Giacenza = itemCurrent.Giacenza,
						Marca = itemCurrent.Marca,
						Prezzo = itemCurrent.Prezzo,
						PrezzoARichiesta = itemCurrent.PrezzoARichiesta,
						PrezzoBarrato = itemCurrent.PrezzoBarrato,
						Testo = itemCurrent.Testo,
						Titolo = itemCurrent.Titolo,
						UsaAnnuncioTurbo = itemCurrent.UsaAnnuncioTurbo,
						DataUltimaModifica = DateTime.Now
					});
					uof.Commit();
				}
				MessageManager.NotificaInfo("Duplicazione avvenuta co successo");
				_logger.Info("Duplica articolo");
				EventAggregator.Instance().Publish<ArticoliToUpdate>(new ArticoliToUpdate());


			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}


	}
}
