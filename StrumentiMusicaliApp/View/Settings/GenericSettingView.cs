using StrumentiMusicali.App.View.BaseControl;

namespace StrumentiMusicali.App.View.Settings
{
	public class GenericSettingView : SettingBaseView
	{ 
 		public GenericSettingView(object itemToBind)
			: base()
		{
			BindProp(itemToBind, "");	
		}

		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~GenericSettingView()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}
		
	}
}