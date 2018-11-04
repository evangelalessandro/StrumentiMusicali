using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.Manager
{
	[AddINotifyPropertyChangedInterface]
	public class ProgressManager : INotifyPropertyChanged
	{
		private static ProgressManager _Instance;

		public ProgressManager()
		{
		}

		public static ProgressManager Instance()
		{
			if (_Instance == null)
			{
				_Instance = new ProgressManager();
			}
			return _Instance;
		}

		public bool Visible { get; set; } = false;
		public int Value { get; set; }
		public int Max { get; set; }
		public string Messaggio { get; set; }

		public void RaiseProChange()
		{
			if (PropertyChanged!=null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs("Visible"));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
