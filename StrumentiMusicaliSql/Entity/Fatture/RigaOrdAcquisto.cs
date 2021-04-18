using StrumentiMusicali.Library.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.Library.Entity.Fatture
{
    public class RigaOrdAcquisto : RigaDoc
    {
        [CustomHideUIAttribute]
        [Required]
        public int FatturaID { get; set; }

        [CustomHideUIAttribute]
        public virtual Fattura Fattura { get; set; }

        [CustomHideUIAttribute]
        [NotMapped]
        public decimal Importo { get { return PrezzoUnitario * Qta; } set { } }

        [Required]
        [CustomUIViewAttribute(Width = 100, Ordine = 4, Money = true)]
        public decimal PrezzoUnitario { get; set; }

        [CustomUIViewAttribute(Width = 40, Ordine = 5)]
        public string CodiceFornitore { get; set; }
    }
}
