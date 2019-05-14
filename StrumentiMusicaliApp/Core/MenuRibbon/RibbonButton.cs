using PropertyChanged;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace StrumentiMusicali.App.Core.MenuRibbon
{

	[AddINotifyPropertyChangedInterface]
	public class RibbonMenuButton : BaseRibbonItem
	{
		public Bitmap Immagine { get; set; }
		public bool Checked { get; set; } = false;
		public bool EnableOnlyExistItem { get; set; } 
		public event EventHandler Click;
		public string Tag { get; set; }
		public void PerformClick()
		{
			if (Click != null)
				Click(this, new EventArgs());
		}
        public override string ToString()
        {
            var text = this.Testo;
            text +=  this.Enabled ? "Abilitato" : "Disabilitato" ;
            return text;
        }
    }

	//[ImplementPropertyChanged]
	//[AddINotifyPropertyChangedInterface]
	public class BaseRibbonItem  : INotifyPropertyChanged
	{
		public string Testo { get; set; }
		[AlsoNotifyForAttribute("Visible")]
		public bool Enabled { get;
			set; } = true;

		public bool Visible { get; set; } = true;

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class RibbonMenuPanel : BaseRibbonItem
	{
		public List<RibbonMenuButton> Pulsanti { get; set; } = new List<RibbonMenuButton>();

		internal RibbonMenuButton Add(string titolo, Bitmap bitmap, bool enableOnlyOnEdit=false)
		{
			RibbonMenuButton button = new RibbonMenuButton() { Testo = titolo, Immagine = bitmap };
			button.EnableOnlyExistItem = enableOnlyOnEdit;
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
		 
		public event EventHandler OnSelected;
	 
		public void PerformSelect()
		{
			if (OnSelected != null)
				OnSelected(this, new EventArgs());
		}
	}

	public class MenuTab : BaseRibbonItem
	{
		public void ApplyValidation(bool itemSelected)
		{
			foreach (var item in Tabs.SelectMany(a=>a.Pannelli).SelectMany(a=>a.Pulsanti).Where(a=>a.EnableOnlyExistItem))
			{
				item.Enabled = itemSelected;
			}
		}
		public List<RibbonMenuButton> ItemByTag(string tag)
		{
			return Tabs.SelectMany(a => a.Pannelli).SelectMany(a => a.Pulsanti).Where(a => a.Tag == tag).ToList();
			 
		}
		public List<RibbonMenuTab> Tabs { get; set; } = new List<RibbonMenuTab>();

		internal RibbonMenuTab Add(string text)
		{
			var tab = new RibbonMenuTab() { Testo = text };
			Tabs.Add(tab);
			return tab;
		}

		public const string TagCerca = "CERCA";
        public const string TagCercaClear = "CERCAClear";
        
        public const string TagAdd = "ADD";
		public const string TagRemove = "Remove";
		public const string TagEdit = "Edit";

	}
}