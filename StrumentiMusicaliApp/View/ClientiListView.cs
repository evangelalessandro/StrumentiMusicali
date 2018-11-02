﻿using StrumentiMusicali.App.Core.Controllers;
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
		 
		public ClientiListView(ControllerClienti controller)
			: base(controller)
		{
			this.Load += control_Load;

		 	
		 
		}
		private void control_Load(object sender, System.EventArgs e)
		{
		 
			
			dgvRighe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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
		
		
		public new void Dispose()
		{

			base.Dispose();
			
		}
	}
	
}