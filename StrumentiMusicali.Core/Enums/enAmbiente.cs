﻿using StrumentiMusicali.Core.Attributes;

namespace StrumentiMusicali.Core.Enum
{
    public enum enAmbiente
    {
        [UIAmbienteAttribute(true)]
        Main = 1,
        [UIAmbienteAttribute(true)]
        Fattura = 2,
        [UIAmbienteAttribute(false)]
        FattureList = 3,
        [UIAmbienteAttribute(true)]
        StrumentiDetail = 4,
        [UIAmbienteAttribute(true)]
        ArticoloSconto = 700,
        [UIAmbienteAttribute(false, AmbienteMutuale = enAmbiente.RicercaArticolo)]
        StrumentiList = 5,

        [UIAmbienteAttribute(false, AmbienteMutuale = enAmbiente.RicercaArticolo)]
        LibriList = 50,
        [UIAmbienteAttribute(true)]
        LibriDetail = 51,

        Magazzino = 6,
        [UIAmbienteAttribute(false)]
        SettingFatture = 7,
        [UIAmbienteAttribute(true)]
        SettingSito = 8,
        [UIAmbienteAttribute(true)]
        SettingDocPagamenti = 30,
        [UIAmbienteAttribute(false)]
        SettingStampa = 9,
        [UIAmbienteAttribute(false)]
        ScaricoMagazzino = 10,
        //[UIAmbienteAttribute(false)]
        //LogView=11,
        [UIAmbienteAttribute(false)]
        LogViewList = 12,
        [UIAmbienteAttribute(false)]
        ClientiList = 13,
        [UIAmbienteAttribute(true)]
        Cliente = 14,

        ListinoPrezziFornitoreList = 60,
        [UIAmbienteAttribute(true,NomeAmbiente ="Listino prezzi fornitore" )]
        ListinoPrezziFornitoreDett = 61,

        FattureRigheList = 15,
        [UIAmbienteAttribute(true)]
        FattureRigheDett = 16,
        [UIAmbienteAttribute(true)]
        Deposito = 17,
        [UIAmbienteAttribute(false)]
        DepositoList = 18,


        [UIAmbienteAttribute(true)]
        Utente = 20,
        [UIAmbienteAttribute(false)]
        UtentiList = 21,

        [UIAmbienteAttribute(false)]
        SettingFtpBackup = 40,



        [UIAmbienteAttribute(false, NomeAmbiente = "Articoli sottoscorta")]
        ArticoliSottoscorta,

        [UIAmbienteAttribute(false)]
        SettingScontrino = 125,

        NonSpecificato = 100,
        [UIAmbienteAttribute(false
            )]
        PagamentiList = 701,
        [UIAmbienteAttribute(true)]
        Pagamento = 702,

        [UIAmbienteAttribute(true)]
        PagamentoDocumenti = 710,
        [UIAmbienteAttribute(false, AmbienteMutuale = enAmbiente.StrumentiList)]
        RicercaArticolo = 703,

        [UIAmbienteAttribute(false)]
        Scheduler = 130,
        [UIAmbienteAttribute(true)]
        SchedulerDetail = 131,

        [UIAmbienteAttribute(false, NomeAmbiente = "Tipo pagamenti documenti")]
        TipiPagamentiList,
        [UIAmbienteAttribute(true, NomeAmbiente = "Tipo pagamento documento")]
        TipiPagamenti,

        [UIAmbienteAttribute(false, NomeAmbiente = "Tipi pagamenti scontrino")]
        TipiPagamentiScontrinoList,
        [UIAmbienteAttribute(true, NomeAmbiente = "Tipo pagamento scontrino")]
        TipiPagamentiScontrino,

        [UIAmbienteAttribute( false, NomeAmbiente = "Nomi listini clienti")]
        NomeListiniClientiList,
        [UIAmbienteAttribute(viewInForm: true, NomeAmbiente = "Nome listino clienti")]
        NomeListiniClienti,
        [UIAmbienteAttribute(false, NomeAmbiente = "Categorie articoli")]

        CategorieArticoliList,
        [UIAmbienteAttribute(true, NomeAmbiente = "Categoria articoli")]

        CategorieArticoliDett,

        [UIAmbienteAttribute(false, NomeAmbiente = "Lista Riordino Periodi")]
        RiordinoPeriodiList,

        [UIAmbienteAttribute(true, NomeAmbiente = "Periodo Riordino")]
        RiordinoPeriodi,
        
        [UIAmbienteAttribute(true, NomeAmbiente = "Impostazioni programma")]
        SettingProgramma,
    }


}