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
using Tulpep.NotificationWindow;
using Oracle.DataAccess.Types;

namespace test
{
    public partial class ShowNoteForm : UserControl
    {
        ShowNotesForm ths;
        string option;
        string selectedItem;
        string userName;
        int userID;
        Form1 form1;
        public ShowNoteForm(ShowNotesForm form, Form1 f, string o)
        {
            InitializeComponent();
            ths = form;
            form1 = f;
            if (ths.list_text.SelectedIndex > -1)
            {
                selectedItem = ths.list_text.SelectedItem.ToString();
                textBox1.Text = selectedItem;
            }
            userName = f.userName.Text.ToString();
            userID = f.userID;
            option = o;
        }

        private void ShowNoteForm_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
        private void addcontroltopanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panelcontrol.Controls.Clear();
            panelcontrol.Controls.Add(c);
        }
        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            ShowNotesForm s = new ShowNotesForm(form1);
            addcontroltopanel(s);
        }

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            if (option.Equals("insert") && !textBox1.Text.Equals(""))
            {
                // MessageBox.Show("insert" + userID);
                int newID = 0;
                // int useid = 0;
                string ordb = "Data source=orcl;User Id=scott; Password=tiger;";
                OracleConnection conn_2 = new OracleConnection("Data Source=ORCL;User Id=scott;password=tiger;");
                conn_2 = new OracleConnection(ordb);
                conn_2.Open();
                OracleCommand c_2 = new OracleCommand();
                c_2.Connection = conn_2;
                c_2.CommandText = "GETNOTID";
                c_2.CommandType = CommandType.StoredProcedure;
                c_2.Parameters.Add("newID", OracleDbType.Int32, ParameterDirection.Output);
                c_2.ExecuteNonQuery();
                try
                {

                    newID = Convert.ToInt32(c_2.Parameters["newID"].Value.ToString());
                    newID += 1;


                }

                catch { newID = 1; }
                

                OracleCommand c = new OracleCommand();
                c.Connection = conn_2;
                c.CommandText = "insert into NOTE(NOTE_ID,TEXT,EDIT_DATE,IMAGE,INSERT_DATE,USERID) values (:note_id_2,:new_text,:date_edite,:image,:insert_date,:userID)";

                c.Parameters.Add("note_id_2 ", newID);
                c.Parameters.Add("new_text", textBox1.Text);
                c.Parameters.Add("date_edite", System.DateTime.Now);
                c.Parameters.Add("image", null);
                c.Parameters.Add("insert_date", System.DateTime.Now);
                c.Parameters.Add("userID ", userID);

                int test = c.ExecuteNonQuery();

                if (test != -1)
                {

                    //MessageBox.Show("insert sucecc" + userID);
                    PopupNotifier popup = new PopupNotifier();
                    popup.TitleText = "Personal Dairy Application";
                    popup.ContentText = "Note Recorded :)";
                    popup.Image = Properties.Resources.icons8_Notification_32px;
                    popup.Popup();
                    form1.bunifuCustomLabel4.Text = (Convert.ToInt32(form1.bunifuCustomLabel4.Text) + 1).ToString();
                    ShowNotesForm s = new ShowNotesForm(form1);
                    addcontroltopanel(s);
                }
            }
            else if (option.Equals("update") && !textBox1.Text.Equals(""))
            {
                //MessageBox.Show("update" + userID);
                int newID_now = 0;
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

                c_2.Parameters.Add("text_now", selectedItem);


                // lesa msh bya2raa el node_id 


                c_2.ExecuteNonQuery();
                try
                {

                    newID_now = Convert.ToInt32(c_2.Parameters["var"].Value.ToString());


                }

                catch { newID_now = 1; }


                ordb = "Data source=orcl;User Id=scott; Password=tiger;";
                OracleConnection conn_5 = new OracleConnection("Data Source=ORCL;User Id=scott;password=tiger;");
                conn_5 = new OracleConnection(ordb);
                conn_5.Open();

                OracleCommand c_6 = new OracleCommand();
                c_6.Connection = conn_5;
                c_6.CommandText = "update NOTE set  TEXT=:new_text , EDIT_DATE=:date_edite,IMAGE=:image  where USERID =:userID and NOTE_ID=:note_id ";

                c_6.Parameters.Add("new_text", textBox1.Text);
                c_6.Parameters.Add("date_edite", System.DateTime.Now);
                c_6.Parameters.Add("image", null);
                //c_6.Parameters.Add("insert_date", null);
                c_6.Parameters.Add("userID ", userID);
                c_6.Parameters.Add("note_id ", newID_now);

                int r = c_6.ExecuteNonQuery();
                if (r != -1)

                {

                    //MessageBox.Show("Text modified");
                    PopupNotifier popup = new PopupNotifier();
                    popup.TitleText = "Personal Dairy Application";
                    popup.ContentText = "Note Modified :)";
                    popup.Image = Properties.Resources.icons8_Notification_32px;
                    popup.Popup();
                    ShowNotesForm s = new ShowNotesForm(form1);
                    addcontroltopanel(s);
                }

            }
            else
            {
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Personal Dairy Application";
                popup.ContentText = "Text Empty !!!!! you should write a note or delete it";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }
            
        }
    }
}
