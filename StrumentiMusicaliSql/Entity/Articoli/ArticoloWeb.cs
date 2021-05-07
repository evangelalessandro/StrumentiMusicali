using StrumentiMusicali.Library.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Articoli
{
    public class ArticoloWeb : INotifyPropertyChanged
    {
        [CustomUIView(Ordine = 2, Category = "HTML", MultiLine = 6)]
        [MaxLength(4000)]
        public string DescrizioneHtml { get; set; } = "";

        [CustomUIView(Ordine = 1, Category = "HTML", MultiLine = 3)]
        [MaxLength(800)]
        public string DescrizioneBreveHtml { get; set; } = "";

        [MaxLength(2000)]
        [CustomUIView(Width = 500, Ordine = 111, MultiLine = 4, Titolo = "Testo annuncio", Category = "HTML", FunzioneAbilitazione = enFunzioniCheck.Ecommerce)]
        public string Testo { get; set; }

        [CustomUIView(Width = 80, Titolo = "Prezzo per il web iva compresa", Ordine = 30, Category = "Prezzo", Money = true)]
        [Required]
        public decimal PrezzoWeb { get; set; } = 0;

        


        //[CustomUIView(Width = 60, Ordine = 44, Category = "Prezzo", Money = true)]
        //public bool PrezzoARichiesta { get; set; } = false;

        //[CustomUIView(Width = 60, Ordine = 45, Category = "Prezzo", Money = true)]
        //public decimal PrezzoBarrato { get; set; } = 0;


        public event PropertyChangedEventHandler PropertyChanged;
    }
}