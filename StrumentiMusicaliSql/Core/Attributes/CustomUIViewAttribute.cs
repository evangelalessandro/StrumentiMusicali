using System;
using System.Collections.Generic;

namespace StrumentiMusicali.Library.Core
{
	public class CustomUIViewAttribute : Attribute
	{
		public CustomUIViewAttribute()
		{
		}

		public int Width { get; set; } = 200;

		public int MultiLine { get; set; } = 0;

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

    }
    public enum TipoDatiCollegati
	{
		Nessuno,
		Clienti,
		Categorie,
		Condizione,
        Articoli,
        Marca,
        Modello,
        Rivenditore,
        Colore,
        NomeStrumento,
        LibroAutore
    }
	 

}