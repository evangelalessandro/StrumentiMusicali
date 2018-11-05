using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
	static class ViewFactory
	{
		private static ObservableCollection<Tuple<enAmbienti, GenericSettingView>> _windows=new ObservableCollection<Tuple<enAmbienti, GenericSettingView>>();

		public static GenericSettingView GetView(enAmbienti ambiente)
		{
			
			return _windows.Where(a => a.Item1 == ambiente).Select(a => a.Item2).DefaultIfEmpty(null).FirstOrDefault(); 
		}

		internal static void AddView(enAmbienti ambiente, GenericSettingView view)
		{
			_windows.Add(new Tuple<enAmbienti, GenericSettingView>(ambiente, view));
		}
	}
}
