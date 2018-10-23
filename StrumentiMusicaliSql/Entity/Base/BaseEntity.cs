using System;

namespace StrumentiMusicali.Library.Entity.Base
{
	public class BaseEntity
	{
		public DateTime DataCreazione { get; set; } = DateTime.Now;
		public DateTime DataUltimaModifica { get; set; } = DateTime.Now;
	}
}