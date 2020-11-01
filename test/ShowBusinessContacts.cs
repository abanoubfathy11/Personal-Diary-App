using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Microsoft.VisualBasic;

namespace test
{
    public partial class ShowBusinessContacts : UserControl
    {
        Form1 ths;
        string userName;
        int userID;
        string ordb = "Data source=orcl;User Id=scott; Password=tiger;";
        OracleConnection conn;
        public ShowBusinessContacts( Form1 form)
        {
            InitializeComponent();
            ths = form;
            userName = ths.userName.Text.ToString();
            userID = ths.userID;


            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select bnname from bussines_note where userid=:iid";
            cmd.Parameters.Add("iid", userID);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listBox1.Items.Add(dr[0].ToString());
            }
            dr.Close();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
        private void addcontroltopanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panelcontrol.Controls.Clear();
            panelcontrol.Controls.Add(c);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            ShowBusinessContact s = new ShowBusinessContact(this, ths, "insert");
            addcontroltopanel(s);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ShowBusinessContact s = new ShowBusinessContact(this, ths, "update");
            //addcontroltopanel(s);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                var res = MessageBox.Show("Are You Sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    string selectedNote = listBox1.SelectedItem.ToString(); ;
                    conn = new OracleConnection(ordb);
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select bn_id from bussines_note where bnname=:sname";
                    cmd.Parameters.Add("sname", selectedNote);
                    OracleDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    int noteID = Convert.ToInt32(dr[0]);
                    dr.Close();

                    OracleCommand c = new OracleCommand();
                    c.Connection = conn;
                    c.CommandText = "DeleteBussinesNote";
                    c.CommandType = CommandType.StoredProcedure;
                    c.Parameters.Add("nid", noteID);
                    int d = c.ExecuteNonQuery();
                    listBox1.Items.Remove(selectedNote);

                    PopupNotifier popup = new PopupNotifier();
                    popup.TitleText = "Bussines Note has Been Deleted Successfully";
                    popup.Popup();
                    if (ths.bunifuCustomLabel5.Text != "1")
                        ths.bunifuCustomLabel5.Text = (Convert.ToInt32(ths.bunifuCustomLabel5.Text) - 1).ToString();
                    else ths.bunifuCustomLabel5.Text = "0";
                }
            }
            else
            {
                MessageBox.Show("No Selected Bussines Note to Delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                ShowBusinessContact s = new ShowBusinessContact(this, ths, "update");
                addcontroltopanel(s);

            }
            else
            {
                MessageBox.Show("No Selected Bussines Note to Show", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
