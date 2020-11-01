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
    public partial class Form1 : Form
    {
        public int userID = -1;
        int mov;
        int movX;
        int movY;

        OracleConnection conn;
        public Form1()
        {
            InitializeComponent();
            movePanel(btnNotes);
            timer1.Start();
            bunifuCustomLabel4.Text = "";
            bunifuCustomLabel5.Text = "";
            bunifuCustomLabel7.Text = "";

            userName.Text = "";

            conn = new OracleConnection("Data source=orcl;User Id=scott; Password=tiger;");
        }

        private void movePanel(Control c)
        {
            sidePanel.Height = c.Height;
            sidePanel.Top = c.Top;
        }
        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            addcontroltopanel(panellogin);
            bunifuCustomLabel4.Text = "";
            bunifuCustomLabel5.Text = "";
            bunifuCustomLabel7.Text = "";

            userName.Text = "";

            userID = -1;
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            if (userID != -1)
            {
                movePanel(btnRemainder);
                ShowRemainders s = new ShowRemainders(this);
                addcontroltopanel(s);
            }
            else
            {
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Personal Dairy Application";
                popup.ContentText = "LOGIN FIRST PLZ! ";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            if (userID != -1)
            {
                movePanel(btnContact);
                ShowBusinessContacts s = new ShowBusinessContacts(this);
                addcontroltopanel(s);
            }
            else
            {
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Personal Dairy Application";
                popup.ContentText = "LOGIN FIRST PLZ! ";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }
        }
        private void addcontroltopanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panelcontrol.Controls.Clear();
            panelcontrol.Controls.Add(c);
        }
        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            if (userID != -1)
            {
                movePanel(btnNotes);
                ShowNotesForm s = new ShowNotesForm(this);
                addcontroltopanel(s);
            }
            else
            {
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Personal Dairy Application";
                popup.ContentText = "LOGIN FIRST PLZ! ";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuCustomLabel4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if(mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        private void userName_Click(object sender, EventArgs e)
        {
            if (userID != -1)
            {
                ShowUserProfile s = new ShowUserProfile(this);
                addcontroltopanel(s);
            }
        }

        private void bunifuFlatButton3_Click_1(object sender, EventArgs e)
        {
            
        }

        private void bunifuFlatButton3_Click_2(object sender, EventArgs e)
        {
            //notification


            //if signed true
            

            PopupNotifier popup = new PopupNotifier();
            popup.TitleText = "Personal Dairy Application";

            Boolean EMAIL = true, PASS = true;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select user_id FROM users where user_email=:email";
            cmd.Parameters.Add("email", textBox1.Text.ToString());
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
                userID = Convert.ToInt32(dr[0]);
            else
                EMAIL = false;
            dr.Close();
            conn.Close();

            if (EMAIL == true)
            {
                string name = "";
                conn.Open();
                cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select user_email,user_pass,user_name from users where user_id=" + userID;
                cmd.CommandType = CommandType.Text;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                    if (!textBox2.Text.Equals(dr[1].ToString()))
                    {
                        PASS = false;

                    }
                    else name = dr[2].ToString();
                dr.Close();

                if (PASS == true)
                {
                    cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select count(note_id) from note where userid=" + userID;
                    cmd.CommandType = CommandType.Text;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                        bunifuCustomLabel4.Text = dr[0].ToString();
                    dr.Close();


                    cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select count(bn_id) from bussines_note where userid=" + userID;
                    cmd.CommandType = CommandType.Text;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                        bunifuCustomLabel5.Text = dr[0].ToString();
                    dr.Close();


                    cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select count(rem_id) from reminder where userid=" + userID;
                    cmd.CommandType = CommandType.Text;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                        bunifuCustomLabel7.Text = dr[0].ToString();
                    dr.Close();

                    conn.Close();
                    movePanel(btnNotes);
                    ShowNotesForm s = new ShowNotesForm(this);
                    addcontroltopanel(s);

                    userName.Text = name.ToString();

                    popup.ContentText = "Welcome, " + name;
                    popup.Image = Properties.Resources.icons8_Handshake_Heart_32px_1;
                    popup.Popup();

                    textBox1.Text = "";
                    textBox2.Text = "";
                }
                else
                { 
                    popup.ContentText = "Password incorrect, try agian if you already have a mail if not sign up. ";
                    popup.Image = Properties.Resources.icons8_Error_32px;
                    popup.Popup();
                }
            }
            else
            {
                popup.ContentText = "Email incorrect, try agian if you already have a mail if not sign up. ";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }
            conn.Close();

        }

        private void bunifuFlatButton2_Click_1(object sender, EventArgs e)
        {
            //notification
            PopupNotifier popup = new PopupNotifier();
            popup.TitleText = "Personal Dairy Application";

            //if signed true
            //popup.ContentText = "Welcome, ";
            //popup.Image = Properties.Resources.icons8_Handshake_Heart_32px_1;
            //popup.Popup();

            //if signed false
            
            //movePanel(btnNotes);
            //ShowNotesForm s = new ShowNotesForm(this);
            //addcontroltopanel(s);
            Boolean EMAIL = true;

            if (textBox5.Text.Equals("") || textBox3.Text.Equals("") || textBox4.Text.Equals(""))
            {
                popup.ContentText = "Something wrong, try again and enter all data. ";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }
            else if(dateTimePicker1.Value > System.DateTime.Now)
            {
                popup.ContentText = "INvalid Birthdate !!!";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }
            else
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select user_id FROM users where user_email=:email";
                cmd.Parameters.Add("email", textBox3.Text.ToString());
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                    EMAIL = false;

                dr.Close();
                conn.Close();
                if (EMAIL != false)
                {
                    conn.Open();
                    cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select max(user_id) from users";
                    cmd.CommandType = CommandType.Text;
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                        userID = Convert.ToInt32(dr[0]) + 1;
                    else userID = 1;
                    dr.Close();

                    cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO users VALUES (:id,:name,:email,:pass,:bdate)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("id", userID);
                    cmd.Parameters.Add("name", textBox5.Text);
                    cmd.Parameters.Add("email", textBox3.Text);
                    cmd.Parameters.Add("pass", textBox4.Text);
                    cmd.Parameters.Add("bdate",Convert.ToDateTime( dateTimePicker1.Value));
                    int err = cmd.ExecuteNonQuery();
                    if(err != -1)
                    {
                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "select count(note_id) from note where userid=" + userID;
                        cmd.CommandType = CommandType.Text;
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                            bunifuCustomLabel4.Text = dr[0].ToString();
                        else bunifuCustomLabel4.Text = 0.ToString(); 
                        dr.Close();


                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "select count(bn_id) from bussines_note where userid=" + userID;
                        cmd.CommandType = CommandType.Text;
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                            bunifuCustomLabel5.Text = dr[0].ToString();
                        else
                            bunifuCustomLabel5.Text = 0.ToString();
                        dr.Close();


                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "select count(rem_id) from reminder where userid=" + userID;
                        cmd.CommandType = CommandType.Text;
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                            bunifuCustomLabel7.Text = dr[0].ToString();
                        else bunifuCustomLabel7.Text = 0.ToString();
                        dr.Close();

                        conn.Close();
                        movePanel(btnNotes);
                        ShowNotesForm s = new ShowNotesForm(this);
                        addcontroltopanel(s);

                        userName.Text = textBox5.Text.ToString();

                        popup.ContentText = "Welcome, " + textBox5.Text.ToString();
                        popup.Image = Properties.Resources.icons8_Handshake_Heart_32px_1;
                        popup.Popup();

                        textBox5.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        dateTimePicker1.Value = System.DateTime.Now;
                    }
                    conn.Close();
                }
                else
                {
                    popup.ContentText = "EMAIL already exist !!";
                    popup.Image = Properties.Resources.icons8_Error_32px;
                    popup.Popup();
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            labeltimer.Text = dt.ToString("HH:MM:ss");
            //used for remainder
            //check each tick == or != any row in database'remainder'
        }

        private void bunifuCustomLabel17_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel3_Click(object sender, EventArgs e)
        {
            if (userID != -1)
            {
                ShowNotesReport s = new ShowNotesReport(this);
                addcontroltopanel(s);
            }
            else
            {
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Personal Dairy Application";
                popup.ContentText = "LOGIN FIRST PLZ! ";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }

        }

        private void bunifuCustomLabel6_Click(object sender, EventArgs e)
        {
            if (userID != -1)
            {
                ShowContactsReport s = new ShowContactsReport(this);
                addcontroltopanel(s);
            }
            else
            {
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Personal Dairy Application";
                popup.ContentText = "LOGIN FIRST PLZ! ";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }
        }

        private void bunifuCustomLabel8_Click(object sender, EventArgs e)
        {
            if (userID != -1)
            {
                ShowRemainderReport s = new ShowRemainderReport(this);
                addcontroltopanel(s);
            }
            else
            {
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Personal Dairy Application";
                popup.ContentText = "LOGIN FIRST PLZ! ";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }
        }

        private void bunifuCustomLabel18_Click(object sender, EventArgs e)
        {
            if (userID != -1)
            {
                ShowTODO s = new ShowTODO(this);
                addcontroltopanel(s);
            }
            else
            {
                PopupNotifier popup = new PopupNotifier();
                popup.TitleText = "Personal Dairy Application";
                popup.ContentText = "LOGIN FIRST PLZ! ";
                popup.Image = Properties.Resources.icons8_Error_32px;
                popup.Popup();
            }
        }
    }
}
