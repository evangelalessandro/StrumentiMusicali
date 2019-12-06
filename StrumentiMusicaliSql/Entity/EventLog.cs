using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Entity
{
    public class EventLog : BaseEntity
    {

        public string Evento { get; set; }
        public string TipoEvento { get; set; }
        public string Errore { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Class { get; set; }
        public int ThreadId { get; set; }
    }
}