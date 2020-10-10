using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using comWithPlc;
using mySqlOperate;
using oracleDatabase;
using Oracle.ManagedDataAccess.Client;
using System.Threading;





#region
/*


 * timer3定时时间1s
 * 
 * 
 * 
 */
#endregion





namespace wxMes
{
    public partial class Form1 : Form
    {
       

        public Form1()
        {
            InitializeComponent();
   
        }


            getPlcValues operatePlc = new getPlcValues();

            mySqlHelp sqlOperate = new mySqlHelp();

            oracleDatabaseOperate oraOperate = new oracleDatabaseOperate();



        public string totalCarNum;
        public string[] chimneyTem = new string[7];
        public string[] tnvTem = new string[7];
        public double ratetotal, rateone, ratetwo;


        private void Form1_Load(object sender, EventArgs e)
        {
            //打开数据库连接
            oraOperate.connOpen();
   
            Thread carRate = new Thread(new ThreadStart(carRateToMysql));      
            carRate.Start();

            Thread lineone = new Thread(new ThreadStart(getColorOneInfo));
            lineone.Start();

            Thread linetwo = new Thread(new ThreadStart(getColorTwoInfo));
            linetwo.Start();

            Thread ovenInfo = new Thread(new ThreadStart(ovenInfoToAliMysql));
            ovenInfo.Start();



    

            timer3.Start();//每1s触发一次

  
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Interval = 1000;//执行间隔时间,单位为毫秒;此时时间间隔为1秒          
      
            carNumToMysql();//定点数据插入数据库  

            toolStripStatusLabel1.Text = DateTime.Now.ToLocalTime().ToString();
        }


        #region   //窗口关闭时，程序后台运行

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要退出程序吗？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                notifyIcon1.Visible = false;
                this.Close();
                this.Dispose();
                Application.Exit();
            }

        }

        private void hideMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void showMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();

        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//判断鼠标的按键
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Minimized;

                    this.Hide();
                }
                else if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
            }

        }

        #endregion



 

        #region 20201010 获取烘干炉温度信息

        private void ovenInfoToAliMysql()
        {
            while (true)
            {
                Thread.Sleep(600000);
                chimneyTem[0] = Convert.ToString(operatePlc.readPlcDbdValues("10.228.140.46", 0, 3, 92, 0));
                chimneyTem[1] = Convert.ToString(operatePlc.readPlcDbdValues("10.228.140.54", 0, 3, 92, 0));
                chimneyTem[2] = Convert.ToString(operatePlc.readPlcDbdValues("10.228.140.98", 0, 3, 92, 0));
                chimneyTem[3] = Convert.ToString(operatePlc.readPlcDbdValues("10.228.141.38", 0, 3, 92, 0));
                chimneyTem[4] = Convert.ToString(operatePlc.readPlcDbdValues("10.228.141.46", 0, 3, 92, 0));
                chimneyTem[5] = Convert.ToString(operatePlc.readPlcDbdValues("10.228.141.82", 0, 3, 92, 0));
                chimneyTem[6] = Convert.ToString(operatePlc.readPlcDbdValues("10.228.141.126", 0, 3, 92, 0));
                tnvTem[0] = Convert.ToString(operatePlc.readPlcDbwValue("10.228.140.46", 0, 3, 294, 2));
                tnvTem[1] = Convert.ToString(operatePlc.readPlcDbwValue("10.228.140.54", 0, 3, 294, 2));
                tnvTem[2] = Convert.ToString(operatePlc.readPlcDbwValue("10.228.140.98", 0, 3, 294, 2));
                tnvTem[3] = Convert.ToString(operatePlc.readPlcDbwValue("10.228.141.38", 0, 3, 294, 2));
                tnvTem[4] = Convert.ToString(operatePlc.readPlcDbwValue("10.228.141.46", 0, 3, 294, 2));
                tnvTem[5] = Convert.ToString(operatePlc.readPlcDbwValue("10.228.141.82", 0, 3, 294, 2));
                tnvTem[6] = Convert.ToString(operatePlc.readPlcDbwValue("10.228.141.126", 0, 3, 294, 2));

                string sql = "insert into oveninfo (DA,ED1TNV,ED1CHIMNEY,ED2TNV,ED2CHIMNEY,PVCTNV,PVCCHIMNEY,PR1TNV,PR1CHIMNEY,PR2TNV,PR2CHIMNEY,TC1TNV,TC1CHIMNEY,TC2TNV,TC2CHIMNEY) values('" + DateTime.Now.ToString() + "','" + chimneyTem[0] + "','" + chimneyTem[1] + "','" + chimneyTem[2] + "','" + chimneyTem[3] + "','" + chimneyTem[4] + "','" + chimneyTem[5] + "','" + chimneyTem[6] + "','" + tnvTem[0] + "','" + tnvTem[1] + "','" + tnvTem[2] + "','" + tnvTem[3] + "','" + tnvTem[4] + "','" + tnvTem[5] + "','" + tnvTem[6] + "')";


                //检测网络连接状态，网络连接成功后，写入数据

                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

                System.Net.NetworkInformation.PingReply pingStatus = ping.Send(IPAddress.Parse("202.108.22.5"), 1000);//ping 百度的IP地址

                if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
                {

                    sqlOperate.MySqlCom(sql);

                }

            }







        }


        #endregion

        #region 20201010 获取车身数量信息及实时交检率
        private int getCarNum(string dataBaseName)
        {


            string sql = "select * from " + dataBaseName + " where DT='合计'  ";
       
            OracleDataReader read = oraOperate.OrcGetRead(sql);
            read.Read();
            return Convert.ToInt16(read["TOTAL"].ToString());

        }

        private int getOkCarNum(string dataBaseName)
        {
            string sql = "select * from " + dataBaseName + " where DT='合计'  ";
         
            OracleDataReader read = oraOperate.OrcGetRead(sql);
            read.Read();
            return Convert.ToInt16(read["OK"].ToString());
        }

        private int getTotalCarNum()
        {
            return getCarNum("YM_FINISH_INFO") + getCarNum("YM_FINISH2_INFO") + getCarNum("YM_FINISH3_INFO") + getCarNum("YM_FINISH4_INFO");
        }
        private int getTotalOkCarNum()
        {
            return getOkCarNum("YM_FINISH_INFO") + getOkCarNum("YM_FINISH2_INFO") + getOkCarNum("YM_FINISH3_INFO") + getOkCarNum("YM_FINISH4_INFO");
        }

        private int getTotalCarNumone()
        {
            return getCarNum("YM_FINISH_INFO") + getCarNum("YM_FINISH2_INFO") ;
        }
        private int getTotalOkCarNumone()
        {
            return getOkCarNum("YM_FINISH_INFO") + getOkCarNum("YM_FINISH2_INFO");
        }

        private int getTotalCarNumtwo()
        {
            return  getCarNum("YM_FINISH3_INFO") + getCarNum("YM_FINISH4_INFO");
        }
        private int getTotalOkCarNumtwo()
        {
            return getOkCarNum("YM_FINISH3_INFO") + getOkCarNum("YM_FINISH4_INFO");
        }

        private void carNumToMysql()
        {
            if (DateTime.Now.Hour == Convert.ToInt32(23) && DateTime.Now.Minute == Convert.ToInt32(59) && DateTime.Now.Second == Convert.ToInt32(00))
            {

                string sql = "insert into production (RIQI,SHULIANG) values('" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + getTotalCarNum() + "')";


                //检测网络连接状态，网络连接成功后，写入数据

                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

                System.Net.NetworkInformation.PingReply pingStatus = ping.Send(IPAddress.Parse("202.108.22.5"), 1000);//ping 百度的IP地址

                if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    sqlOperate.MySqlCom(sql);

                }


            }


        }

        private void carRateToMysql()
        {

            while (getTotalCarNumone()!=0 &&getTotalCarNumtwo()!=0)
            {

                ratetotal = (double)getTotalOkCarNum() / getTotalCarNum();
                rateone = (double)getTotalOkCarNumone() / getTotalCarNumone();
                ratetwo = (double)getTotalOkCarNumtwo() / getTotalCarNumtwo();

                string sql = "insert into finalcarinfo values('" + DateTime.Now.ToString() + "','" + getTotalCarNum() + "','" + ratetotal.ToString("0.00%") + "','" + getTotalCarNumone() + "','" + rateone.ToString("0.00%") + "','" + getTotalCarNumtwo() + "','" + ratetwo.ToString("0.00%") + "')";
                string sqldel = "delete from finalcarinfo";


                //检测网络连接状态，网络连接成功后，写入数据

                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

                System.Net.NetworkInformation.PingReply pingStatus = ping.Send(IPAddress.Parse("202.108.22.5"), 1000);//ping 百度的IP地址

                if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    sqlOperate.MySqlCom(sqldel);
                    sqlOperate.MySqlCom(sql);

                }

                Thread.Sleep(300000);

            }






        }


        #endregion

        #region 20201010 获取面漆一线换色率

        private void getColorOneInfo()
        {
            while (true)
            {
                string sql = "select * from V_MQ WHERE D='" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND STAT='mq1'";

                int a = oraOperate.OrcGetNums(sql);
                int b = 0;
                int onelinetimes = 0;
                string[] colorInfo = new string[a];


                OracleDataReader read = oraOperate.OrcGetRead(sql);

                while (read.Read())
                {
                    colorInfo[b] = read["COLOR"].ToString();
                    b++;
                }

                for (int c = 0; c < a - 1; c++)
                {
                    if (colorInfo[c + 1] == colorInfo[c])
                    {
                        colorInfo[c] = "0";
                    }
                }

                for (int d = 0; d < a - 1; d++)
                {
                    if (colorInfo[d] == "0")
                    {

                    }
                    else
                    {
                        onelinetimes++;
                    }
                }



                string sqll = "insert into ccrateinfo (LINEPOS,LINENUM,LINETIMES,LINERATE) values ('one','" + a + "','" + onelinetimes + "','" + a / onelinetimes + "')";
                string sqldel = "delete from ccrateinfo where LINEPOS='one'";

                //检测网络连接状态，网络连接成功后，写入数据

                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

                System.Net.NetworkInformation.PingReply pingStatus = ping.Send(IPAddress.Parse("202.108.22.5"), 1000);//ping 百度的IP地址

                if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
                {

                    sqlOperate.MySqlCom(sqldel);
                    sqlOperate.MySqlCom(sqll);

                }

                Thread.Sleep(240000);
            }
           
               


            

        }




        #endregion

        #region 20201010 获取面漆二线换色率

        private void getColorTwoInfo()
        {
            while (true)
            {
                Thread.Sleep(300000);

                string sql = "select * from V_MQ WHERE D='" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND STAT='mq2'";

                int a = oraOperate.OrcGetNums(sql);
                int b = 0;
                int twolinetimes = 0;
                string[] colorInfo = new string[a];


                OracleDataReader read = oraOperate.OrcGetRead(sql);

                while (read.Read())
                {
                    colorInfo[b] = read["COLOR"].ToString();
                    b++;
                }

                for (int c = 0; c < a - 1; c++)
                {
                    if (colorInfo[c + 1] == colorInfo[c])
                    {
                        colorInfo[c] = "0";
                    }
                }

                for (int d = 0; d < a - 1; d++)
                {
                    if (colorInfo[d] == "0")
                    {

                    }
                    else
                    {
                        twolinetimes++;
                    }
                }

                string sqll = "insert into ccrateinfo (LINEPOS,LINENUM,LINETIMES,LINERATE) values('two','" + a + "','" + twolinetimes + "','" + a / twolinetimes + "')";
                string sqldel = "delete from ccrateinfo where LINEPOS='two'";

                //检测网络连接状态，网络连接成功后，写入数据

                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

                System.Net.NetworkInformation.PingReply pingStatus = ping.Send(IPAddress.Parse("202.108.22.5"), 1000);//ping 百度的IP地址

                if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    sqlOperate.MySqlCom(sqldel);
                    sqlOperate.MySqlCom(sqll);

                }

            }





        }




        #endregion
    }
}