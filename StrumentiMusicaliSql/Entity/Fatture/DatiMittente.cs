using StrumentiMusicali.Library.Core;

namespace StrumentiMusicali.Library.Entity
{
    public class DatiMittente : Soggetto
    {

        [CustomUIViewAttribute(Ordine = 31)]
        public bool SoggettoARitenuta { get; set; }
        [CustomUIViewAttribute(Ordine = 32)]
        public bool IscrittoRegistroImprese { get; set; }
        [CustomUIViewAttribute(Ordine = 30)]
        public UfficioRegistro UfficioRegistroImp { get; set; } = new UfficioRegistro();

        public class UfficioRegistro
        {
            public string SiglaProv { get; set; }
            public string NumeroRea { get; set; }
            public decimal CapitaleSociale { get; set; }
            public bool SocioUnico { get; set; }
            public bool SocioMultiplo { get; set; }
        }
        /*assieme alla banca*/
        [CustomUIViewAttribute(Width = 120, Ordine = 13)]
        public string IBAN { get; set; }
    }
}