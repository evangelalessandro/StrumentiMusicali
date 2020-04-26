using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;

namespace StrumentiMusicali.Library.Entity.Altro
{
    public class SchedulerJob : BaseEntity
    {
        [CustomUIView(Width = 80, Enable = false)]
        public string Nome { get; set; }

        [CustomHideUI]
        public DateTime ProssimoAvvio { get; set; } = new DateTime(1900, 1, 1);

        //       [CustomUIView(Width = 80, Enable = false, Titolo = "Ultimo avvio", DateTimeView = true)]
        [CustomHideUI]
        public DateTime UltimaEsecuzione { get; set; } = new DateTime(1900, 1, 1);
        [CustomHideUI]
        public string Errore { get; set; }

        [CustomUIView(Width = 80, Enable = true, Titolo = "Abilitato")]
        public bool Enabled { get; set; }

        [CustomHideUI]
        public TimeSpan Duration { get; set; }
    }
}