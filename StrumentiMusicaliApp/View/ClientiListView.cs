﻿using StrumentiMusicali.App.Core.Controllers;
using StrumentiMusicali.App.Core.Item;
using StrumentiMusicali.App.View.BaseControl;
using StrumentiMusicali.App.View.Interfaces;
using StrumentiMusicali.Library.Core.Item;
using StrumentiMusicali.Library.Entity;
using System;
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
		 
			//dgvRighe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}

		public override void FormatGrid()
		{
			dgvRighe.Columns["Entity"].Visible = false;
			dgvRighe.BestFitColumns(true);
			dgvRighe.Columns["ID"].VisibleIndex = 0;
		}
		
		
		public new void Dispose()
		{

			base.Dispose();
			
		}
	}
	
}