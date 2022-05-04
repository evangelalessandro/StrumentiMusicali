using StrumentiMusicali.Library.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Articoli
{
	public class ArticoloWeb : INotifyPropertyChanged
	{
		[CustomUIView(Ordine = 3, Category = "HTML", MultiLine = 6,HTMLViewer =true)]
		[MaxLength(20000)]
		public string DescrizioneHtml { get; set; } = "";


		[CustomUIView(Ordine = 2, Category = "HTML", MultiLine = 3, HTMLViewer = true)]
		[MaxLength(800)]
		public string DescrizioneBreveHtml { get; set; } = "";

		 


		[CustomUIView(Width = 80, Titolo = "Prezzo per il web iva compresa", Ordine = 30, Category = "Prezzo", Money = true)]
		[Required]
		public decimal PrezzoWeb { get; set; } = 0;

		[CustomUIView(Ordine = 100, Category = "Riferimenti",Enable =false,Titolo ="Codice identificativo web")]
		public long CodiceArticoloWeb { get; set; } = 0;

		//[CustomUIView(Width = 50, Titolo = "Percentuale iva", Ordine = 35, Category = "Prezzo")]
		//public decimal Iva { get; set; } = 22;

		public event PropertyChangedEventHandler PropertyChanged;
	}
}