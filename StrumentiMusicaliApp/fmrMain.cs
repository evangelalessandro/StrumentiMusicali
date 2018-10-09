using StrumentiMusicaliSql.Core;
using StrumentiMusicaliSql.Repo;
using SturmentiMusicaliApp.Core;
using SturmentiMusicaliApp.Core.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SturmentiMusicaliApp
{
    public partial class fmrMain : Form
    {
        public fmrMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
			using (var uof = new UnitOfWork())
			{ 
				var list = uof.ArticoliRepository.Find(a=>1==1).Select(a=>new ArticoloItem { ID= a.ID,Titolo= a.Titolo,ArticoloCS=a,DataCreazione=a.DataCreazione,DataModifica=a.DataUltimaModifica,Pinned=a.Pinned }).ToList();
				dgvMaster.DataSource = list;
			}

			EventAggregator.Instance().Subscribe<ArticoloAdd>(AggiungiArticolo);
		}

		private void ribAddArt_Click(object sender, EventArgs e)
		{
			EventAggregator.Instance().Publish(new ArticoloAdd());
		}
		private void AggiungiArticolo(ArticoloAdd articoloAdd)
		{
			using (var frm=new Forms.frmArticolo())
			{
				frm.ShowDialog();
			}
		}
		
	}
}
