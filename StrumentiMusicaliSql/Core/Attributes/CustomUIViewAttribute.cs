using System;

namespace StrumentiMusicali.Library.Core
{
    public class CustomUIViewAttribute : Attribute
    {
        public CustomUIViewAttribute()
        {
        }

        public int Width { get; set; } = 200;

        public bool Money { get; set; } = false;
        public int MultiLine { get; set; } = 0;

        public string MaskDate { get; set; } = "";
        /// <summary>
        /// ordine di visualizzazione nella form
        /// </summary>
        public int Ordine { get; set; }

        public bool DateView { get; set; }

        public bool TimeView { get; set; }

        public bool DateTimeView { get; set; }

        public TipoDatiCollegati Combo { get; set; } = TipoDatiCollegati.Nessuno;

        public bool ComboLibera { get; set; } = false;

        public bool Enable { get; set; } = true;

        public string Titolo { get; set; } = "";

        public bool ShowGroupName { get; set; } = true;

        public string Category { get; set; } = "Generale";

        public bool Percentuale { get; set; } = false;

    }
    public enum TipoDatiCollegati
    {
        Nessuno,
        Clienti,
        Categorie,
        Condizione,
        Articoli,
        Fornitori,
        Marca,
        Modello,
        Rivenditore,
        Colore,
        NomeStrumento,
        LibroAutore,
        TipiSoggetto,
        TipiPagamentiDocumenti,
        RiordinoPeriodi
    }


}