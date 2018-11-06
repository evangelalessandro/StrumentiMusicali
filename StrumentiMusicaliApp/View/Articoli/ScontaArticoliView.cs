using StrumentiMusicali.App.Core.Events.Articoli;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using System;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View.Articoli
{
	public partial class ScontaArticoliView : UserControl, IMenu, ICloseSave
	{
		public ScontaArticoliView()
		{
			InitializeComponent();
		}

		private MenuTab _menuTab = null;

		public event EventHandler<EventArgs> OnSave;
		public event EventHandler<EventArgs> OnClose;

		public MenuTab GetMenu()
		{
			if (_menuTab == null)
			{
				_menuTab = new MenuTab();

				AggiungiComandi();

			}
			return _menuTab;
		}

		private void AggiungiComandi()
		{
			var tabArticoli = _menuTab.Add("Principale");
			var panelComandi = tabArticoli.Add("Comandi");
			UtilityView.AddButtonSaveAndClose(panelComandi, this, false);

			var ribCrea = panelComandi.Add("Conferma", Properties.Resources.Check_OK_48);
			ribCrea.Click += RibCrea_Click;
		}

		private void RibCrea_Click(object sender, EventArgs e)
		{
			this.Validate();
			EventAggregator.Instance().Publish(new ArticoloSconta(numericUpDown1.Value ) );
			RaiseClose();
		}

		public void RaiseSave()
		{
			 
		}

		public void RaiseClose()
		{
			OnClose?.Invoke(this, new EventArgs());

		}
	}
}
