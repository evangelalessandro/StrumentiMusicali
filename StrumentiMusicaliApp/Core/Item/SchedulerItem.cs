using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Altro;
using System;

namespace StrumentiMusicali.App.Core.Item
{
    public class SchedulerItem : BaseItem<SchedulerJob>
    {
        public SchedulerItem()
            : base()
        {
        }

        public SchedulerItem(SchedulerJob schedulerField)
            : base()
        {
            ID = schedulerField.ID;
            Nome = schedulerField.Nome;
            ProssimoAvvio = schedulerField.ProssimoAvvio;
            UltimaEsecuzione = schedulerField.UltimaEsecuzione;

            Errore = schedulerField.Errore;
            Disabled = schedulerField.Disabled;
            Duration = schedulerField.Duration;
        }

        public string Nome { get; set; }
        public DateTime ProssimoAvvio { get; set; }
        public DateTime UltimaEsecuzione { get; set; }
        public string Errore { get; set; }
        public bool Disabled { get; set; }

        public TimeSpan Duration { get; set; }
    }
}