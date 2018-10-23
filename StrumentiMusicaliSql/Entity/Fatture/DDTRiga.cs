using System.ComponentModel.DataAnnotations;

namespace StrumentiMusicali.Library.Entity
{
	public class DDTRiga : RigaDoc
	{ 
		[Required]
		public int DDTID { get; set; }

		public virtual DDt DDt { get; set; }
	}
}