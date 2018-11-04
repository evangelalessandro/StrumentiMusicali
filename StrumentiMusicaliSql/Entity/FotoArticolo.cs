using StrumentiMusicali.Library.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity
{
	public class FotoArticolo : BaseEntity
	{
		public int ArticoloID { get; set; }

		public virtual Articolo Articolo { get; set; }

		[Required]
		public string UrlFoto { get; set; }

		[Required]
		public int Ordine { get; set; } = -1;
	}
}