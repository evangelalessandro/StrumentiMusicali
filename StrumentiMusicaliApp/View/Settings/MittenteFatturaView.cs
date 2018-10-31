using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.FatturaElett;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.BaseControl.ElementiDettaglio;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StrumentiMusicali.App.View.Settings
{
	public class MittenteFatturaView : SettingBaseView
	{ 
		private DatiMittente _datiMittente;
		private BaseController controller;
		public MittenteFatturaView()
			: base()
		{
			 
			controller = new BaseController();

			_datiMittente = controller.ReadSetting().datiMittente;

			if (_datiMittente == null)
				_datiMittente = new DatiMittente();
			if (_datiMittente.UfficioRegistroImp == null)
				_datiMittente.UfficioRegistroImp = new DatiMittente.UfficioRegistro();
			BindProp(_datiMittente, "");

			this.OnSave += (a,b)=> { Salva(); };
		}

		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~MittenteFatturaView()
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
				setting.datiMittente = _datiMittente;
				controller.SaveSetting(setting);

				MessageManager.NotificaInfo(MessageManager.GetMessage(Core.Controllers.enSaveOperation.OpSave));
			}
		}
	}
}