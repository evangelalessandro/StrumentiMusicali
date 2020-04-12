using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [CustomUIView(Ordine = 10, Enable = false, Category = "Allineamento")]
        [MaxLength(50)]
        public string CodiceArticoloEcommerce { get; set; } = "";

        [CustomUIView(Ordine = 20, Enable = false, DateTimeView = true, Category = "Allineamento")]
        public DateTime DataUltimoAggiornamentoWeb { get; set; } = DateTime.MinValue;


        [CustomUIView(Width = 80, Titolo = "Prezzo per il web", Ordine = 30, Category = "Prezzo", Money = true)]
        [Required]
        public decimal PrezzoWeb { get; set; } = 0;

        [CustomUIView(Width = 50, Titolo = "Percentuale iva", Ordine = 35, Category = "Prezzo", Money = true)]
        public decimal Iva { get; set; } = 22;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
