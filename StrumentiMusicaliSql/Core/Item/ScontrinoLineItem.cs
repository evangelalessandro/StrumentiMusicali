using PropertyChanged;
using System.ComponentModel;

namespace StrumentiMusicali.Library.Core.Item
{
    [AddINotifyPropertyChangedInterface]
    public class ScontrinoLineItem
    {
        public int Ordine { get; set; }
        public TipoRigaScontrino TipoRigaScontrino { get; set; }
        public int Articolo { get; set; }
        public string Descrizione { get; set; }
        public decimal IvaPerc { get; set; }
        public decimal PrezzoIvato { get; set; }


        //public event PropertyChangedEventHandler PropertyChanged;


    }
    public enum TipoRigaScontrino
    {
        Vendita,
        Sconto,
        Totale
    }
}
