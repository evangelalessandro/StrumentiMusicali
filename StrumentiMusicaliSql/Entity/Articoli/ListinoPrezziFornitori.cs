using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Entity.Articoli
{

    [Table("DAT_ListinoPrezziFornitori")]
    public class ListinoPrezziFornitori : BaseEntity
    {
        [CustomUIView(Width = 100, Ordine = 1, Combo = TipoDatiCollegati.Articoli, Titolo = "Articolo")]
        [Required]
        [Column("LPF_ArticoloID")]
        public int ArticoloID { get; set; }

        [CustomHideUI]
        public virtual Articolo Articolo { get; set; }


        [CustomUIView(Width = 100, Ordine = 1, Combo = TipoDatiCollegati.Fornitori, Titolo = "Fornitore")]
        [Required]
        [Column("LPF_FornitoreID")]
        public int FornitoreID { get; set; }

        [CustomHideUI]
        public virtual Soggetto Fornitore { get; set; }

        [CustomUIView(Ordine = 10, Money = true, Titolo = "Prezzo Unitario")]
        [Column("LPF_Prezzo")]
        public decimal Prezzo { get; set; } = 0;

        [CustomUIView(Ordine = 20, Titolo = "Codice Articolo Fornitore")]
        [Column("LPF_CodiceArticoloFornitore")]
        public string CodiceArticoloFornitore { get; set; }
        [CustomUIView(Ordine = 30, Titolo = "Quantità minima ordinabile (0=indifferente)")]
        [Column("LPF_QTA_MINIMA")]
        public int MinimoOrdinabile { get; set; } = 0;

    }
}
