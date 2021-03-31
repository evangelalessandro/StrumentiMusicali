using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Entity
{
    public class TipiPagamentoScontrino : BaseEntity
    {
        public int Codice { get; set; }

        public string Descrizione { get; set; }

        public bool Enable { get; set; } = true;

    }
}
