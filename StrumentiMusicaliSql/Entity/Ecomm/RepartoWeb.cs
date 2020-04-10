using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Ecomm
{
    public class RepartoWeb : BaseEntity
    {

        [MaxLength(50)]
        public string Reparto { get; set; }
        public long CodiceWeb { get; set; }
        
    }
}