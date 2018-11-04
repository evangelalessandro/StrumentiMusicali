using NLog;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;

namespace StrumentiMusicali.App.Core.Controllers.Fatture
{
	public class ImportFatture : IDisposable
	{
		internal readonly ILogger _logger = LogManager.GetCurrentClassLogger();

		public void ImportAccessDB(string file)
		{
			try
			{
				//;Password=****
				using (var connection = new OleDbConnection(string.Format("Provider=Microsoft.JET.OLEDB.4.0;" + "data source={0}", file)))
				{
					connection.Open();

					using (var uof = new UnitOfWork())
					{
						var clientiList = ImportClienti(connection);

						uof.ClientiRepository.AddRange(clientiList);
						uof.Commit();

						MessageManager.NotificaInfo("Importazione clienti!");
						var fattureList = ImportTestateFatture(connection, clientiList);

						foreach (var item in fattureList)
						{
							uof.FatturaRepository.Add(item);
							uof.Commit();
						}



						var righeFatturaList = ImportRigheFatture(connection, fattureList);
						OrdinaRighe(righeFatturaList);

						uof.FattureRigheRepository.AddRange(righeFatturaList.ToList());
						uof.Commit();

						MessageManager.NotificaInfo("Importazione fatture!");
						var ddtList = ImportDDT(connection, clientiList);

						uof.DDTRepository.AddRange(ddtList);
						uof.Commit();

						var ddtrigheList = ImportRigheDDT(connection, ddtList);
						OrdinaRighe(ddtrigheList);
						uof.DDTRigheRepository.AddRange(ddtrigheList);
						uof.Commit();

						MessageManager.NotificaInfo("Importazione completata correttamente!");
					}
				}
			}
			catch (Exception ex)
			{
				MessageManager.NotificaError("Si è verificato un errore nell'importazione", ex);
			}
		}

		private static void OrdinaRighe(List<DDTRiga> righeFatturaList)
		{
			var list = righeFatturaList.Select(a => new { a, a.DDTID }).GroupBy(a => a.DDTID).ToList();
			foreach (var itemGr in list)
			{
				int ordine = 0;
				foreach (var item in itemGr.OrderBy(a => a.a.OrdineVisualizzazione).ToList())
				{
					item.a.OrdineVisualizzazione = ordine;
					ordine++;
				}
			}
		}

		private static void OrdinaRighe(List<FatturaRiga> righeFatturaList)
		{
			var list = righeFatturaList.Select(a => new { a, a.FatturaID }).GroupBy(a => a.FatturaID).ToList();
			foreach (var itemGr in list)
			{
				int ordine = 0;
				foreach (var item in itemGr.OrderBy(a => a.a.OrdineVisualizzazione).ToList())
				{
					item.a.OrdineVisualizzazione = ordine;
					ordine++;
				}
			}
		}

		private List<DDTRiga> ImportRigheDDT(OleDbConnection conection, List<DDt> ddtList)
		{
			var query = "Select * From [prodotti DDT]";
			var listaDDTRighe = new List<DDTRiga>();
			foreach (var a in GetDati(conection, query).AsEnumerable())
			{
				try
				{
					var riga = new DDTRiga
					{
						CodiceArticoloOld = (a["Cod_Art"].ToString()),

						Descrizione = (a["Descrizione"].ToString()),
					};
					var dato = a["Q_tà"].ToString();
					if (dato.Length > 0)
					{
						riga.Qta = int.Parse(dato);
					}
					var fattura = ddtList.Where(b => b.Codice == (a["#documento"].ToString())).FirstOrDefault();

					if (fattura == null)
					{
						continue;
					}
					riga.DDTID = fattura.ID;
					listaDDTRighe.Add(riga);
				}
				catch (Exception ex)
				{
					MessageManager.NotificaError("Si è verificato un errore nell'importazione righe ddt", ex);
					throw ex;
				}
			}

			return listaDDTRighe;
		}

		private List<FatturaRiga> ImportRigheFatture(OleDbConnection conection, List<Fattura> listaFatture)
		{
			var query = "Select * From [prodotti fatture]";
			var listaFattureRighe = new List<FatturaRiga>();
			foreach (var a in GetDati(conection, query).AsEnumerable())
			{
				try
				{
					var riga = new FatturaRiga
					{
						CodiceArticoloOld = (a["Cod_Art"].ToString()),

						Descrizione = (a["Descrizione"].ToString()),

						IvaApplicata = (a["IVA %"].ToString()),
					};
					var dato = a["Q_tà"].ToString();
					if (dato.Length > 0)
					{
						riga.Qta = int.Parse(dato);
					}
					dato = a["Prezzo Cad"].ToString();
					if (dato.Length > 0)
					{
						riga.PrezzoUnitario = decimal.Parse(dato);
					}

					var fattura = listaFatture.Where(b => b.Codice == (a["#documento"].ToString())).FirstOrDefault();

					if (fattura == null)
					{
						continue;
					}
					riga.FatturaID = fattura.ID;

					listaFattureRighe.Add(riga);
				}
				catch (Exception ex)
				{
					MessageManager.NotificaError("Si è verificato un errore nell'importazione fatture", ex);
					throw ex;
				}
			}

			return listaFattureRighe;
		}

		private List<DDt> ImportDDT(OleDbConnection conection, List<Cliente> listaClienti)
		{
			var query = "Select * From DDT";
			var ddtList = new List<DDt>();
			foreach (var a in GetDati(conection, query).AsEnumerable())
			{
				try
				{
					var fattura = new DDt()
					{
						Codice = (a["# documento"].ToString()),
						AspettoEsterno = (a["Aspetto esteriore beni"].ToString()),
						CausaleTrasporto = (a["Casuale Trasporto"].ToString()),
						//ImportoTotale = decimal.Parse((a["Ora Trasporto"].ToString())),
						Note1 = (a["Note1:"].ToString()),
						Note2 = (a["Note2:"].ToString()),

						Porto = (a["Porto"].ToString()),
						TrasportoACura = (a["Trasporto a cura"].ToString()),
						Vettore = (a["Vettore"].ToString()),
					};

					var dato = a["Data Trasporto"].ToString();
					if (dato.Length > 0)
					{
						fattura.DataTrasporto = DateTime.Parse(dato);
					}

					dato = a["Ora Trasporto"].ToString();
					if (dato.Length > 0)
					{
						fattura.OraTrasporto = DateTime.Parse(dato);
					}
					dato = a["N_colli"].ToString();
					if (dato.Length > 0)
					{
						fattura.NumeroColli = int.Parse(dato);
					}
					dato = a["Data"].ToString();
					if (dato.Length > 0)
					{
						fattura.Data = DateTime.Parse(dato);
					}
					dato = a["Peso"].ToString();
					if (dato.Length > 0)
					{
						fattura.PesoKg = int.Parse(dato);
					}
					var ragioneSociale = a["Ragione sociale"].ToString();
					var cliente = listaClienti.Where(b => b.RagioneSociale == (ragioneSociale)).FirstOrDefault();
					if (cliente != null)
					{
						fattura.ClienteID = cliente.ID;
						fattura.RagioneSociale = cliente.RagioneSociale;
						fattura.PIVA = cliente.PIVA;
					}
					else
					{
						var message = ("Manca cliente : " + ragioneSociale + " Data: " + fattura.Codice.ToString());
						Debug.WriteLine(message);
						_logger.Warn(message);
						continue;
					}

					ddtList.Add(fattura);
				}
				catch (Exception ex)
				{
					MessageManager.NotificaError("Si è verificato un errore nell'importazione fatture", ex);
					throw ex;
				}
			}
			return ddtList;
		}

		private List<Fattura> ImportTestateFatture(OleDbConnection conection, List<Cliente> listaClienti)
		{
			var query = "Select * From fatture";
			var fattureList = new List<Fattura>();
			foreach (var a in GetDati(conection, query).AsEnumerable())
			{
				try
				{
					var fattura = new Fattura()
					{
						Codice = (a["# documento"].ToString()),
						AspettoEsterno = (a["Aspetto esteriore beni"].ToString()),
						CausaleTrasporto = (a["Casuale Trasporto"].ToString()),
						//ImportoTotale = decimal.Parse((a["Ora Trasporto"].ToString())),
						Note1 = (a["Note1:"].ToString()),
						Note2 = (a["Note2:"].ToString()),
						Pagamento = a["Pagamento"].ToString(),

						Porto = (a["Porto"].ToString()),
						TrasportoACura = (a["Trasporto a cura"].ToString()),
						Vettore = (a["Vettore"].ToString()),
					};
					if (string.IsNullOrEmpty(fattura.Pagamento))
					{
						fattura.Pagamento = "Rimessa Diretta";
					}
					var dato = a["Data Trasporto"].ToString();
					if (dato.Length > 0)
					{
						fattura.DataTrasporto = DateTime.Parse(dato);
					}

					dato = a["Ora Trasporto"].ToString();
					if (dato.Length > 0)
					{
						fattura.OraTrasporto = DateTime.Parse(dato);
					}
					dato = a["N_colli"].ToString();
					if (dato.Length > 0)
					{
						fattura.NumeroColli = int.Parse(dato);
					}
					dato = a["Data"].ToString();
					if (dato.Length > 0)
					{
						fattura.Data = DateTime.Parse(dato);
					}
					dato = a["Peso"].ToString();
					if (dato.Length > 0)
					{
						fattura.PesoKg = int.Parse(dato);
					}
					var ragioneSociale = a["Ragione sociale"].ToString();
					var cliente = listaClienti.Where(b => b.RagioneSociale == (ragioneSociale)).FirstOrDefault();
					if (cliente != null)
					{
						fattura.ClienteID = cliente.ID;
						fattura.RagioneSociale = cliente.RagioneSociale;
						fattura.PIVA = cliente.PIVA;
					}
					else
					{
						var message = ("Manca cliente : " + ragioneSociale + " Data: " + fattura.Codice.ToString());
						Debug.WriteLine(message);
						_logger.Warn(message);
						continue;
					}

					fattureList.Add(fattura);
				}
				catch (Exception ex)
				{
					MessageManager.NotificaError("Si è verificato un errore nell'importazione fatture", ex);
					throw ex;
				}
			}
			return fattureList;
		}

		private List<Cliente> ImportClienti(OleDbConnection conection)
		{
			var query = "Select * From anagrafica";

			var tabella = GetDati(conection, query);
			var listaClienti = new List<Cliente>();
			foreach (var a in tabella.AsEnumerable())
			{
				try
				{
					var cliente = new Cliente
					{
						CodiceClienteOld = int.Parse(a["cod_cliente"].ToString()),
						Cellulare = (a["Cellulare"].ToString()),
						Citta = (a["Città"].ToString()),
						Fax = (a["Fax"].ToString()),
						LuogoDestinazione = (a["Luogo destinazione"].ToString()),
						NomeBanca = (a["Banca Nome"].ToString()),
						PIVA = (a["P_IVA -  Cod_Fisc"].ToString()),
						RagioneSociale = (a["Ragione sociale - Nome Cognome"].ToString()),
						Telefono = (a["Telefono"].ToString()),
						Via = (a["Via"].ToString()),
					};
					var datoR = a["Banca ABI"].ToString();
					if (datoR != "")
						cliente.BancaAbi = int.Parse(datoR);
					datoR = a["Banca CAB"].ToString();
					if (datoR != "")
						cliente.BancaCab = int.Parse(datoR);
					datoR = a["CAP"].ToString();
					if (datoR != "")
						cliente.Cap = int.Parse(datoR);

					listaClienti.Add(
						 cliente
						);
				}
				catch (Exception ex)
				{
					MessageManager.NotificaError("Errore nell'import clienti", ex);
					throw ex;
				}
			}

			return listaClienti;
		}

		private DataTable GetDati(OleDbConnection conection, string query)
		{
			using (var adapter = new OleDbDataAdapter(query, conection))
			{
				var myDataTable = new DataTable();

				adapter.Fill(myDataTable);
				return myDataTable;
			}
		}

		public void Dispose()
		{
		}
	}
}