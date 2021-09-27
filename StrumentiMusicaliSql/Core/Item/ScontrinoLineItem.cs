using PropertyChanged;
using System.ComponentModel;

namespace StrumentiMusicali.Library.Core.Item
{
    [AddINotifyPropertyChangedInterface]
    public class ScontrinoLineItem
    {
        public TipoRigaScontrino TipoRigaScontrino { get; set; }
        public int Articolo { get; set; }
        public string Descrizione { get; set; }
        public decimal IvaPerc { get; set; }

        public int Reparto { get; set; }

        public int Qta { get; set; }


        public decimal PrezzoIvato { get; set; }

        public int ScontoPerc { get; set; } = 0;

        

        //public event PropertyChangedEventHandler PropertyChanged;


    }
    public enum TipoRigaScontrino
    {
        Vendita,
        ScontoIncondizionato,
        Totale,
        Sconto,
        Incassato
    }
}
