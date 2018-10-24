using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrumentiMusicali.App.Core.MenuRibbon
{

	public class RibbonMenuButton : BaseRibbonItem,  INotifyPropertyChanged
	{
		public Bitmap Immagine { get; set; }
		public event EventHandler Click;
		public event PropertyChangedEventHandler PropertyChanged;

		public void PerformClick()
		{
			if (Click != null)
				Click(this, new EventArgs());
		}
	}
	//[ImplementPropertyChanged]
	[AddINotifyPropertyChangedInterface]
	public class BaseRibbonItem :  INotifyPropertyChanged
	{
		public string Testo { get; set; }
		public bool Enabled { get; set; } = true;
		
		public bool Visible { get; set; } = true;
		public event PropertyChangedEventHandler PropertyChanged;


	}
	public class RibbonMenuPanel : BaseRibbonItem
	{
		public List<RibbonMenuButton> Pulsanti { get; set; } = new List<RibbonMenuButton>();

	 

		internal RibbonMenuButton Add(string titolo, Bitmap bitmap)
		{

			RibbonMenuButton button = new RibbonMenuButton() { Testo = titolo,Immagine=bitmap };
			Pulsanti.Add(button);
			return button;
		}
	}
	public class RibbonMenuTab : BaseRibbonItem
	{
		public List<RibbonMenuPanel> Pannelli { get; set; } = new List<RibbonMenuPanel>();

	 

		internal RibbonMenuPanel Add(string titolo)
		{
			RibbonMenuPanel ribPannel = new RibbonMenuPanel() { Testo = titolo };
			Pannelli.Add(ribPannel);
			return ribPannel;
		}
	}
	public class MenuTab
	{
		public List<RibbonMenuTab> Tabs { get; set; } = new List<RibbonMenuTab>();

		
		internal RibbonMenuTab Add( string text)
		{
			var tab = new RibbonMenuTab();
			Tabs.Add(tab);
			return tab;
		}
	}
}
