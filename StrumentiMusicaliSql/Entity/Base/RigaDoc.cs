using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public class RigaDoc : BaseEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		public string CodiceArticoloOld { get; set; }

		public string ArticoloID { get; set; }

		public virtual Articolo Articolo { get; set; }

		public string Descrizione { get; set; }

		public int Qta { get; set; }
	}
}