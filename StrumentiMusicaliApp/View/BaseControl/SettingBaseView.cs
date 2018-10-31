using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.FatturaElett;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.BaseControl.ElementiDettaglio;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StrumentiMusicali.App.View.BaseControl
{
	public class SettingBaseView : BaseDataControl, IMenu
	{
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
	 
		public SettingBaseView()
			: base()
		{
			InitializeComponent();

		 
		}

		public void BindProp(object objToBind, string prefixText)
		{
			foreach (var item in Utility.UtilityView.GetProperties(objToBind))
			{
				var titolo = string.Join(" ", Regex.Split(item.Name, @"(?<!^)(?=[A-Z])"));
				if (item.Name.CompareTo(item.Name.ToUpperInvariant()) == 0)
				{
					titolo = item.Name;
				}
				titolo = prefixText + titolo;
				if (item.PropertyType.FullName.Contains("String"))
				{
					EDTesto artCNT = AggiungiTesto(titolo);
					artCNT.Width = 250;
					BindObj(item, artCNT, objToBind);
				}
				else if (item.PropertyType.FullName.Contains("Boolean"))
				{
					var cnt = AggiungiCheck(titolo);
					BindObj(item, cnt, objToBind);
				}
				else if (item.PropertyType.FullName.Contains("Decimal"))
				{
					var cnt = AggiungiNumericoDecimal(titolo);
					BindObj(item, cnt, objToBind);
				}
				else
				{
					if (!item.PropertyType.Name.StartsWith("System."))
					{

						BindProp(item.GetValue(objToBind), "[" + titolo + "]  ");
					}
					else
					{

					}
				}
			}
		}

		private void BindObj(System.Reflection.PropertyInfo item, EDBase controlBase, object objToBind)
		{
			controlBase.Tag = item.Name;
			controlBase.BindProprieta(item.Name, objToBind);
			controlBase.Height = 50;
		}

		private EDCheckBok AggiungiCheck(string titolo)
		{
			var artCNT = new EDCheckBok();

			artCNT.Titolo = titolo;
			artCNT.SetMinSize = true;
			flowLayoutPanel1.Controls.Add(artCNT);
			return artCNT;
		}
		private EDTesto AggiungiTesto(string titolo, List<string> suggestTestList)
		{
			var artCNT = new EDTesto();
			if (suggestTestList != null)
				artCNT.SetListSuggest(suggestTestList.ToArray());
			artCNT.Titolo = titolo;
			flowLayoutPanel1.Controls.Add(artCNT);
			return artCNT;
		}

		private EDTesto AggiungiTesto(string titolo)
		{
			return AggiungiTesto(titolo, null);
		}

		private EDNumeric AggiungiNumerico(string titolo)
		{
			var qtaCNT = new EDNumeric();
			qtaCNT.Titolo = titolo;
			flowLayoutPanel1.Controls.Add(qtaCNT);
			qtaCNT.SetMinSize = true;
			qtaCNT.SetMinMax(0, 10000, 0);
			return qtaCNT;
		}

		private EDNumeric AggiungiNumericoDecimal(string titolo)
		{
			var qtaCNT = AggiungiNumerico(titolo);
			qtaCNT.SetMinMax(0, 10000, 2);
			return qtaCNT;
		}

		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~SettingBaseView()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		// The bulk of the clean-up code is implemented in Dispose(bool)
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				flowLayoutPanel1.Dispose();
			}

			// free native resources if there are any.
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.SuspendLayout();
			//
			// flowLayoutPanel1
			//
			this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(1058, 490);
			this.flowLayoutPanel1.TabIndex = 11;
			//
			// FattureRigheDetailView
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.Controls.Add(this.flowLayoutPanel1);
			this.DoubleBuffered = true;
			this.Name = "MittenteFatturaView";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ResumeLayout(false);
		}

		public MenuTab GetMenu()
		{
			if (_menuTab == null)
			{
				_menuTab = new MenuTab();

				AggiungiComandi();
			}
			return _menuTab;
		}

		private MenuTab _menuTab = null;
		private void AggiungiComandi()
		{
			var tabArticoli = _menuTab.Add("Principale");
			var panelComandiArticoli = tabArticoli.Add("Comandi");
			var rib1 = panelComandiArticoli.Add("Salva", Properties.Resources.Save);

			rib1.Click += (a, e) =>
			{
				if (OnSave != null)
					OnSave(this, new EventArgs());
			};

		}
		public event EventHandler<EventArgs> OnSave;
		

		//private void Salva()
		//{
		//	using (var cur = new CursorManager())
		//	{
		//		this.Validate();
		//		var setting = controller.ReadSetting();
		//		setting.datiMittente = _datiMittente;
		//		controller.SaveSetting(setting);

		//		MessageManager.NotificaInfo(MessageManager.GetMessage(Core.Controllers.enSaveOperation.OpSave));
		//	}
		//}
	}
}