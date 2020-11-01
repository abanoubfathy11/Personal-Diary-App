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
    public partial class ShowBusinessContact : UserControl
    {
        ShowBusinessContacts show;
        Form1 form1;
        string selectedItem;
        string userName;
        int userId;
        string option;
        string ordb = "Data source=orcl;User Id=scott; Password=tiger;";
        OracleConnection conn;
        public ShowBusinessContact(ShowBusinessContacts Show, Form1 f, string o)
        {
            InitializeComponent();
            show = Show;
            form1 = f;
            option = o;
            userId = f.userID;
            userName = f.userName.Text.ToString();
            if (show.listBox1.SelectedIndex > -1)
            {
                selectedItem = show.listBox1.SelectedItem.ToString();

                conn = new OracleConnection(ordb);
                conn.Open();

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;

                cmd.CommandText = "select massage, bnname,ID,bn_id from bussines_note where userid=:iid and bnname=:nname";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("iid", userId);
                cmd.Parameters.Add("nnmae", selectedItem);
                OracleDataReader dr = cmd.ExecuteReader();
                dr.Read();
                textBox4.Text = dr[0].ToString();
                textBox1.Text = dr[1].ToString();
                textBox2.Text = dr[2].ToString();
                int noteID = Convert.ToInt32(dr[3]);
                dr.Close();

                OracleCommand c = new OracleCommand();
                c.Connection = conn;
                c.CommandText = "select bankaccount from bn_bankaccount where bnid=:iid";
                c.CommandType = CommandType.Text;
                c.Parameters.Add("iid", noteID);

                OracleDataReader drr = c.ExecuteReader();
                while (drr.Read())
                {
                    textBox3.Text = drr[0].ToString();

                }
                drr.Close();


                OracleCommand address = new OracleCommand();
                address.Connection = conn;
                address.CommandText = "select address from bn_addresses where bnid=:iid";
                address.CommandType = CommandType.Text;
                address.Parameters.Add("iid", noteID);
                OracleDataReader h = address.ExecuteReader();
                while (h.Read())
                {
                    listBox1.Items.Add(h[0].ToString());
                }
                h.Close();

                OracleCommand phone = new OracleCommand();
                phone.Connection = conn;
                phone.CommandText = "select phone from bn_phones where bnid=:iid";
                phone.CommandType = CommandType.Text;
                phone.Parameters.Add("iid", noteID);
                OracleDataReader phone1 = phone.ExecuteReader();
                while (phone1.Read())
                {
                    listBox2.Items.Add(phone1[0].ToString());
                }
                phone1.Close();


                OracleCommand email = new OracleCommand();
                email.Connection = conn;
                email.CommandText = "select email from bn_emails where bnid=:iid";
                email.CommandType = CommandType.Text;
                email.Parameters.Add("iid", noteID);
                OracleDataReader email1 = email.ExecuteReader();
                while (email1.Read())
                {
                    listBox3.Items.Add(email1[0].ToString());
                }
                email1.Close();
            }
            else if (show.listBox1.SelectedItem != null)
            {
                MessageBox.Show("No selected reminder to show ");
            }
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

        private void pictureBox7_Click_1(object sender, EventArgs e)
        {
            ShowBusinessContacts s = new ShowBusinessContacts(form1);
            addcontroltopanel(s);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ShowBusinessContacts s = new ShowBusinessContacts(form1);
            if (option.Equals("update"))
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || listBox1.Items.Count < 1 || listBox2.Items.Count < 1 || listBox3.Items.Count < 1)
                {
                    MessageBox.Show("Invalid Input", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    try
                    {
                        string noteName = selectedItem;
                        conn = new OracleConnection(ordb);
                        conn.Open();


                        OracleCommand action = new OracleCommand();
                        action.Connection = conn;
                        action.CommandText = "select bn_id from bussines_note where bnname=:noteName";
                        action.CommandType = CommandType.Text;
                        action.Parameters.Add("noteName", noteName);
                        OracleDataReader drr = action.ExecuteReader();


                        drr.Read();
                        int noteID = Convert.ToInt32(drr[0]);
                        drr.Close();


                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "UpadetBussinesNote";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("massagee", textBox4.Text.ToString());
                        cmd.Parameters.Add("bnnamee", textBox1.Text.ToString());
                        cmd.Parameters.Add("idd", Convert.ToInt32(textBox2.Text));
                        cmd.Parameters.Add("bnid", noteID);
                        cmd.ExecuteNonQuery();

                        OracleCommand bankAcc = new OracleCommand();
                        bankAcc.Connection = conn;
                        bankAcc.CommandText = "update bn_bankaccount set bankaccount=:bnba where bnid=:niid";
                        bankAcc.CommandType = CommandType.Text;
                        bankAcc.Parameters.Add("bnba", Convert.ToInt32(textBox3.Text));
                        bankAcc.Parameters.Add("niid", noteID);
                        bankAcc.ExecuteNonQuery();

                        OracleCommand addressDelete = new OracleCommand();
                        addressDelete.Connection = conn;
                        addressDelete.CommandText = "delete from bn_addresses where bnid=:niid";
                        addressDelete.CommandType = CommandType.Text;
                        addressDelete.Parameters.Add("niid", noteID);
                        addressDelete.ExecuteNonQuery();

                        OracleCommand phoneDelete = new OracleCommand();
                        phoneDelete.Connection = conn;
                        phoneDelete.CommandText = "delete from bn_phones where bnid=:niid";
                        phoneDelete.CommandType = CommandType.Text;
                        phoneDelete.Parameters.Add("niid", noteID);
                        phoneDelete.ExecuteNonQuery();

                        OracleCommand emailDelete = new OracleCommand();
                        emailDelete.Connection = conn;
                        emailDelete.CommandText = "delete from bn_emails where bnid=:niid";
                        emailDelete.CommandType = CommandType.Text;
                        emailDelete.Parameters.Add("niid", noteID);
                        emailDelete.ExecuteNonQuery();

                        for (int i = 0; i < listBox1.Items.Count; i++)
                        {
                            OracleCommand bnAdreess = new OracleCommand();
                            bnAdreess.Connection = conn;
                            bnAdreess.CommandText = "insert into bn_addresses values (:bnaddrees,:noteid)";
                            bnAdreess.CommandType = CommandType.Text;
                            bnAdreess.Parameters.Add("bnaddrees", listBox1.Items[i].ToString());
                            bnAdreess.Parameters.Add("noteid", noteID);
                            bnAdreess.ExecuteNonQuery();

                        }

                        for (int i = 0; i < listBox2.Items.Count; i++)
                        {
                            OracleCommand bnPhones = new OracleCommand();
                            bnPhones.Connection = conn;
                            bnPhones.CommandText = "insert into bn_phones values (:bnphone,:noteid)";
                            bnPhones.CommandType = CommandType.Text;
                            bnPhones.Parameters.Add("bnphone", Convert.ToInt32(listBox2.Items[i]));
                            bnPhones.Parameters.Add("noteid", noteID);
                            bnPhones.ExecuteNonQuery();

                        }

                        for (int i = 0; i < listBox3.Items.Count; i++)
                        {
                            OracleCommand bnEmail = new OracleCommand();
                            bnEmail.Connection = conn;
                            bnEmail.CommandText = "insert into bn_emails values (:bnemail,:noteid)";
                            bnEmail.CommandType = CommandType.Text;
                            bnEmail.Parameters.Add("bnemail", listBox3.Items[i].ToString());
                            bnEmail.Parameters.Add("noteid", noteID);
                            bnEmail.ExecuteNonQuery();

                        }
                        PopupNotifier popup = new PopupNotifier();
                        popup.TitleText = "Bussines Note Updated Successfully";
                        popup.Popup();
                        s.listBox1.Items.Remove(selectedItem);
                        s.listBox1.Items.Add(textBox1.Text);
                        addcontroltopanel(s);
                    }
                    catch
                    {
                        MessageBox.Show("invalid input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }





            }
            else if (option.Equals("insert"))
            {

                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || listBox1.Items.Count < 1 || listBox2.Items.Count < 1 || listBox3.Items.Count < 1)
                {
                    MessageBox.Show("Invalid Input", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string noteName = textBox1.Text.ToString();

                    int newid = 0;
                    conn = new OracleConnection(ordb);
                    conn.Open();
                    OracleCommand action = new OracleCommand();
                    action.Connection = conn;
                    action.CommandText = "GetBNID";
                    action.CommandType = CommandType.StoredProcedure;
                    action.Parameters.Add("newid", OracleDbType.Int32, ParameterDirection.Output);
                    action.ExecuteNonQuery();

                    try
                    {
                        newid = Convert.ToInt32(action.Parameters["newid"].Value.ToString());
                        newid += 1;
                    }
                    catch { newid = 1; }
                    bool isExit = false;
                    for (int i = 0; i < s.listBox1.Items.Count; i++)
                    {
                        if (s.listBox1.Items[i].Equals(noteName))
                            isExit = true;

                    }
                    if (isExit)
                    {
                        MessageBox.Show("Name is Already Exist", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        try
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = conn;
                            cmd.CommandText = "CreateBussinesNote";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("bn_idd", newid);
                            cmd.Parameters.Add("massagee", textBox4.Text);
                            cmd.Parameters.Add("bnnamee", textBox1.Text);
                            cmd.Parameters.Add("useridd", userId);
                            cmd.Parameters.Add("idd", Convert.ToInt32(textBox2.Text));
                            cmd.ExecuteNonQuery();



                            OracleCommand bnBankAcc = new OracleCommand();
                            bnBankAcc.Connection = conn;
                            bnBankAcc.CommandText = "insert into bn_bankaccount values (:bankacc,:noteid)";
                            bnBankAcc.CommandType = CommandType.Text;
                            bnBankAcc.Parameters.Add("bankacc", Convert.ToInt32(textBox3.Text));
                            bnBankAcc.Parameters.Add("noteid", newid);
                            bnBankAcc.ExecuteNonQuery();


                            for (int i = 0; i < listBox1.Items.Count; i++)
                            {
                                OracleCommand bnAdreess = new OracleCommand();
                                bnAdreess.Connection = conn;
                                bnAdreess.CommandText = "insert into bn_addresses values (:bnaddrees,:noteid)";
                                bnAdreess.CommandType = CommandType.Text;
                                bnAdreess.Parameters.Add("bnaddrees", listBox1.Items[i].ToString());
                                bnAdreess.Parameters.Add("noteid", newid);
                                bnAdreess.ExecuteNonQuery();

                            }

                            for (int i = 0; i < listBox2.Items.Count; i++)
                            {
                                OracleCommand bnPhones = new OracleCommand();
                                bnPhones.Connection = conn;
                                bnPhones.CommandText = "insert into bn_phones values (:bnphone,:noteid)";
                                bnPhones.CommandType = CommandType.Text;
                                bnPhones.Parameters.Add("bnphone", Convert.ToInt32(listBox2.Items[i]));
                                bnPhones.Parameters.Add("noteid", newid);
                                bnPhones.ExecuteNonQuery();

                            }

                            for (int i = 0; i < listBox3.Items.Count; i++)
                            {
                                OracleCommand bnEmail = new OracleCommand();
                                bnEmail.Connection = conn;
                                bnEmail.CommandText = "insert into bn_emails values (:bnemail,:noteid)";
                                bnEmail.CommandType = CommandType.Text;
                                bnEmail.Parameters.Add("bnemail", listBox3.Items[i].ToString());
                                bnEmail.Parameters.Add("noteid", newid);
                                bnEmail.ExecuteNonQuery();

                            }
                            PopupNotifier popup = new PopupNotifier();
                            popup.TitleText = "Bussines Note Created Successfully";
                            popup.Popup();
                            form1.bunifuCustomLabel5.Text = (Convert.ToInt32(form1.bunifuCustomLabel5.Text) + 1).ToString();
                            s.listBox1.Items.Add(textBox1.Text);
                            addcontroltopanel(s);
                        }
                        catch
                        {
                            MessageBox.Show("invalid input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                    }


                }


            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string x = "";
            string address = "";
            bool isExit = false;
            address = Interaction.InputBox("Please Enter Your address", "Address", x, -1, -1);

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.Items[i].Equals(address))
                    isExit = true;
            }

            if (isExit)
            {
                MessageBox.Show("Address is Already Exist", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (address != "")
            {
                listBox1.Items.Add(address);
                // MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            string x = "";
            string phone = "";
            bool isExit = false;
            phone = Interaction.InputBox("Please Enter Your Phone", "Phone", x, -1, -1);
            bool flag = Extensions.IsNumeric(phone);
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                if (listBox2.Items[i].Equals(phone))
                    isExit = true;
            }

            if (isExit)
            {
                MessageBox.Show("Phone is Already Exist", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (flag && phone.Length < 12 && !isExit && phone.Length > 0)
            {
                listBox2.Items.Add(phone);
            }
            else
            {
                MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            listBox3.Items.Remove(listBox3.SelectedItem);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            string x = "";
            string email = "";
            bool isExit = false;
            email = Interaction.InputBox("Please Enter Your email", "Email", x, -1, -1);

            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                if (listBox3.Items[i].Equals(email))
                    isExit = true;
            }

            if (isExit)
            {
                MessageBox.Show("Email is Already Exist", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (email != "")
            {
                listBox3.Items.Add(email);
                
            }
            else
            {
                MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
