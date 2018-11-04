using StrumentiMusicali.App.Core;
using StrumentiMusicali.App.Core.Controllers.Base;
using StrumentiMusicali.App.Core.Controllers.FatturaElett;
using StrumentiMusicali.App.Core.Manager;
using StrumentiMusicali.App.Core.MenuRibbon;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.BaseControl.ElementiDettaglio;
using StrumentiMusicali.Library.Core;
using System;
using System.Linq;

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

			this.Paint += SettingBaseView_Paint;
		}

		private void SettingBaseView_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			var graphics = e.Graphics;
			var gradient_rectangle = new System.Drawing.Rectangle(0, 0, this.Width, this.Height);
			//System.Drawing.Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(gradient_rectangle, 
			//	System.Drawing.Color.DeepSkyBlue,
			//	System.Drawing.Color.CornflowerBlue, 65f);
			System.Drawing.Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(gradient_rectangle, System.Drawing.Color.AliceBlue,
				System.Drawing.Color.LightBlue, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);

			graphics.FillRectangle(b, gradient_rectangle);
		}

		public void BindProp(object objToBind, string prefixText)
		{
			foreach (var item in Utility.UtilityView.GetProperties(objToBind).OrderBy(a=>
			{
				var sel = (CustomUIViewAttribute)a.GetCustomAttributes(typeof(CustomUIViewAttribute), true).FirstOrDefault();
				if (sel==null || sel.Ordine==0 )
				{
					return 100;
				}
				return sel.Ordine;
			}
			))
			{
				var hideAttr=item.CustomAttributes.Where(a => a.AttributeType == typeof(CustomHideUIAttribute)).FirstOrDefault();
				if (hideAttr!=null)
					continue;
				 
				var widthAttr = (CustomUIViewAttribute)item.GetCustomAttributes(typeof(CustomUIViewAttribute),true).FirstOrDefault();
					

				var titolo = string.Join(" ", Regex.Split(item.Name, @"(?<!^)(?=[A-Z])"));
				if (item.Name.CompareTo(item.Name.ToUpperInvariant()) == 0)
				{
					titolo = item.Name;
				}
				titolo = prefixText + titolo;
				EDBase newControl = null;
				if (item.PropertyType.FullName.Contains("String"))
				{
					newControl  = AggiungiTesto(titolo);
					newControl.Width = 250;
					
				}
				else if (item.PropertyType.FullName.Contains("Boolean"))
				{
					newControl = AggiungiCheck(titolo);
					 
				}
				else if (item.PropertyType.FullName.Contains("Decimal"))
				{
					newControl= AggiungiNumericoDecimal(titolo);
					 
				}
				else if (item.PropertyType.FullName.Contains("Int32"))
				{
					newControl= AggiungiNumerico(titolo);
					
				}
				else if (item.PropertyType.FullName.Contains("DateTime"))
				{
					newControl= AggiungiDateTime(titolo, widthAttr);
					
				}
				else
				{
					if (item!=null && (!item.PropertyType.Name.StartsWith("System.")
						&&
						!item.PropertyType.FullName.Contains("Library.Entity")
						))
					{

						BindProp(item.GetValue(objToBind), "[" + titolo + "]  ");
					}
					else
					{

					}
				}
				if (newControl!=null)
				{
					BindObj(item, newControl, objToBind, widthAttr);
				}
			}
		}

		private void BindObj(System.Reflection.PropertyInfo item, EDBase controlBase, object objToBind, CustomUIViewAttribute attribute)
		{
			controlBase.Tag = item.Name;
			controlBase.BindProprieta(attribute,item.Name, objToBind);
			controlBase.Height = 50;
			if (item.Name == "DataCreazione"
				|| item.Name == "DataUltimaModifica"
				|| item.Name == "ID")
			{
				controlBase.Enabled = false;
			}
			if (attribute!=null)
			{
				controlBase.Width = attribute.Width;
			}
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
			qtaCNT.SetMinSize = false;
			if (titolo=="Cap")
			{
				qtaCNT.SetMinMax(0, 9999999, 0);

			}
			else
			{
				qtaCNT.SetMinMax(0, 10000000, 0);

			}
			return qtaCNT;
		}

		private EDNumeric AggiungiNumericoDecimal(string titolo)
		{
			var qtaCNT = AggiungiNumerico(titolo);
			qtaCNT.SetMinMax(0, 10000, 2);
			return qtaCNT;
		}
		private EDDateTime AggiungiDateTime(string titolo, CustomUIViewAttribute widthAttr)
		{
			var qtaCNT = new EDDateTime();
			qtaCNT.Titolo = titolo;
			flowLayoutPanel1.Controls.Add(qtaCNT);
			
			
			return qtaCNT; ;
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
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(1058, 490);
			this.flowLayoutPanel1.TabIndex = 11;
			//
			// FattureRigheDetailView
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.BackColor = System.Drawing.Color.Transparent;
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
			var rib2 = panelComandiArticoli.Add("Salva e chiudi", Properties.Resources.Save_Close);

			rib2.Click += (a, e) =>
			{
				if (OnSave != null)
					OnSave(this, new EventArgs());

				if (OnClose != null)
					OnClose(this, new EventArgs());
			};
			var rib3 = panelComandiArticoli.Add("Chiudi", Properties.Resources.Close_48);

			rib3.Click += (a, e) =>
			{
				if (OnClose != null)
					OnClose(this, new EventArgs());
			};
		}
		public event EventHandler<EventArgs> OnSave;
		public event EventHandler<EventArgs> OnClose;



	}
}