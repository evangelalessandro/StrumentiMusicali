using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
	public class Categorie : BaseEntity
	{
 		public string Reparto { get; set; }
		public string Categoria { get; set; }
		public string CategoriaCondivisaCon { get; set; }
	}
}