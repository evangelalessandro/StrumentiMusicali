using StrumentiMusicaliSql.Repo;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var uof = new UnitOfWork();
            var list= uof.CategorieRepository.Find(a => a.Reparto.StartsWith("C")).ToList();
            dataGridView1.DataSource = list;
        }
    }
}
