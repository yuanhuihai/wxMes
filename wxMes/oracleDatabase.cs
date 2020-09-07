using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace oracleDatabase
{
    class oracleOperate
    {


       //连接Oracle数据库的方法by ethan  20180916
    
        //连接数据库
        public OracleConnection OrcGetCon()
        {
           string M_str_sqlcon = "Data Source=10.228.141.253/ORCL;User Id=JSL;Password=fawccr";//定义数据库连接字符串          
           OracleConnection myCon = new OracleConnection(M_str_sqlcon);
            return myCon;
        }

        //关闭数据库连接
        public void conClose()
        {
           this.OrcGetCon().Close();
        }

        //连接OracleConnection,执行SQL
        public void OrcGetCom(string M_str_sqlstr)
        {
            OracleConnection orccon = this.OrcGetCon();
            orccon.Open();
            OracleCommand orccom = new OracleCommand(M_str_sqlstr, orccon);
            orccom.ExecuteNonQuery();
            orccom.Dispose();
            orccon.Close();
            orccon.Dispose();
        }


        //创建DataSet对象
        public DataSet OrcGetDs(string M_str_sqlstr, string M_str_table)
        {
            OracleConnection orccon = this.OrcGetCon();
            OracleDataAdapter orcda = new OracleDataAdapter(M_str_sqlstr, orccon);
            DataSet myds = new DataSet();
            orcda.Fill(myds, M_str_table);
            return myds;
        }


        //创建OracleDataReader对象
        public OracleDataReader OrcGetRead(string M_str_sqlstr)
        {
            OracleConnection orccon = this.OrcGetCon();
            OracleCommand orccom = new OracleCommand(M_str_sqlstr, orccon);
            orccon.Open();
            //OracleDataReader orcread = orccom.ExecuteReader(CommandBehavior.CloseConnection);
            OracleDataReader orcread = orccom.ExecuteReader(CommandBehavior.CloseConnection);
            return orcread;
        }
    }
}
