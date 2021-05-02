using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Core.Item
{
    public class ListinoPrezziVenditaNomeItem : BaseItem<ListinoPrezziVenditaNome>
    {
        
        public ListinoPrezziVenditaNomeItem()
        {

        }
        public string Nome { get; set; }



    }
}