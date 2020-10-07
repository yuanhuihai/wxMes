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




#region
/*
 * timer1定时时间600s
 * timer2定时时间120s
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
        

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();//获取PLC信息
            timer2.Start();//信息发送给阿里云
            timer3.Start();//每1s触发一次

            //打开数据库连接
            oraOperate.connOpen();
            sqlOperate.connOpen();
        }


        #region //烘干炉信息上传至阿里云


        //每隔10分钟获取一次PLC的信息
        private void timer1_Tick(object sender, EventArgs e)
            {
                timer1.Interval = 600000;//执行间隔时间,单位为毫秒;此时时间间隔为600秒   


            #region  获取烘干炉烟囱温度/TNV出口温度信息
            label19.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.140.46", 0, 3, 92, 0));
                    label18.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.140.54", 0, 3, 92, 0));
                    label17.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.140.98", 0, 3, 92, 0));
                    label16.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.141.38", 0, 3, 92, 0));
                    label15.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.141.46", 0, 3, 92, 0));
                    label14.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.141.82", 0, 3, 92, 0));
                    label13.Text = Convert.ToString(operatePlc.readPlcDbdValues("10.228.141.126", 0, 3, 92, 0));
                    label25.Text = System.Convert.ToString(operatePlc.readPlcDbwValue("10.228.140.46", 0, 3, 294, 2));
                    label24.Text = System.Convert.ToString(operatePlc.readPlcDbwValue("10.228.140.54", 0, 3, 294, 2));
                    label23.Text = System.Convert.ToString(operatePlc.readPlcDbwValue("10.228.140.98", 0, 3, 294, 2));
                    label22.Text = System.Convert.ToString(operatePlc.readPlcDbwValue("10.228.141.38", 0, 3, 294, 2));
                    label21.Text = System.Convert.ToString(operatePlc.readPlcDbwValue("10.228.141.46", 0, 3, 294, 2));
                    label20.Text = System.Convert.ToString(operatePlc.readPlcDbwValue("10.228.141.82", 0, 3, 294, 2));
                    label5.Text = System.Convert.ToString(operatePlc.readPlcDbwValue("10.228.141.126", 0, 3, 294, 2));

            #endregion

            #region 将获取到的信息上传至阿里云

            string currentTime = DateTime.Now.ToString();

            string sql = "insert into oveninfo (DA,ED1TNV,ED1CHIMNEY,ED2TNV,ED2CHIMNEY,PVCTNV,PVCCHIMNEY,PR1TNV,PR1CHIMNEY,PR2TNV,PR2CHIMNEY,TC1TNV,TC1CHIMNEY,TC2TNV,TC2CHIMNEY) values('" + currentTime + "','" + label19.Text + "','" + label25.Text + "','" + label18.Text + "','" + label24.Text + "','" + label17.Text + "','" + label23.Text + "','" + label16.Text + "','" + label22.Text + "','" + label15.Text + "','" + label21.Text + "','" + label14.Text + "','" + label20.Text + "','" + label13.Text + "','" + label5.Text + "')";


            //检测网络连接状态，网络连接成功后，写入数据
            try
            {

                //System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

                //System.Net.NetworkInformation.PingReply pingStatus = ping.Send(IPAddress.Parse("202.108.22.5"), 1000);//ping 百度的IP地址

                //if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
                //{
                //    sqlOperate.MySqlCom(sql);


                //}
                //else
                //{
                //    listInfo.Items.Add(DateTime.Now.ToString() + "ping外网连接失败");
                //    if (listInfo.Items.Count > 50)
                //    {

                //        listInfo.Items.Clear();

                //    }
                //}
                sqlOperate.MySqlCom(sql);//20201007修改
            }
            catch
            {
                listInfo.Items.Add(DateTime.Now.ToString() + "catch外网连接失败");
            }


            #endregion



        }

        //每镉2分钟获取产量信息
        private void timer2_Tick(object sender, EventArgs e)
            {
            timer2.Interval = 120000;
            string riqi = DateTime.Now.ToString("yyyy-MM-dd");
            LineOne();
            LineTwo();
            LineThree();
            LineFour();
            label35.Text = riqi;
            label40.Text = (Convert.ToInt32(label36.Text) + Convert.ToInt32(label37.Text) + Convert.ToInt32(label38.Text) + Convert.ToInt32(label39.Text)).ToString();     
        }

        #endregion

        #region   //产量信息上传至阿里云

        private void LineOne()
        {


            string sql = "select * from YM_FINISH_INFO where DT='合计'  ";  
            OracleDataReader read = oraOperate.OrcGetRead(sql);
            read.Read();
            label36.Text = read["TOTAL"].ToString();
       
        }

        private void LineTwo()
        {


            string sql = "select * from YM_FINISH2_INFO where DT='合计'  ";

            OracleDataReader read = oraOperate.OrcGetRead(sql);
            read.Read();
            label37.Text = read["TOTAL"].ToString();
    


        }

        private void LineThree()
        {


            string sql = "select * from YM_FINISH3_INFO where DT='合计'  ";
            OracleDataReader read = oraOperate.OrcGetRead(sql);
            read.Read();
            label38.Text = read["TOTAL"].ToString();
 



        }
        private void LineFour()
        {


            string sql = "select * from YM_FINISH4_INFO where DT='合计'  ";

            OracleDataReader read = oraOperate.OrcGetRead(sql);
            read.Read();
            label39.Text = read["TOTAL"].ToString();
     



        }
        #endregion

        private void dataToAliMysql()
        {

            string riqi = DateTime.Now.ToString("yyyy-MM-dd");
            if (DateTime.Now.Hour == Convert.ToInt32(23) && DateTime.Now.Minute == Convert.ToInt32(59) && DateTime.Now.Second == Convert.ToInt32(00))
            {
                      

                string sql = "insert into production (RIQI,SHULIANG) values('" + riqi+ "','" + label40.Text + "')";


                //检测网络连接状态，网络连接成功后，写入数据

                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

                System.Net.NetworkInformation.PingReply pingStatus = ping.Send(IPAddress.Parse("202.108.22.5"), 1000);//ping 百度的IP地址

                if (pingStatus.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    sqlOperate.MySqlCom(sql);
                    listInfo.Items.Add(riqi + "产量信息添加成功");

                }
                else
                {
                    listInfo.Items.Add(DateTime.Now.ToString() + "产量信息外网连接失败");
                    if (listInfo.Items.Count > 50)
                    {

                        listInfo.Items.Clear();

                    }
                }


            }
               
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

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Interval = 1000;//执行间隔时间,单位为毫秒;此时时间间隔为1秒          
      
            dataToAliMysql();//定点数据插入数据库  

            toolStripStatusLabel1.Text = DateTime.Now.ToLocalTime().ToString();
        }

 
    }
}