using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UpdaterApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


        }



        private void Form1_Load(object sender, EventArgs e)
        {
            using (var checkup = new CheckUpdateFn())
            {
                checkup.UpdateText += (o, i) =>
                {
                    this.Text = i;
                };

                var connString = ConfigurationManager.ConnectionStrings["ModelSm"].ConnectionString;
                using (var sqlConn = new SqlConnection(connString))
                {
                    sqlConn.Open();
                    using (var sqlCommand = new SqlCommand("UPD_CHECKUPDATE", sqlConn))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@pNomePostazione", Environment.MachineName);
                        var val = sqlCommand.ExecuteScalar();
                        if (val.Equals("UPDATED"))
                        {
                            checkup.StartApplication();
                            return;
                        }
                    }
                }


                MessageBox.Show("Controllo aggiornamenti, premere ok e attendere!", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void Checkup_UpdateText(object sender, string e)
        {
            throw new NotImplementedException();
        }
    }
}