using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Articoli;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity.Scontrino
{
    public class ScontrinoRighe : BaseEntity
    {

        [CustomHideUIAttribute]
        [Required]
        public int ScontrinoTestataID { get; set; }

        [CustomHideUIAttribute]
        public virtual ScontrinoTestata ScontrinoTestata { get; set; }

        [CustomHideUIAttribute]
        //[Required]
        public System.Nullable<int> ArticoloID { get; set; }

        [CustomHideUIAttribute]
        public virtual   Articolo Articolo { get; set; }

        public string ArticoloDescrizione { get; set; }
        public bool  ArticoloGenerico { get; set; }

        public decimal PrezzoIvato { get; set; }
        public int Quantita{ get; set; }
        public decimal IvaPerc { get; set; }

        public int Reparto { get; set; } = 0;
    }
}