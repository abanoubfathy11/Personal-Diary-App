using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class ShowRemainderReport : UserControl
    {
        Form1 ths;
        RemaindersReport report;
        int userID;
        public ShowRemainderReport(Form1 form)
        {
            InitializeComponent();
            ths = form;
            string userName = ths.userName.Text.ToLower();
            userID = ths.userID;
            report = new RemaindersReport();
            report.SetParameterValue(0, 3);
            crystalReportViewer1.ReportSource = report;

        }

        private void bunifuCustomLabel4_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
