namespace StrumentiMusicali.App.Core.Events
{
	public class ArticoloCurrent
	{
		public ArticoloCurrent(ArticoloItem itemSelected)
		{
			ItemSelected = itemSelected;
		}
		public ArticoloItem ItemSelected { get; private set; }
	}
}
