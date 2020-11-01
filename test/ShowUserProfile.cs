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
    public partial class ShowUserProfile : UserControl
    {
        Form1 ths;
        int userID = -1;
        string ordb = "Data source=orcl;User Id=scott; Password=tiger;";
        OracleConnection conn;
        public ShowUserProfile(Form1 form)
        {
            InitializeComponent();
            ths = form;
            string user = ths.userName.Text.ToString();
            userID = ths.userID;

            conn = new OracleConnection(ordb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select user_name,user_email,user_pass,bdata from users where user_id=:uuid";
            cmd.Parameters.Add("uuid", userID);
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            try
            {
                textBox1.Text = dr[0].ToString();
                textBox2.Text = dr[1].ToString();
                textBox3.Text = dr[2].ToString();
                dateTimePicker1.Text = dr[3].ToString();
                dr.Close();
            }
            catch
            {
                MessageBox.Show("You should Login Frist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //OracleCommand c = new OracleCommand();
            //c.Connection = conn;
            //c.CommandText = "select phone from phone WHERE userid=:iid";
            //c.Parameters.Add("iid", userID);

            //OracleDataReader drr = c.ExecuteReader();
            //while (drr.Read())
            //{
            //    listBox1.Items.Add(drr[0].ToString());
            //}
            //drr.Close();

            OracleCommand cv = new OracleCommand();
            cv.Connection = conn;
            cv.CommandText = "GetPhonesUser";
            cv.CommandType = CommandType.StoredProcedure;
            cv.Parameters.Add("iid", userID);
            cv.Parameters.Add("Phone", OracleDbType.RefCursor, ParameterDirection.Output);

            OracleDataReader dt = cv.ExecuteReader();
            while (dt.Read())
            {
                listBox1.Items.Add(dt[0].ToString());
            }
            dt.Close();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            //update user Profile
            //DON'T MISS VALIDATION for textboxes
            //username in variable called [user]
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                conn = new OracleConnection(ordb);
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "update users set user_name=:uname, user_email=:uemail, user_pass=:upass, bdata=:udata  where user_id=:uuid";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("uname", textBox1.Text);
                cmd.Parameters.Add("uemail", textBox2.Text);
                cmd.Parameters.Add("upass", textBox3.Text);
                cmd.Parameters.Add("udata", dateTimePicker1.Value);
                cmd.Parameters.Add("uuid", userID);



                int r = -1, t = -1;
                try
                {
                    r = cmd.ExecuteNonQuery();
                    ths.userName.Text = textBox1.Text;
                }
                catch
                {
                    MessageBox.Show("Email is Already Exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                OracleCommand c = new OracleCommand();
                c.Connection = conn;
                c.CommandText = "delete from phone where userid=:iid";
                c.CommandType = CommandType.Text;
                c.Parameters.Add("iid", userID);
                int h = c.ExecuteNonQuery();


                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    OracleCommand cc = new OracleCommand();
                    cc.Connection = conn;
                    cc.CommandText = "insert into phone values (:phone,:iid)";
                    cc.Parameters.Add("phone", listBox1.Items[i]);
                    cc.Parameters.Add("iid", userID);
                    try
                    {
                        t = cc.ExecuteNonQuery();
                    }
                    catch
                    {
                        MessageBox.Show("No Phone Number Entered", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

                if (r != -1)
                {
                    MessageBox.Show("User Profile has been Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }


        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string selectedItem = listBox1.SelectedItem.ToString();

                conn = new OracleConnection(ordb);
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "delete from phone where phone=:phone and userid=:iid";
                cmd.Parameters.Add("phone", selectedItem);
                cmd.Parameters.Add("iid", userID);
                int r = cmd.ExecuteNonQuery();
                if (r != -1)
                {
                    listBox1.Items.Remove(selectedItem);
                    MessageBox.Show("Phone Number has been Deleted Successfully", "Erroe", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
            else
            {
                MessageBox.Show("Please Select Phone to Delete", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            string x = "";
            string phone = "";
            bool isExit = false;
            phone = Interaction.InputBox("Please Enter Your Phone", "Phone", x, -1, -1);
            bool flag = Extensions.IsNumeric(phone);
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.Items[i].Equals(phone))
                    isExit = true;
            }

            if (isExit)
            {
                MessageBox.Show("Phone is Already Exist", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (flag && phone.Length < 12 && !isExit && phone.Length > 0)
            {
                listBox1.Items.Add(phone);
            }
            else
            {
                MessageBox.Show("Invalid Input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
