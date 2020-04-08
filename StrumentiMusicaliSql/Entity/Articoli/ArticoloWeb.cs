using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [CustomUIView(Ordine = 1, Category = "HTML",MultiLine =3)]
        [MaxLength(800)]
        public string DescrizioneBreveHtml { get; set; } = "";

        [CustomUIView(Ordine = 10,Enable =false)]
        [MaxLength(50)]
        public string CodiceArticoloEcommerce { get; set; } = "";

        [CustomUIView(Ordine = 20,Enable =false,DateTimeView =true)]
        public DateTime DataUltimoAggiornamentoWeb { get; set; } = DateTime.MinValue;



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
