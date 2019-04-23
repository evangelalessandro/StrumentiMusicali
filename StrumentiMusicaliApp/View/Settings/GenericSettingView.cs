using StrumentiMusicali.App.View.BaseControl;

namespace StrumentiMusicali.App.View.Settings
{
	public class GenericSettingView : SettingBaseView
	{
		object _itemToBind;
 		public GenericSettingView(object itemToBind)
			: base()
		{
			_itemToBind = itemToBind;
            this.Load += GenericSettingView_Load;
            this.SuspendLayout();
            BindProp(_itemToBind, "");
            
        }

		private void GenericSettingView_Load(object sender, System.EventArgs e)
		{
            this.ResumeLayout();
        }

		//public void Rebind(object itemToBind)
		//{
		//	this.Load -= GenericSettingView_Load;
		//	Utility.UtilityView.SetDataBind(this, null, itemToBind);
			
		//}
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