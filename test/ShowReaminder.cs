using System;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace test
{
    public partial class ShowReaminder : UserControl
    {
        ShowRemainders ths;
        string option;
        string selectedItem;
        string userName;
        int userID;
        Form1 form1;
        string ordb = "Data source=orcl;User Id=scott; Password=tiger;";
        OracleConnection conn;
        public ShowReaminder(ShowRemainders form,Form1 f,string o)
        {
            InitializeComponent();
            ths = form;
            form1 = f;
            if (ths.listBox1.SelectedIndex > -1)
            {
                selectedItem = ths.listBox1.SelectedItem.ToString();
                textBox1.Text = selectedItem;
                selectedItem = ths.listBox1.SelectedItem.ToString();
                conn = new OracleConnection(ordb);
                conn.Open();

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select rem_name , text , rem_data from reminder where rem_name = :rem_name";
                cmd.CommandType = CommandType.Text;

                OracleParameter p_rem_name = new OracleParameter();
                p_rem_name.OracleDbType = OracleDbType.Varchar2;
                p_rem_name.Value = selectedItem.ToString();
                cmd.Parameters.Add(p_rem_name);
                OracleDataReader dr = cmd.ExecuteReader();

                dr.Read();

                textBox1.Text = dr[0].ToString();
                dateTimePicker1.Text = dr[2].ToString();
                textBox4.Text = dr[1].ToString();


                dr.Close();

            }
            else if (ths.listBox1.SelectedItem != null)
            {
                MessageBox.Show("No selected reminder to show ");
            }

            userName = f.userName.Text.ToString();
            userID = f.userID;
            option = o;
        }
        private void addcontroltopanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panelcontrol.Controls.Clear();
            panelcontrol.Controls.Add(c);
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (option.Equals("update"))
            {
                if (textBox1.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    ShowRemainders sd = new ShowRemainders(form1);
                    string listname = selectedItem;
                    sd.listBox1.Items.Remove(selectedItem);
                    if (sd.listBox1.FindString(textBox1.Text) == -1)
                    {
                        int remid = 0;
                        OracleCommand c = new OracleCommand();
                        c.Connection = conn;
                        c.CommandText = "select rem_id from reminder where rem_name=:rrname and userid=:uuid";
                        c.CommandType = CommandType.Text;
                        c.Parameters.Add("rrname", selectedItem);
                        c.Parameters.Add("uuid", userID);

                        OracleDataReader dr = c.ExecuteReader();
                        while (dr.Read())
                        {
                            remid = Convert.ToInt32(dr[0]);
                        }

                        OracleCommand cc = new OracleCommand();
                        cc.Connection = conn;
                        cc.CommandText = "update reminder set rem_name=:rrname, text=:rrtext, rem_data=:rrdate  where rem_id=:rrid";
                        cc.CommandType = CommandType.Text;
                        cc.Parameters.Add("rrname", textBox1.Text);
                        cc.Parameters.Add("rrtext", textBox4.Text);
                        cc.Parameters.Add("rrdate", dateTimePicker1.Value);
                        cc.Parameters.Add("rrid", remid);
                        int r = cc.ExecuteNonQuery();

                        if (r != -1)
                        {
                            PopupNotifier popup = new PopupNotifier();
                            popup.TitleText = "Note has Been Updated Successfully";
                            popup.Popup();

                            MessageBox.Show("Reminder Note Updated", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // MessageBox.Show("update" + userID);
                            ShowRemainders s = new ShowRemainders(form1);
                            addcontroltopanel(s);
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Reminder Name is Already ", "Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            else if (option.Equals("insert"))
            {
                if (textBox1.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string useName = "";
                    int newid = 0;
                    int useID = 0;
                    conn = new OracleConnection(ordb);
                    conn.Open();
                    OracleCommand action = new OracleCommand();
                    action.Connection = conn;
                    action.CommandText = "GetRemID";
                    action.CommandType = CommandType.StoredProcedure;
                    action.Parameters.Add("newid", OracleDbType.Int32, ParameterDirection.Output);
                    action.ExecuteNonQuery();

                    try
                    {
                        newid = Convert.ToInt32(action.Parameters["newid"].Value.ToString());
                        newid += 1;
                    }
                    catch { newid = 1; }

                    OracleCommand rem_name = new OracleCommand();
                    rem_name.Connection = conn;
                    rem_name.CommandText = "select rem_name ,rem_id from reminder where rem_name= :name and userid= :userid";
                    rem_name.CommandType = CommandType.Text;

                    rem_name.Parameters.Add("name", textBox1.Text);
                    rem_name.Parameters.Add("userid", userID);

                    OracleDataReader drty = rem_name.ExecuteReader();
                    while (drty.Read())
                    {
                        useName = drty[0].ToString();
                        useID = Convert.ToInt32(drty[1]);

                    }
                    drty.Close();
                    if (useName == "")
                    {
                        OracleCommand cmdd = new OracleCommand();
                        cmdd.Connection = conn;
                        cmdd.CommandText = "insert into reminder values (:nid,:nname,:ntext,:nimage,:ndate,:nuserid)";
                        cmdd.CommandType = CommandType.Text;

                        cmdd.Parameters.Add("nid", Convert.ToInt32(newid));
                        cmdd.Parameters.Add("nname", textBox1.Text);
                        cmdd.Parameters.Add("ntext", textBox4.Text);
                        cmdd.Parameters.Add("nimage", "");
                        cmdd.Parameters.Add("ndate", dateTimePicker1.Value);
                        cmdd.Parameters.Add("nuserid", userID);

                        int r = cmdd.ExecuteNonQuery();


                        if (r != -1)
                        {
                            PopupNotifier popup = new PopupNotifier();
                            popup.TitleText = "Note has Been Added Successfully";
                            popup.Popup();
                            MessageBox.Show("Reminder Note added", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // MessageBox.Show("insert" + userID);
                            form1.bunifuCustomLabel7.Text = (Convert.ToInt32(form1.bunifuCustomLabel7.Text) + 1).ToString();
                            ShowRemainders s = new ShowRemainders(form1);
                            addcontroltopanel(s);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Reminder Name is Already Exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                //MessageBox.Show("insert" + userID);
                //ShowRemainders s = new ShowRemainders(form1);
                //addcontroltopanel(s);
            }
        }

        private void pictureBox7_Click_1(object sender, EventArgs e)
        {
            ShowRemainders s = new ShowRemainders(form1);
            addcontroltopanel(s);
        }
    }
}
