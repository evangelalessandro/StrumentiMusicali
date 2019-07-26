using NLog;
using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;

namespace StrumentiMusicali.App.Core.Fatture
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
                    try
                    {

                        ProgressManager.Instance().Visible = true;
                        using (var uof = new UnitOfWork())
                        {

                            ProgressManager.Instance().Value = 0;
                            var clientiList = ImportClienti(connection, uof);
                            var fattureList = ImportFattureTestate(connection, uof, clientiList);

                            ImportFattureRighe(connection, uof, fattureList);

                            AggiornaTotaliFatture(uof, fattureList);

                            var ddtList = ImportaDdtTestate(connection, uof, clientiList);

                            ImportDDTRighe(connection, uof, ddtList);

                            MessageManager.NotificaInfo("Importazione completata correttamente!");
                        }
                    }
                    finally
                    {
                        ProgressManager.Instance().Messaggio = "";
                        ProgressManager.Instance().Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageManager.NotificaError("Si è verificato un errore nell'importazione", ex);
            }
        }

        private void ImportDDTRighe(OleDbConnection connection, UnitOfWork uof, List<Fattura> ddtList)
        {
            var ddtrigheList = ImportRigheDDT(connection, ddtList);
            ProgressManager.Instance().Messaggio = "Inizio Ddt righe";

            OrdinaRighe(ddtrigheList);
            ProgressManager.Instance().Value = 0;
            ProgressManager.Instance().Max = ddtrigheList.Count;
            foreach (var item in ddtrigheList)
            {
                uof.FattureRigheRepository.Add(item);
                ProgressManager.Instance().Value++;
            }
            ProgressManager.Instance().Messaggio = "Ddt righe finiti";

            uof.Commit();
        }

        private List<Fattura> ImportaDdtTestate(OleDbConnection connection, UnitOfWork uof, List<Cliente> clientiList)
        {
            ProgressManager.Instance().Value = 0;
            var ddtList = ImportDDT(connection, clientiList);
            ProgressManager.Instance().Value = 0;

            ProgressManager.Instance().Max = ddtList.Count;
            ProgressManager.Instance().Messaggio = "DDT";

            foreach (var item in ddtList)
            {
                uof.FatturaRepository.Add(item);
                ProgressManager.Instance().Value++;
            }
            ProgressManager.Instance().Messaggio = "Ddt finiti";

            uof.Commit();
            return ddtList;
        }

        private static void AggiornaTotaliFatture(UnitOfWork uof, List<Fattura> fattureList)
        {
            ProgressManager.Instance().Messaggio = "Aggiorna totali fatture";
            ProgressManager.Instance().Value = 0;
            ProgressManager.Instance().Max = fattureList.Count();

            foreach (var item in fattureList)
            {
                try
                {


                    uof.FatturaRepository.Update(
                    ControllerFatturazione.CalcolaTotali(item)
                    );
                    ProgressManager.Instance().Value++;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            uof.Commit();
            ProgressManager.Instance().Messaggio = "Fine agg. totali fatture";
        }

        private void ImportFattureRighe(OleDbConnection connection, UnitOfWork uof, List<Fattura> fattureList)
        {
            var righeFatturaList = ImportRigheFatture(connection, fattureList);
            OrdinaRighe(righeFatturaList);

            ProgressManager.Instance().Value = 0;
            ProgressManager.Instance().Max = righeFatturaList.Count();
            ProgressManager.Instance().Messaggio = "Fatture righe";

            foreach (var item in righeFatturaList)
            {
                uof.FattureRigheRepository.Add(item);
                ProgressManager.Instance().Value++;
            }

            ProgressManager.Instance().Messaggio = "Fatture righe finite";
            uof.Commit();
        }

        private List<Fattura> ImportFattureTestate(OleDbConnection connection, UnitOfWork uof, List<Cliente> clientiList)
        {
            var fattureList = ImportTestateFatture(connection, clientiList);

            ProgressManager.Instance().Messaggio = "Inizio fatture";
            ProgressManager.Instance().Value = 0;
            ProgressManager.Instance().Max = fattureList.Count();


            foreach (var item in fattureList)
            {
                uof.FatturaRepository.Add(item);
                ProgressManager.Instance().Value++;
            }
            ProgressManager.Instance().Messaggio = "Fatture finite";
            uof.Commit();
            return fattureList;
        }

        private List<Cliente> ImportClienti(OleDbConnection connection, UnitOfWork uof)
        {
            var clientiList = ImportClienti(connection);
            ProgressManager.Instance().Max = clientiList.Count();
            ProgressManager.Instance().Messaggio = ("Inizio clienti");

            foreach (var item in clientiList)
            {
                uof.ClientiRepository.Add(item);
                ProgressManager.Instance().Value++;

            }
            uof.Commit();

            ProgressManager.Instance().Messaggio = ("clienti finiti");
            return clientiList;
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

        private List<FatturaRiga> ImportRigheDDT(OleDbConnection conection, List<Fattura> ddtList)
        {
            var query = "Select * From [prodotti DDT]";
            var listaDDTRighe = new List<FatturaRiga>();
            foreach (var a in GetDati(conection, query).AsEnumerable())
            {
                try
                {
                    var riga = new FatturaRiga
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
                    riga.FatturaID = fattura.ID;
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

        private List<Fattura> ImportDDT(OleDbConnection conection, List<Cliente> listaClienti)
        {
            var query = "Select * From DDT";
            var ddtList = new List<Fattura>();
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

                        Porto = (a["Porto"].ToString()),
                        TrasportoACura = (a["Trasporto a cura"].ToString()),
                        Vettore = (a["Vettore"].ToString()),
                    };
                    fattura.TipoDocumento = EnTipoDocumento.DDT;
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
            var query = "Select * From fatture where [# documento] like '%18' or  [# documento] like '%17'";
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
                    fattura.TipoDocumento = EnTipoDocumento.FatturaDiCortesia;
                    if (fattura.Codice.StartsWith("N"))
                    {
                        fattura.TipoDocumento = EnTipoDocumento.NotaDiCredito;
                    }
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
                        Indirizzo = new Indirizzo()
                        {
                            Citta = (a["Città"].ToString()),
                            IndirizzoConCivico = (a["Via"].ToString()),
                        },

                        Fax = (a["Fax"].ToString()),
                        LuogoDestinazione = (a["Luogo destinazione"].ToString()),
                        NomeBanca = (a["Banca Nome"].ToString()),
                        PIVA = (a["P_IVA -  Cod_Fisc"].ToString()),
                        RagioneSociale = (a["Ragione sociale - Nome Cognome"].ToString()),
                        Telefono = (a["Telefono"].ToString()),

                    };
                    if (cliente.PIVA.Length == 16 && !cliente.PIVA.Contains("."))
                    {
                        cliente.CodiceFiscale = cliente.PIVA;
                    }
                    else if (cliente.PIVA.Length > 16 && cliente.PIVA.Contains("/"))
                    {
                        var split = cliente.PIVA.Split('/');
                        if (split[0].Length == 11 && split[1].Length == 16)
                        {
                            cliente.CodiceFiscale = split[1];
                            cliente.PIVA = split[0];
                        }
                    }
                    var datoR = a["Banca ABI"].ToString();
                    if (datoR != "")
                        cliente.BancaAbi = int.Parse(datoR);
                    datoR = a["Banca CAB"].ToString();
                    if (datoR != "")
                        cliente.BancaCab = int.Parse(datoR);
                    datoR = a["CAP"].ToString();
                    if (datoR != "")
                    {
                        try
                        {
                            if (datoR != "0")
                                datoR = int.Parse(datoR).ToString("00000");
                        }
                        catch
                        {
                        }

                        cliente.Indirizzo.Cap = datoR;
                    }


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