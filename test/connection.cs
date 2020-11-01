using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Data;

namespace test
{
    class connection
    {
        public string ordb = "Data source=orcl;User Id=scott; Password=tiger;";
        public OracleCommand cmd;
        public OracleConnection conn;
        public connection()
        {

            conn = new OracleConnection("Data Source=ORCL;User Id=scott;password=tiger;");
            conn.Open();
            cmd = new OracleCommand();
        }
        public void comand()
        {
            cmd.Connection = conn;
        }
        public void DELETING_PROC(int a, int b)
        {

            cmd.Connection = conn;
            cmd.CommandText = "DELETING_PROC ";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("id_proc", a);
            cmd.Parameters.Add("note_delete", b);


            //cmd.Parameters.Add("text_proc", text); 
            cmd.ExecuteNonQuery();
            conn.Close();



        }


        internal void DELETING_PROC(string p)
        {
            throw new NotImplementedException();
        }
    }
}
