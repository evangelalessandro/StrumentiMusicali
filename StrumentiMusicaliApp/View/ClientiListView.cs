using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.Settings;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Settings;
using StrumentiMusicali.App.View.Utility;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace StrumentiMusicali.App.View
{
	public class ClientiListView : BaseGridViewGeneric<ClientiItem, ControllerClienti, Cliente>, IMenu, IDisposable
	{
		private ControllerClienti _ControllerClienti;
		private System.Windows.Forms.Panel pnlArticoli;
		private System.Windows.Forms.Panel pnlCerca;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCerca;
		Subscription<Edit<ClientiItem, Cliente>> _subEdit;
		public ClientiListView(ControllerClienti controller)
			: base(controller)
		{
			this.Load += control_Load;
			_ControllerClienti = controller;
			_subEdit = EventAggregator.Instance().Subscribe<Edit<ClientiItem, Cliente>>((a) => { EditItem();
			});

			Init();
			(_ControllerClienti as INotifyPropertyChanged).PropertyChanged += (b, c) => {
				var dato = c.PropertyName;
				UpdateButtonState();
			};
			txtCerca.KeyUp += TxtCerca_KeyUp;
		}
		private void TxtCerca_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				var dato = _ControllerClienti.ReadSetting(enAmbienti.ClientiList);
				dato.LastStringaRicerca = txtCerca.Text;
				_ControllerClienti.SaveSetting(enAmbienti.ClientiList, dato);
				_ControllerClienti.TestoRicerca = txtCerca.Text;
				_ControllerClienti.RefreshList(null);

				dgvRighe.DataSource = _ControllerClienti.DataSource;

				dgvRighe.Refresh();

				UpdateButtonState();
			}
		}
		private void Init()
		{
			this.pnlArticoli = new System.Windows.Forms.Panel();
			this.pnlCerca = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.txtCerca = new System.Windows.Forms.TextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			// 
			// pnlCerca
			// 
			this.pnlCerca.Controls.Add(this.label1);
			this.pnlCerca.Controls.Add(this.txtCerca);
			this.pnlCerca.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlCerca.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pnlCerca.Location = new System.Drawing.Point(0, 0);
			this.pnlCerca.Name = "pnlCerca";
			this.pnlCerca.Size = new System.Drawing.Size(851, 71);
			this.pnlCerca.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(26, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Testo";
			// 
			// txtCerca
			// 
			this.txtCerca.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCerca.Location = new System.Drawing.Point(76, 23);
			this.txtCerca.Name = "txtCerca";
			this.txtCerca.Size = new System.Drawing.Size(763, 22);
			this.txtCerca.TabIndex = 0;
			// 
			// splitter1
			// 
			this.splitter1.BackColor = System.Drawing.Color.PowderBlue;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 71);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(851, 11);
			this.splitter1.TabIndex = 4;
			this.splitter1.TabStop = false;
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.pnlCerca);

		}
		private void control_Load(object sender, System.EventArgs e)
		{
			var dato = _ControllerClienti.ReadSetting(enAmbienti.ClientiList);
			txtCerca.Text = dato.LastStringaRicerca;
			_ControllerClienti.TestoRicerca= dato.LastStringaRicerca;
			_ControllerClienti.RefreshList(null);
			dgvRighe.DataSource = _ControllerClienti.DataSource;

			dgvRighe.Refresh();
			EventAggregator.Instance().Subscribe<UpdateList<Cliente>>(RefreshList);

			dgvRighe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}

		private void RefreshList(UpdateList<Cliente> obj)
		{
			this.Invalidate();

			dgvRighe.DataSource = _ControllerClienti.DataSource;

			dgvRighe.Refresh();
			dgvRighe.Update();
		}

		public override void FormatGrid()
		{

			dgvRighe.Columns["Entity"].Visible = false;
			//dgvRighe.Columns["ID"].Visible = false;
			dgvRighe.AutoResizeColumns();
			//	dgvRighe.Columns["CodiceArt"].DisplayIndex = 0;
			//	dgvRighe.Columns["RigaDescrizione"].DisplayIndex = 1;
			//	dgvRighe.Columns["RigaQta"].DisplayIndex = 2;
			//	dgvRighe.Columns["PrezzoUnitario"].DisplayIndex = 3;
			//	dgvRighe.Columns["RigaImporto"].DisplayIndex = 4;
			//	dgvRighe.Columns["Iva"].DisplayIndex = 5;
			//}
		}
		private MenuTab _menuTab = null;

		public MenuTab GetMenu()
		{
			if (_menuTab == null)
			{
				_menuTab = new MenuTab();

				AggiungiComandiArticolo();

			}
			return _menuTab;
		}
		private RibbonMenuButton ribCercaArticolo;

		private RibbonMenuButton ribDeleteArt;

		//private RibbonMenuButton ribDuplicaArt;

		private RibbonMenuButton ribEditArt;

		private void AggiungiComandiArticolo()
		{
			var tabArticoli = _menuTab.Add("Clienti");
			var panelComandiArticoli = tabArticoli.Add("Comandi");
			var ribCreArt = panelComandiArticoli.Add("Crea", Properties.Resources.Add);
			ribEditArt = panelComandiArticoli.Add(@"Vedi\Modifica", Properties.Resources.Edit);
			ribDeleteArt = panelComandiArticoli.Add("Cancella", Properties.Resources.Delete);
			//ribDuplicaArt = panelComandiArticoli.Add("Duplica", Properties.Resources.Duplicate);
			var panelComandiArticoliCerca = tabArticoli.Add("Cerca");
			ribCercaArticolo = panelComandiArticoliCerca.Add("Cerca", Properties.Resources.Find);

			ribCreArt.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new Add<ClientiItem, Cliente>());
			};
			ribDeleteArt.Click += (a, e) =>
			{
				EventAggregator.Instance().Publish(new Remove<ClientiItem, Cliente>());
			};
			ribEditArt.Click += (a, e) =>
			{
				EditItem();
			};
			//ribDuplicaArt.Click += (a, e) =>
			//{
			//	EventAggregator.Instance().Publish(new dupl<ClientiItem, Cliente>());
			//};
			ribCercaArticolo.Click += (a, e) =>
			{
				bool visibleCerca = pnlCerca.Visible;
				pnlCerca.Visible = !visibleCerca;
				splitter1.Visible = !visibleCerca;
				if (!visibleCerca)
				{
					pnlArticoli.Dock = DockStyle.Fill;
				}
				else
				{
					pnlCerca.Dock = DockStyle.Top;
				}
				UpdateButtonState();
			};
		}
		private async void EditItem()
		{
			var item = _ControllerClienti.ReadSetting().settingSito;
			if (!item.CheckFolderImmagini())
			{
				return;
			}
			var itemSelected = (ClientiItem)dgvRighe.SelectedRows[0].DataBoundItem;
			using (var view = new GenericSettingView(itemSelected.Entity))
			{
				view.OnSave += (a, b) =>
				{
					view.Validate();
					EventAggregator.Instance().Publish<Save<ClientiItem, Cliente>>
					(new Save<ClientiItem, Cliente>());
				};
				_ControllerClienti.ShowView(view, enAmbienti.Cliente);
			}

			_ControllerClienti.RefreshList(null);
			await dgvRighe.SelezionaRiga(itemSelected.ID);
		}
		private void UpdateButtonState()
		{
			if (_menuTab != null)
			{
				_menuTab.Enabled = !(dgvRighe.DataSource == null);

				ribEditArt.Enabled = dgvRighe.SelectedRows.Count > 0;
				//ribDuplicaArt.Enabled = dgvRighe.SelectedRows.Count > 0;
				ribDeleteArt.Enabled = dgvRighe.SelectedRows.Count > 0;
				ribCercaArticolo.Checked = pnlCerca.Visible;
			}
		}
		public new void Dispose()
		{
			base.Dispose();
			EventAggregator.Instance().UnSbscribe(_subEdit);
		}
	}
	
}