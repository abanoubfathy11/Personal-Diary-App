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
    public partial class ShowNotesReport : UserControl
    {
        Form1 ths;
        NotesReport report;
        int userID;
        public ShowNotesReport(Form1 form)
        {
            InitializeComponent();
            ths = form;
            string userName = ths.userName.Text.ToLower();
            userID = ths.userID;
            report = new NotesReport();
            report.SetParameterValue(0, userID);
            report.SetParameterValue(1, "31-DEC-2000");
            report.SetParameterValue(2, System.DateTime.Now);
            crystalReportViewer1.ReportSource = report;
        }

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {
            report = new NotesReport();
            report.SetParameterValue(0, userID);
            report.SetParameterValue(1, "22-DEC-2000");
            report.SetParameterValue(2, System.DateTime.Now);
            crystalReportViewer1.ReportSource = report;
        }

        private void bunifuCustomLabel2_Click(object sender, EventArgs e)
        {
            report = new NotesReport();
            report.SetParameterValue(0, userID);
            report.SetParameterValue(1, Convert.ToDateTime(dateTimePicker1.Value.Date));
            report.SetParameterValue(2, Convert.ToDateTime(dateTimePicker2.Value.Date));
            crystalReportViewer1.ReportSource = report;
        }
    }
}
