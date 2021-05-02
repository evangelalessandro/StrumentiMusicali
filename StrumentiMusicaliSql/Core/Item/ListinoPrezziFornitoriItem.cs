using PropertyChanged;
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Entity.Articoli;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Core.Item
{
    [AddINotifyPropertyChangedInterface]
    public class ListinoPrezziFornitoriItem : BaseItem<ListinoPrezziFornitori>
    {
        public ListinoPrezziFornitoriItem()
        {
            ((INotifyPropertyChanged)this).PropertyChanged += ListinoPrezziFornitoriItem_PropertyChanged;
        }

        private void ListinoPrezziFornitoriItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ToSave = true;
        }

        [CustomUIViewAttribute(Ordine = 2, Enable = false)]

        public string Articolo { get; set; }
        [CustomUIViewAttribute(Ordine = 1, Enable = false)]

        public string Fornitore { get; set; }

        [CustomUIViewAttribute(Ordine = 3, Titolo = "Prezzo Unitario", Money = true,Enable =false)]
        public decimal Prezzo { get; set; } = 0;

        [CustomUIViewAttribute(Ordine = 3, Titolo = "Qta di SottoScorta", Enable = false)]
        public int SottoScorta { get; set; } = 0;
        
        [CustomUIViewAttribute(Ordine = 5, Enable = false)]

        public string CodiceArticoloFornitore { get; set; } = "";

        [CustomUIViewAttribute(Ordine = 7,Titolo ="Qta giacenza attuale", Enable = false)]
        public int QtaGiacenza { get; set; }

        [CustomUIViewAttribute(Ordine = 10, Titolo = "Qta da ordinare", Enable = true)]
        public int QtaDaOrdinare { get; set; }

        [CustomUIViewAttribute(Ordine = 9, Titolo = "Qta Minima ordine", Enable = false)]
        
        public int QtaMinimaOrdine { get; set; }


        [CustomUIViewAttribute(Ordine = 16, Titolo = "Qta in ordine", Enable = false)]
        public int QtaInOrdine{ get; set; }

        [CustomUIViewAttribute(Ordine = 160, Titolo = "Da salvare", Enable = false)]
        [NotMapped]
        [DoNotNotify]
        public bool ToSave { get; set; } = false;
    }
}