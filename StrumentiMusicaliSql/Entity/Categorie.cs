using StrumentiMusicali.Library.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
	public class Categoria : BaseEntity
	{
 		public string Reparto { get; set; }
		public string Nome { get; set; }
		public string CategoriaCondivisaCon { get; set; }
	}
}