using Autofac;
using Newtonsoft.Json;
using NLog;
using StrumentiMusicaliApp.Settings;
using StrumentiMusicaliSql.Repo;
using System;
using System.IO;
using System.Windows.Forms;

namespace StrumentiMusicaliApp.Core.Controllers
{
	public class BaseController : IDisposable
	{
		internal readonly ILogger _logger = LogManager.GetCurrentClassLogger();
		internal readonly string _PathSetting = Path.Combine(Application.StartupPath, @"settings.json");
		public BaseController()
		{
 
		}
		public UserSettings ReadSetting()
		{
			if (File.Exists(_PathSetting))
			{ 
				var json = File.ReadAllText(_PathSetting);
				return JsonConvert.DeserializeObject<UserSettings>(json);
			}
			else
			{
				return new UserSettings();
			}
		}
		 
		public void SaveSetting(UserSettings settings)
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
