using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace oracleDatabase
{
    class oracleDatabaseOperate
    {

        //连接Oracle数据库的方法by ethan  20180916

        //连接数据库

        //OracleConnection conn = new OracleConnection("Data Source=10.228.141.253/ORCL;User Id=WEBKF;Password=WEBKF");
        
       OracleConnection conn = new OracleConnection("Data Source = 10.228.141.253/ORCL;User Id = JSL; Password=fawccr");

        public void connOpen()
        {
            conn.Open();
        }

        public void connClose()
        {
            conn.Close();
        }
        //连接OracleConnection,执行SQL
        public void OrcGetCom(string M_str_sqlstr)
        {


            OracleCommand orccom = new OracleCommand(M_str_sqlstr, conn);
            orccom.ExecuteNonQuery();


        }


        //创建DataSet对象
        public DataSet OrcGetDs(string M_str_sqlstr, string M_str_table)
        {

            OracleDataAdapter orcda = new OracleDataAdapter(M_str_sqlstr, conn);
            DataSet myds = new DataSet();
            orcda.Fill(myds, M_str_table);
            return myds;
        }


        //创建OracleDataReader对象
        public OracleDataReader OrcGetRead(string M_str_sqlstr)
        {

            OracleCommand orccom = new OracleCommand(M_str_sqlstr, conn);
            OracleDataReader orcread = orccom.ExecuteReader();
            return orcread;

        }


        //获取数据库中条数
        public int OrcGetNums(string M_str_sqlstr)
        {

            OracleCommand orccom = new OracleCommand(M_str_sqlstr, conn);
            OracleDataReader orcread = orccom.ExecuteReader();

            int i = 0;

            while (orcread.Read())
            {
                i++;
            }

            return i;

        }
    }
}
