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

namespace test
{
    public partial class ShowRemainders : UserControl
    {
        Form1 ths;
        string userName;
        int userID;
        string ordb = "Data source=orcl;User Id=scott; Password=tiger;";
        OracleConnection conn;
        public ShowRemainders(Form1 form )
        {
            InitializeComponent();
            ths = form;
            userName = ths.userName.Text.ToString();
            userID = ths.userID;
            conn = new OracleConnection(ordb);
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select rem_name from reminder  where userid=:id";

            cmd.CommandType = CommandType.Text;
            OracleParameter p_userid = new OracleParameter();
            p_userid.OracleDbType = OracleDbType.Int32;
            p_userid.Value = userID;
            cmd.Parameters.Add(p_userid);
            OracleDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                listBox1.Items.Add(dr[0]);

            }
            dr.Close();
        }
        private void addcontroltopanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panelcontrol.Controls.Clear();
            panelcontrol.Controls.Add(c);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ShowReaminder s = new ShowReaminder(this, ths, "insert");
            addcontroltopanel(s);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // ShowReaminder s = new ShowReaminder(this, ths, "update");
            //addcontroltopanel(s);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            if (listBox1.SelectedItem != null)
            {
                var res = MessageBox.Show("Are You Sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    string rem_name_delete = listBox1.SelectedItem.ToString();
                    conn = new OracleConnection(ordb);
                    conn.Open();

                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "delete from reminder  where rem_name=:rem_name_delete";
                    cmd.CommandType = CommandType.Text;
                    OracleParameter p_rem_name_delete = new OracleParameter();
                    p_rem_name_delete.OracleDbType = OracleDbType.Varchar2;
                    p_rem_name_delete.Value = rem_name_delete.ToString();
                    cmd.Parameters.Add(p_rem_name_delete);
                    OracleDataReader dr = cmd.ExecuteReader();
                    dr.Read();

                    listBox1.Items.Remove(rem_name_delete);
                    conn.Close();

                    PopupNotifier popup = new PopupNotifier();
                    popup.TitleText = "Note has Been Deleted Successfully";
                    popup.Popup();
                    //MessageBox.Show("done");
                    if (ths.bunifuCustomLabel7.Text != "1")
                        ths.bunifuCustomLabel7.Text = (Convert.ToInt32(ths.bunifuCustomLabel7.Text) - 1).ToString();
                    else ths.bunifuCustomLabel7.Text = "0";
                }

            }
            else
            {
                MessageBox.Show("nothing selected to delete ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                ShowReaminder s = new ShowReaminder(this, ths, "update");
                addcontroltopanel(s);
                //MessageBox.Show(rem_name);
                //conn = new OracleConnection(ordb);
                //conn.Open();

                //OracleCommand cmd = new OracleCommand();
                //cmd.Connection = conn;
                //cmd.CommandText = "select rem_name , text , rem_data from reminder where rem_name = :rem_name";
                //cmd.CommandType = CommandType.Text;

                //OracleParameter p_rem_name = new OracleParameter();
                //p_rem_name.OracleDbType = OracleDbType.Varchar2;
                //p_rem_name.Value = rem_name.ToString();
                //cmd.Parameters.Add(p_rem_name);      
                //OracleDataReader dr = cmd.ExecuteReader();

                // dr.Read();

                //    MessageBox.Show(dr[0].ToString());
                //    MessageBox.Show(dr[1].ToString());
                //    MessageBox.Show(dr[2].ToString());


                //dr.Close();
            }
            else
            {
                MessageBox.Show("No Selected Reminder to Show", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
