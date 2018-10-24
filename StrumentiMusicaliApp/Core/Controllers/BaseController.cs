using Autofac;
using Newtonsoft.Json;
using NLog;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.Library.Repo;
using System;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace StrumentiMusicali.App.Core.Controllers
{
	public class BaseController : IDisposable
	{
		internal readonly ILogger _logger = LogManager.GetCurrentClassLogger();
		internal readonly string _PathSetting = Path.Combine(Application.StartupPath, @"settings.json");
		public BaseController()
		{
 
		}
		
		private UserSettings ReadSetting()
		{
			UserSettings setting;
			if (File.Exists(_PathSetting))
			{
				var json = File.ReadAllText(_PathSetting);
				setting = JsonConvert.DeserializeObject<UserSettings>(json);
			}
			else
			{
				setting = new UserSettings();
			}
			return setting;
		}
		public FormRicerca ReadSetting(enAmbienti ambiente)
		{
			var setting = ReadSetting();
			if (setting.Form==null)
			{
				setting.Form = new System.Collections.Generic.List<Tuple<enAmbienti, FormRicerca>>();
			}
			var elem= setting.Form.Where(a => a.Item1 == ambiente).FirstOrDefault();
			if (elem==null)
			{
				setting.Form.Add(new Tuple<enAmbienti, FormRicerca>(ambiente, new FormRicerca()));
				SaveSetting(setting);
			}
			return setting.Form.Where(a => a.Item1 == ambiente).First().Item2;
		}

		public void SaveSetting(enAmbienti ambiente, FormRicerca formRicerca)
		{
			var setting= ReadSetting();
		 	setting.Form.RemoveAll(a => a.Item1 == ambiente);
			setting.Form.Add(new Tuple<enAmbienti, FormRicerca>(ambiente, formRicerca));
			SaveSetting(setting);

		}
		private void SaveSetting(UserSettings settings)
		{
			File.WriteAllText(_PathSetting, 
				JsonConvert.SerializeObject(settings));
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~BaseController()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		// The bulk of the clean-up code is implemented in Dispose(bool)
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// free managed resources
			
			}
			// free native resources if there are any.
			 
		}

	}
}
