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
    
    public partial class ShowContactsReport : UserControl
    {
        Form1 ths;
        ContactsReport report;
        int userID;
        public ShowContactsReport(Form1 form)
        {
            InitializeComponent();
            ths = form;
            string userName = ths.userName.Text.ToLower();
            userID = ths.userID;
            report = new ContactsReport();
            report.SetParameterValue(0, userID);
            crystalReportViewer1.ReportSource = report;
        }
    }
}
