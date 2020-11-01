using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace test
{
    public partial class ShowTODO : UserControl
    {
        Form1 form;
        int userID;

        OracleDataAdapter adapter;
        OracleCommandBuilder builder;
        DataSet ds;
        DataSet ds1;
        string conStr = "Data Source=orcl; User Id=scott;Password=tiger;";
        string cmdStr = "";
        public ShowTODO(Form1 f)
        {
            InitializeComponent();
            form = f;
            userID = form.userID;

            if  (userID != -1)
            {
                //cmdStr = "select max(dl_id) from dolist "; 
                //adapter = new OracleDataAdapter(cmdStr, conStr);
                //ds = new DataSet();
                //adapter.Fill(ds);
                //int id = Convert.ToInt32( ds.Tables[0].Rows[0][0]);
                //MessageBox.Show(id.ToString());

                cmdStr = "select * from dolist";
                adapter = new OracleDataAdapter(cmdStr, conStr);
                ds1 = new DataSet();
                adapter.Fill(ds1);
                dataGridView2.DataSource = ds1.Tables[0];
                comboBox1.Items.Add("All");
                comboBox1.Items.Add("MY TODO ONLY");


                cmdStr = "select * from dolist where userid = " + userID ;
                adapter = new OracleDataAdapter(cmdStr, conStr);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //   select* from dolist_options , dolist d where dolistid = 3 and d.userid = 1
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            builder = new OracleCommandBuilder(adapter);
            adapter.Update(ds.Tables[0]);
            MessageBox.Show("Done");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("All"))
            {
                dataGridView2.DataSource = ds1.Tables[0];
            }
            else
            {
                dataGridView2.DataSource = ds.Tables[0];
            }
        }
    }
}
