using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.FatturaElett;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.BaseControl.ElementiDettaglio;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StrumentiMusicali.App.View.DatiMittenteFattura
{
	public class DatiSitoView : SettingBaseView
	{ 
		private SettingSito _setting;
		private BaseController controller;
		public DatiSitoView()
			: base()
		{
			 
			controller = new BaseController();

			_setting = controller.ReadSetting().settingSito;

			if (_setting == null)
				_setting = new SettingSito();

			BindProp(_setting, "");

			this.OnSave += (a,b)=> { Salva(); };
		}

		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~DatiSitoView()
		{
			if (controller!=null)
			{ 
				controller.Dispose();
			}
			controller = null;
			// Finalizer calls Dispose(false)
			Dispose(false);
		}
		private void Salva()
		{
			using (var cur = new CursorManager())
			{
				this.Validate();
				var setting = controller.ReadSetting();
				setting.settingSito = _setting;
				controller.SaveSetting(setting);

				MessageManager.NotificaInfo(MessageManager.GetMessage(Core.Controllers.enSaveOperation.OpSave));
			}
		}
	}
}