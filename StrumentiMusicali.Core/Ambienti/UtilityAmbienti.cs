using StrumentiMusicali.Core.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Core.Ambienti
{
    public static class UtilityAmbienti
    {
        public static string TestoAmbiente(enAmbiente ambiente)
        {
            switch (ambiente)
            {
                case enAmbiente.ClientiList:
                    return @"Clienti\Fornitori";

                case enAmbiente.Cliente:
                    return @"Cliente\Fornitore";

                case enAmbiente.Main:
                    return "MainName";

                case enAmbiente.Fattura:
                    return "Fattura";

                case enAmbiente.FattureList:
                    return "Fatture";

                case enAmbiente.StrumentiDetail:
                    return "Strumento";

                case enAmbiente.StrumentiList:
                    return "Gestione Strumenti";

                case enAmbiente.LibriDetail:
                    return "Libro";

                case enAmbiente.LibriList:
                    return "Gestione Libri";

                case enAmbiente.Magazzino:
                    return "Magazzino";

                case enAmbiente.SettingFatture:
                    return "Impostazioni fatture";

                case enAmbiente.SettingSito:
                    return "Impostazioni sito";

                case enAmbiente.SettingDocPagamenti:
                    return "Impostazioni documenti pagamenti";

                case enAmbiente.SettingFtpBackup:
                    return "Impostazioni ftp backup";

                case enAmbiente.SettingScontrino:
                    return "Impostazioni scontrino";

                case enAmbiente.Scheduler:
                    return "Scheduler";

                case enAmbiente.ScaricoMagazzino:
                    return "Scarico Magazzino";

                case enAmbiente.LogViewList:
                    return "Visualizzatore dei log";

                case enAmbiente.SettingStampa:
                    return "Settaggi di stampa fattura";

                case enAmbiente.FattureRigheList:
                    break;
                case enAmbiente.FattureRigheDett:
                    return "Dettaglio riga";
                case enAmbiente.Deposito:
                    return "Deposito";
                case enAmbiente.DepositoList:
                    return "Depositi";
                case enAmbiente.ArticoloSconto:
                    return "Sconta articoli";
                case enAmbiente.UtentiList:
                    return "Utenti";
                case enAmbiente.Utente:
                    return "Utente";
                case enAmbiente.PagamentiList:
                    return "Pagamenti";
                case enAmbiente.Pagamento:
                    return "Pagamento";
                case enAmbiente.RicercaArticolo:
                    return "Ricerca articolo";

                default:
                    return "NIENTE DI IMPOSTATO";

            }
            return "NIENTE DI IMPOSTATO";

        }
        public static Icon GetIcon(enAmbiente ambiente)
        {
            switch (ambiente)
            {
                case enAmbiente.PagamentiList:
                case enAmbiente.Pagamento:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Payment);
                    break;
                case enAmbiente.ClientiList:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Customer_48);
                    break;
                case enAmbiente.UtentiList:
                case enAmbiente.Utente:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Utenti);
                    break;
                case enAmbiente.Cliente:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Customer_48);
                    break;
                case enAmbiente.Main:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.StrumentoMusicale);
                    break;
                case enAmbiente.RicercaArticolo:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Search_48);
                    break;
                case enAmbiente.Fattura:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Invoice);
                    break;
                case enAmbiente.FattureList:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Invoice);
                    break;
                case enAmbiente.StrumentiDetail:
                case enAmbiente.StrumentiList:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.StrumentoMusicale);
                    break;
                case enAmbiente.LibriDetail:
                case enAmbiente.LibriList:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Libro_48);
                    break;
                case enAmbiente.Magazzino:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.UnloadWareHouse);
                    break;
                case enAmbiente.SettingScontrino:
                case enAmbiente.SettingFatture:
                case enAmbiente.SettingFtpBackup:
                case enAmbiente.SettingDocPagamenti:
                case enAmbiente.SettingSito:
                case enAmbiente.SettingStampa:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Settings);
                    break;
                case enAmbiente.ScaricoMagazzino:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.UnloadWareHouse);
                    break;
                case enAmbiente.LogViewList:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.LogView_48);
                    break;
                case enAmbiente.FattureRigheList:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Invoice);
                    break;
                case enAmbiente.FattureRigheDett:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Invoice);
                    break;
                case enAmbiente.Deposito:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Depositi);
                    break;
                case enAmbiente.DepositoList:
                    return  UtilityViewGetIco.GetIco(Properties.ImageIcons.Depositi);
                    break;
                default:
                    break;
            }
            return null;
        }

    }
}
