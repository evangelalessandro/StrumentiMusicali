using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.FatturaElett;
using StrumentiMusicali.App.Core.Controllers.Fatture;
using StrumentiMusicali.App.Core.Controllers.Stampa;
using StrumentiMusicali.App.Core.Events.Fatture;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
	public class ControllerFatturazione : BaseControllerGeneric<Fattura, FatturaItem>, IDisposable
	{

		public ControllerFatturazione() :
			base(enAmbienti.FattureList, enAmbienti.Fattura)
		{
			_sub1 = EventAggregator.Instance().Subscribe<ImportaFattureAccess>(ImportaFatture);
			_sub2 = EventAggregator.Instance().Subscribe<Add<Fattura>>(AddFattura);
			_sub3 = EventAggregator.Instance().Subscribe<Edit<Fattura>>(FatturaEdit);
			_sub4 = EventAggregator.Instance().Subscribe<Save<Fattura>>(Save);
			_sub5 = EventAggregator.Instance().Subscribe<Remove<Fattura>>(DelFattura);

			///comando di stampa
			AggiungiComandi();
		}

		private Subscription<Remove<Fattura>> _sub5;
		private Subscription<Save<Fattura>> _sub4;
		private Subscription<Edit<Fattura>> _sub3;
		private Subscription<Add<Fattura>> _sub2;
		private Subscription<ImportaFattureAccess> _sub1;
		public new void Dispose()
		{
			EventAggregator.Instance().UnSbscribe(_sub1);
			EventAggregator.Instance().UnSbscribe(_sub2);
			EventAggregator.Instance().UnSbscribe(_sub3);
			EventAggregator.Instance().UnSbscribe(_sub4);
			EventAggregator.Instance().UnSbscribe(_sub5);

			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Save(Save<Fattura> obj)
		{
			using (var saveManager = new SaveEntityManager())
			{
				if (string.IsNullOrEmpty(EditItem.Pagamento))
				{
					MessageManager.NotificaWarnig("Occorre impostare il pagamento");
					return;
				}

				var uof = saveManager.UnitOfWork;
				EditItem.Data = EditItem.Data.Date;

				if (EditItem.ID > 0)
				{
					EditItem = CalcolaTotali(EditItem);
					uof.FatturaRepository.Update(EditItem);
				}
				else
				{
					uof.FatturaRepository.Add(EditItem);
				}

				if (saveManager.SaveEntity(enSaveOperation.OpSave))
				{
					EventAggregator.Instance().Publish<UpdateList<Fattura>>(
							new UpdateList<Fattura>());
				}
			}
		}

		~ControllerFatturazione()
		{

		}

		private void DelFattura(Remove<Fattura> obj)
		{
			try
			{
				if (!MessageManager.QuestionMessage("Sei sicuro di voler cancellare la fattura selezionata?"))
					return;
				using (var saveEntity = new SaveEntityManager())
				{
					var uof = saveEntity.UnitOfWork;
					int val = ((Fattura)SelectedItem).ID;
					var item = uof.FatturaRepository.Find(a => a.ID == val).FirstOrDefault();
					_logger.Info(string.Format("Cancellazione fattura/r/n codice {0} /r/n Numero {1}",
						item.Codice, item.ID));

					foreach (var itemRiga in uof.FattureRigheRepository.Find(a => a.FatturaID == val))
					{
						uof.FattureRigheRepository.Delete(itemRiga);
					}

					uof.FatturaRepository.Delete(item);

					if (saveEntity.SaveEntity(enSaveOperation.OpDelete))
					{
						EventAggregator.Instance().Publish<UpdateList<Fattura>>(
							new UpdateList<Fattura>());
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		private void FatturaEdit(Edit<Fattura> edit)
		{
			EditItem = (Fattura)SelectedItem;
			ShowDettaglio();
		}

		private void ShowDettaglio()
		{
			using (var view = new DettaglioFatturaView(this))
			{
				ShowView(view, Settings.enAmbienti.Fattura);
			}
		}

		private void AddFattura(Add<Fattura> obj)
		{
			EditItem = new Fattura();
			ShowDettaglio();
		}


		private void ImportaFatture(ImportaFattureAccess obj)
		{
			try
			{
				using (OpenFileDialog res = new OpenFileDialog())
				{
					res.Title = "Seleziona file da importare";
					//Filter
					res.Filter = "File access|*.mdb|Tutti i file|*.*";

					res.Multiselect = false;
					//When the user select the file
					if (res.ShowDialog() == DialogResult.OK)
					{
						using (var dat = new CursorManager())
						{
							using (var importa = new ImportFatture())
							{
								importa.ImportAccessDB(res.FileName);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}

		internal void StampaFattura(Fattura fattura)
		{
			_logger.Info("Avvio stampa");
			try
			{
				using (var cursorManger = new CursorManager())
				{
					using (var stampaFattura = new StampaFattura())
					{
						var setting = ReadSetting().DatiIntestazione;
						if (setting == null)
						{
							setting = new DatiIntestazioneStampaFattura();
						}
						if (setting.Validate())
						{
							stampaFattura.Stampa(
										setting,
										fattura);
							_logger.Info("Stampa completata correttamente.");

						}

					}
				}

			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}
		}
		public static Fattura CalcolaTotali(Fattura fattura)
		{
			using (var uof = new UnitOfWork())
			{
				var fattItem = uof.FatturaRepository.Find(a => a.ID == fattura.ID).
							Select(a => new { a.RigheFattura, a }).First();

				List<FatturaRiga> righeFatt = fattItem.RigheFattura.ToList();
				decimal importoIvaTot = 0;
				decimal imponibileIVaTot = 0;

				foreach (var item in righeFatt.Where(a => a.Importo > 0 && a.IvaApplicata.Length > 0).GroupBy(a => a.IvaApplicata).OrderBy(a => a.Key))
				{
					var imponibileIva = item.Select(a => a.Importo).DefaultIfEmpty(0).Sum();
					decimal importoIva = 0;
					decimal val;
					if (decimal.TryParse(item.Key, out val))
					{
						importoIva = imponibileIva * ((decimal)val) / ((decimal)100);
					}
					importoIvaTot += importoIva;
					imponibileIVaTot += imponibileIva;
				}
				fattura.TotaleIva = (importoIvaTot);
				fattura.ImponibileIva = (imponibileIVaTot);
				fattura.TotaleFattura = (imponibileIVaTot + importoIvaTot);
				return fattura;
			}
		}
		private void ImpostaQuadroIVa()
		{

		}
		private void GeneraFatturaXml(Fattura selectedItem)
		{
			_logger.Info("Avvio stampa");
			try
			{
				using (var cursorManger = new CursorManager())
				{
					using (var stampa = new FattElettronica())
					{
						var setting = ReadSetting();
						stampa.DatiMittente = setting.datiMittente;

						using (var uof = new UnitOfWork())
						{

							var fatt = uof.FatturaRepository.Find(a => a.ID == selectedItem.ID)
								.Select(a => new { Fattura = a, a.Cliente, a.RigheFattura }).First();
							stampa.DatiDestinatario = fatt.Cliente;

							FatturaHeader header = new FatturaHeader();
							header.Righe =
								fatt.RigheFattura.ToList().Select(a =>

								new FatturaElett.FatturaRighe()
								{
									Descrizione = a.Descrizione,
									QTA = a.Qta,
									PrezzoUnitario = a.PrezzoUnitario,
									PrezzoTotale = a.Importo,
									AliquotaIVA = Iva(a.IvaApplicata)

								}
							).ToList();
							header.Numero = fatt.Fattura.Codice;
							if (fatt.Fattura.Pagamento.ToUpperInvariant().Contains("Bonifico".ToUpperInvariant()))
								header.ModalitaPagamento = enTipoPagamento.Bonifico;
							else if (fatt.Fattura.Pagamento.ToUpperInvariant().Contains("Contrassegno".ToUpperInvariant()))
								header.ModalitaPagamento = enTipoPagamento.Contrassegno;
							else if (fatt.Fattura.Pagamento.ToUpperInvariant().Contains("Rimessa".ToUpperInvariant()))
								header.ModalitaPagamento = enTipoPagamento.Contanti;
							else
							{
								throw new MessageException("Tipo pagamento non gestito");
							}
							header.Data = fatt.Fattura.Data;
							header.ImportoTotaleDocumento = fatt.Fattura.TotaleFattura;

							stampa.FattureList.Add(header);
							stampa.ScriviFattura(fatt.Fattura.ID);
						}
						//stampa.DatiDestinatario.
						////stampa.ScriviFattura()

						_logger.Info("Stampa completata correttamente.");
					}
				}

			}
			catch (Exception ex)
			{
				ExceptionManager.ManageError(ex);
			}

		}
		private int Iva(string val)
		{
			var valiva = 0;
			if (int.TryParse(val.Trim(), out valiva))
				return valiva;
			return 0;
		}
		private void AggiungiComandi()
		{

			AggiungiComandiStampa(GetMenu().Tabs[0], false);
		}

		public void AggiungiComandiStampa(MenuRibbon.RibbonMenuTab tab, bool editItem)
		{
			var pnlStampa = tab.Add("Stampa");

			var ribStampa = pnlStampa.Add("Avvia stampa", Properties.Resources.Print_48, true);
			ribStampa.Click += (a, e) =>
			{
				if (editItem)
					StampaFattura(EditItem);
				else
					StampaFattura(SelectedItem);

			};

			var ribStampaXml = pnlStampa.Add("Genera fattura xml", Properties.Resources.Fattura_xml_48, true);
			ribStampaXml.Click += (a, e) =>
			{

				if (editItem)
					GeneraFatturaXml(EditItem);
				else
					GeneraFatturaXml(SelectedItem);
			};

			var pnlCliente = tab.Add("Anagrafica cliente");
			var ribCust = pnlCliente.Add("Visualizza cliente", Properties.Resources.Customer_48, true);
			ribCust.Click += (x, e) =>
			{
				using (var controllerCl = new ControllerClienti())
				{
					using (var uof = new UnitOfWork())
					{

						var idFatt = 0;
						if (editItem)
							idFatt = (EditItem).ID;
						else
							idFatt = (SelectedItem).ID;

						var cliente = uof.FatturaRepository.Find(a => a.ID == idFatt).Select(a => a.Cliente).First();
						///impostato per la save.
						controllerCl.EditItem = cliente;

						//var frm = ViewFactory.GetView(enAmbienti.Cliente);
						//if (frm == null)
						{
							var view = new GenericSettingView(cliente);
							view.OnSave += (d, b) =>
							{
								view.Validate();
								EventAggregator.Instance().Publish<Save<Cliente>>
								(new Save<Cliente>());
							};
							ShowView(view, enAmbienti.Cliente,null,false);
							ViewFactory.AddView(enAmbienti.Cliente,view);
						}
						//else
						//{

						//	frm.OnSave += (d, b) =>
						//	{
						//		frm.Validate();
						//		EventAggregator.Instance().Publish<Save<Cliente>>
						//		(new Save<Cliente>());
						//	};
						//	frm.Rebind(cliente);
						//	ShowView(frm, enAmbienti.Cliente, null, false);
						//	ViewFactory.AddView(enAmbienti.Cliente, frm);
						//}

					}
				}

			};
		}

		public override void RefreshList(UpdateList<Fattura> obj)
		{

			try
			{
				var datoRicerca = TestoRicerca;
				var list = new List<FatturaItem>();

				using (var uof = new UnitOfWork())
				{
					list = uof.FatturaRepository.Find(a => datoRicerca == ""
					   || a.RagioneSociale.Contains(datoRicerca)
						|| a.PIVA.Contains(datoRicerca)
						|| a.Codice.Contains(datoRicerca)

					).Select(a => new FatturaItem
					{
						ID = a.ID,
						Data = a.Data,
						Entity = a,
						PIVA = a.PIVA,
						Codice = a.Codice,
						RagioneSociale = a.RagioneSociale
					}).OrderByDescending(a => a.ID).Take(100).ToList();
				}

				DataSource = new View.Utility.MySortableBindingList<FatturaItem>(list);
			}
			catch (Exception ex)
			{
				new Task(new Action(() =>
				{ ExceptionManager.ManageError(ex); })).Wait();

			}
		}
	}
}