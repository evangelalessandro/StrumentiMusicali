using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Events.Generics;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.BaseControl.ElementiDettaglio;
using StrumentiMusicali.Library.Core;
using StrumentiMusicali.Library.Entity;
using StrumentiMusicali.Library.Repo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace StrumentiMusicali.App.View.FattureRighe
{
	internal class FattureRigheDetailView : BaseDataControl
	{
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

		public FattureRigheDetailView(ControllerRigheFatture controllerRigheFatture)
			: base()
		{
			InitializeComponent();
			using (var uof = new UnitOfWork())
			{


				EDTesto artCNT = AggiungiTesto("Articolo");
				artCNT.Width = 40;

				var descCNT = AggiungiTesto("Descrizione",
					null
					);
				descCNT.Width = 140;

				var qtaCNT = AggiungiNumerico("Quantità");

				var prezzoCNT = AggiungiNumericoDecimal("Prezzo");
				prezzoCNT.Width = 80;

				var importo = AggiungiNumericoDecimal("Importo");
				importo.Width = 80;
				importo.Enabled = false;

				var ivaCNT = AggiungiTesto("Iva",
					uof.FattureRigheRepository.Find(a => 1 == 1).Where(a => a.IvaApplicata != "").Select(a => a.IvaApplicata)
					.Distinct().ToList());
				ivaCNT.Width = 30;





				Action action = new Action(() =>
				{
					if (this.IsDisposed)
						return;
					descCNT.BindProprieta("Descrizione", controllerRigheFatture.SelectedItem);
					ivaCNT.BindProprieta("IvaApplicata", controllerRigheFatture.SelectedItem);
					artCNT.BindProprieta("CodiceArticoloOld", controllerRigheFatture.SelectedItem);
					qtaCNT.BindProprieta("Qta", controllerRigheFatture.SelectedItem);

					prezzoCNT.BindProprieta("PrezzoUnitario", controllerRigheFatture.SelectedItem);
					importo.BindProprieta("Importo", controllerRigheFatture.SelectedItem);

				});

				action.BeginInvoke(null, null);
				(controllerRigheFatture as INotifyPropertyChanged).PropertyChanged += (a, b) =>
				{
					action.BeginInvoke(null, null);

				};
				_subcribe = EventAggregator.Instance().Subscribe<ItemSelected<FatturaRigaItem, FatturaRiga>>((a) =>
				{
					action.BeginInvoke(null, null);

				}
				);
			}
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
			qtaCNT.Width = 40;
			qtaCNT.SetMinMax(0, 10000, 0);
			return qtaCNT;
		}
		private EDNumeric AggiungiNumericoDecimal(string titolo)
		{
			var qtaCNT = AggiungiNumerico(titolo);
			qtaCNT.SetMinMax(0, 10000, 2);
			return qtaCNT;
		}

		private Subscription<ItemSelected<FatturaRigaItem, FatturaRiga>> _subcribe;
		// NOTE: Leave out the finalizer altogether if this class doesn't
		// own unmanaged resources, but leave the other methods
		// exactly as they are.
		~FattureRigheDetailView()
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

			EventAggregator.Instance().UnSbscribe(_subcribe);
			// free native resources if there are any.
			base.Dispose(disposing);
		}

		private void BindProp()
		{

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
			this.Name = "FattureRigheDetailView";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ResumeLayout(false);

		}
	}
}
