using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity.Articoli
{
    [Table("ListiniPrezzi_VenditaNomi")]
    public class ListinoPrezziVenditaNome : BaseEntity
    {
        [CustomUIView(Ordine = 10, Titolo = "Nome listino")]
        [StringLength(15, ErrorMessage = "Il nome deve essere da 1 a 15 caratteri", MinimumLength = 1)]
        [Index(IsClustered = false, IsUnique = true, Order = 1)]
        public string Nome { get; set; }



    }
}