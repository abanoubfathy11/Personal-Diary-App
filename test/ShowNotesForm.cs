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
using Tulpep.NotificationWindow;

namespace test
{
    public partial class ShowNotesForm : UserControl
    {
        Form1 ths;
        string userName;
        int userID;
        public ShowNotesForm(Form1 form)
        {
            InitializeComponent();
            ths = form;
            userName = ths.userName.Text.ToString();
            userID = ths.userID;
            list_text.Items.Clear();

            string ordb = "Data source=orcl;User Id=scott; Password=tiger;";
            OracleConnection conn_3 = new OracleConnection("Data Source=ORCL;User Id=scott;password=tiger;");
            conn_3 = new OracleConnection(ordb);
            conn_3.Open();

            OracleCommand c_3 = new OracleCommand();
            c_3.Connection = conn_3;
            c_3.CommandText = "select TEXT from NOTE where USERID=:id";
            c_3.CommandType = CommandType.Text;
            c_3.Parameters.Add("id", userID);
            OracleDataReader dr = c_3.ExecuteReader();
            while (dr.Read())
            {
                list_text.Items.Add(dr[0].ToString());
                //list_text.Items.Add(dr[1].ToString());
                //txt_Gender.Text = dr[1].ToString();
            }
            dr.Close();

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {

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
            ShowNoteForm s = new ShowNoteForm(this, ths, "insert");
            addcontroltopanel(s);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            ShowNoteForm s = new ShowNoteForm(this, ths, "update");
            addcontroltopanel(s);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            

            if (list_text.SelectedIndex != -1)
            {
                int newID_now_2 = 0;
                // int useid = 0;
                string ordb = "Data source=orcl;User Id=scott; Password=tiger;";
                OracleConnection conn_4 = new OracleConnection("Data Source=ORCL;User Id=scott;password=tiger;");
                conn_4 = new OracleConnection(ordb);
                conn_4.Open();
                OracleCommand c_2 = new OracleCommand();
                c_2.Connection = conn_4;
                c_2.CommandText = "GETIDNOW";
                c_2.CommandType = CommandType.StoredProcedure;


                c_2.Parameters.Add("var", OracleDbType.Int32, ParameterDirection.Output);
                // c_2.Parameters.Add("text_now", OracleDbType.NVarchar2,ParameterDirection.Input);

                c_2.Parameters.Add("text_now", list_text.Text);


                // lesa msh bya2raa el node_id 


                c_2.ExecuteNonQuery();
                try
                {

                    newID_now_2 = Convert.ToInt32(c_2.Parameters["var"].Value.ToString());


                }

                catch { newID_now_2 = 1; }

                try
                {
                    //a = userID;

                    //b = newID_now_2;



                    connection delete = new connection();
                    if (MessageBox.Show("Are you sure to delete", "Comfirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        delete.DELETING_PROC(userID, newID_now_2);
                        list_text.Items.RemoveAt(list_text.SelectedIndex);
                        if (ths.bunifuCustomLabel4.Text != "1")
                            ths.bunifuCustomLabel4.Text = (Convert.ToInt32(ths.bunifuCustomLabel4.Text) - 1).ToString();
                        else ths.bunifuCustomLabel4.Text = "0";
                    }






                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Personal Dairy Application";
                popup.ContentText = "Select a note which you want to delete it";
                popup.Image = Properties.Resources.icons8_Notification_32px;
                popup.Popup();
            }



        }
    }
}
