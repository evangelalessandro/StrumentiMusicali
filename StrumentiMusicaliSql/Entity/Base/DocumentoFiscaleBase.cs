using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public abstract class DocumentoFiscaleBase : BaseEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[Required]
		public string Codice { get; set; }

		[Required]
		public DateTime Data { get; set; } = DateTime.Now;

		public string RagioneSociale { get; set; }
		public string PIVA { get; set; }

		[Required]
		public int ClienteID { get; set; }

		public virtual Cliente Cliente { get; set; }
		public int TipoDocumento { get; set; }
		public string TrasportoACura { get; set; }
		public string CausaleTrasporto { get; set; }
		public string Porto { get; set; }
		public string Vettore { get; set; }
		public string AspettoEsterno { get; set; }
		public int NumeroColli { get; set; }
		public int PesoKg { get; set; }
		public DateTime? DataTrasporto { get; set; }
		public DateTime? OraTrasporto { get; set; }
		public string Note1 { get; set; }
		public string Note2 { get; set; }
	}
}