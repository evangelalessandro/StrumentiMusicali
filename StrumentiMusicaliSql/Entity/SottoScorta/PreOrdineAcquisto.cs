using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity.Articoli
{
    [Table("DAT_PreordiniAcquisto")]
    public class PreOrdineAcquisto : BaseEntity
    {
        [Column("PRE_ARTICOLOID")]
        [Required]
        [Index("IX_DAT_PreOrdiniAcquisti_ART_FOR", IsUnique =true,IsClustered =false,Order =1)]
        public int ArticoloID { get; set; }
        [CustomHideUI]
        public virtual Articolo Articolo { get; set; }


        
        [Required]
        [Column("PRE_FORNITOREID")]
        [Index("IX_DAT_PreOrdiniAcquisti_ART_FOR", IsUnique = true, IsClustered = false, Order = 2)]
        [CustomUIView(Width = 100, Ordine = 1, Combo = TipoDatiCollegati.Fornitori, Titolo = "Fornitore")]
        public int FornitoreID { get; set; }

        [CustomHideUI]
        public virtual Soggetto Fornitore { get; set; }

        [CustomUIView(Width = 100, Ordine =3, Titolo = "Qta Da Ordinare")]
        
        [Column("PRE_QTA_DA_ORDINARE")]
        public int QtaDaOrdinare { get; set; } = 0;

    }
}