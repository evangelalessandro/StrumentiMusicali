using StrumentiMusicali.Library.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrumentiMusicali.Library.Entity.Base
{
	public class BaseEntity
	{
		[CustomHideUIAttribute]
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		[CustomUIViewAttribute(DateTimeView =true,Width =200)]
		public DateTime DataCreazione { get; set; } = DateTime.Now;
		[CustomUIViewAttribute(DateTimeView = true, Width = 200)]
		public DateTime DataUltimaModifica { get; set; } = DateTime.Now;
	}
}