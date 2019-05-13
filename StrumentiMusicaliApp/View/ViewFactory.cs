using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.Library.View.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace StrumentiMusicali.App.View
{
    static class ViewFactory
	{
		private static ObservableCollection<Tuple<enAmbiente, GenericSettingView>> _windows=new ObservableCollection<Tuple<enAmbiente, GenericSettingView>>();

		public static GenericSettingView GetView(enAmbiente ambiente)
		{
			
			return _windows.Where(a => a.Item1 == ambiente).Select(a => a.Item2).DefaultIfEmpty(null).FirstOrDefault(); 
		}

		internal static void AddView(enAmbiente ambiente, GenericSettingView view)
		{
			_windows.Add(new Tuple<enAmbiente, GenericSettingView>(ambiente, view));
		}
	}
}
