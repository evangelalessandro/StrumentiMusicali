using StrumentiMusicali.Core.Properties;
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using System;
using System.Drawing;

namespace StrumentiMusicali.App.Core.Item
{
    public class LogItem : BaseItem<EventLog>
    {
        public LogItem()
            : base()
        {
        }
        public LogItem(EventLog eventLog)
            : base()
        {
            ID = eventLog.ID;

            if (eventLog.TipoEvento == "Error")
            {
                TipoEvento = ImageIcons.Error_24;
            }
            else if (eventLog.TipoEvento.ToLowerInvariant().Contains("Warn".ToLowerInvariant()))
            {
                TipoEvento = ImageIcons.Warning_24;
            }
            else
            {
                TipoEvento = ImageIcons.Info_24;
            }
            Evento = eventLog.Evento;
            Errore = eventLog.Errore;
            StackTrace = eventLog.StackTrace;
            DataCreazione = eventLog.DataCreazione;
            InnerException = eventLog.InnerException;
            Class = eventLog.Class;
        }

        public string Evento { get; set; }
        public Bitmap TipoEvento { get; set; }
        public string Errore { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Class { get; set; }
        public DateTime DataCreazione { get; set; }
    }
}
