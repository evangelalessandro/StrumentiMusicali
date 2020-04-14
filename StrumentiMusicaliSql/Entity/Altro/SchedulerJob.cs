using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;

namespace StrumentiMusicali.Library.Entity.Altro
{
    public class SchedulerJob : BaseEntity
    {
        public string Nome { get; set; }

        [CustomUIView(Width = 80, Enable = false, Titolo = "Prossimo Avvio", DateTimeView = true)]
        public DateTime ProssimoAvvio { get; set; } = new DateTime(1900, 1, 1);

        [CustomUIView(Width = 80, Enable = false, Titolo = "Ultimo avvio", DateTimeView = true)]
        public DateTime UltimaEsecuzione { get; set; } = new DateTime(1900, 1, 1);

        public string Errore { get; set; }
        public bool Disabled { get; set; }

        [CustomUIView(Width = 80, Enable = false, Titolo = "Durata ultima esecuzione", DateTimeView = true)]
        public TimeSpan Duration { get; set; }
    }
}