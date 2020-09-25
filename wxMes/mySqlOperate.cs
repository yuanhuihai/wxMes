using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace mySqlOperate
{
    class mySqlHelp
    {

        //建立连接 2020-08-18
   
       MySqlConnection conn = new MySqlConnection("server=rm-hp3q8vgfzl4du8493zo.mysql.huhehaote.rds.aliyuncs.com;port=3306;user=ccr_123;password=FAW_ccr123!; database=xiushi;");
  
        public void connOpen()
        {
            conn.Open();
        }
        //执行sql语句 2020-08-18
        public void MySqlCom(string sqlstr)
        {
            MySqlCommand mysqlcom = new MySqlCommand(sqlstr, conn);
            mysqlcom.ExecuteNonQuery();

        }
    }
}
