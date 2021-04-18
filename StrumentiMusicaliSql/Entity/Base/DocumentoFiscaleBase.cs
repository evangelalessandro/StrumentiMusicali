using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
    public abstract class DocumentoFiscaleBase : BaseEntity
    {
        [Required]
        [StringLength(10)]
        public string Codice { get; set; }

        [Required]
        public DateTime Data { get; set; } = DateTime.Now;

        public string RagioneSociale { get; set; }
        public string PIVA { get; set; }

        [Required]
        public int ClienteFornitoreID { get; set; }

        public virtual Soggetto ClienteFornitore { get; set; }

        public string TrasportoACura { get; set; }
        public string CausaleTrasporto { get; set; }
        public string Porto { get; set; }
        public string Vettore { get; set; }
        public string AspettoEsterno { get; set; }
        public int NumeroColli { get; set; }
        public int PesoKg { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DataTrasporto { get; set; }
        [DataType(DataType.Time)]
        public DateTime? OraTrasporto { get; set; }
        [DataType(DataType.MultilineText)]
        public string Note1 { get; set; }
        public string Note2 { get; set; }
    }
}