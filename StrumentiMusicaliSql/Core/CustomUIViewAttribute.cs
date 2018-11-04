using System;
 
namespace StrumentiMusicali.Library.Core
{

	public class CustomUIViewAttribute : Attribute
	{
		public CustomUIViewAttribute() 
		{
		 
		}
		public int Width { get; set; }
		/// <summary>
		/// ordine di visualizzazione nella form
		/// </summary>
		public int Ordine { get; set; }

		public bool DateView { get; set; }

		public bool TimeView { get; set; }

		public bool DateTimeView { get; set; }
	}

}