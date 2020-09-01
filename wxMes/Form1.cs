﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using comWithPlc;
using mySqlOperate;


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

        

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            timer2.Start();

        }

        //每隔5分钟获取一次PLC的信息
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 600000;//执行间隔时间,单位为毫秒;此时时间间隔为60秒   


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

        }



  

        //每镉5s发布一次消息
        private void timer2_Tick(object sender, EventArgs e)
        {
            

            timer2.Interval = 300000;

            string sqlDel = "delete from oveninfo";
            sqlOperate.MySqlCom(sqlDel);

            string currentTime = DateTime.Now.ToString();

            string sql = "insert into oveninfo values('"+currentTime+"','" + label19.Text + "','" + label25.Text + "','" + label18.Text + "','" + label24.Text + "','" + label17.Text + "','" + label23.Text + "','" + label16.Text + "','" + label22.Text + "','" + label15.Text + "','" + label21.Text + "','" + label14.Text + "','" + label20.Text + "','" + label13.Text + "','" + label5.Text + "')";

          
            sqlOperate.MySqlCom(sql);
        }




        //窗口关闭时，程序后台运行
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
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

        //托盘
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

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
