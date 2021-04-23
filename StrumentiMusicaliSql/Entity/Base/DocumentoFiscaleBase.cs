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
        [StringLength(150)]
        public string RagioneSociale { get; set; }
        [StringLength(50)]
        public string PIVA { get; set; }

        [Required]
        public int ClienteFornitoreID { get; set; }

        public virtual Soggetto ClienteFornitore { get; set; }
        [StringLength(150)]
        public string TrasportoACura { get; set; }
        [StringLength(100)]
        public string CausaleTrasporto { get; set; }
        [StringLength(150)]
        public string Porto { get; set; }
        [StringLength(150)]
        public string Vettore { get; set; }
        [StringLength(150)]
        public string AspettoEsterno { get; set; }
        public int NumeroColli { get; set; }
        public int PesoKg { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DataTrasporto { get; set; }
        [DataType(DataType.Time)]
        public DateTime? OraTrasporto { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string Note1 { get; set; }
        [StringLength(500)]
        public string Note2 { get; set; }
    }
}