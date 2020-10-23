using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace HDI
{
    public partial class Form1 : Form
    {
        //HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer
        public static RegistryKey policiesExplorer = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", true);
        public static Boolean flag = Convert.ToBoolean(policiesExplorer.GetValue("NoDesktop"));

        public Form1()
        {
            InitializeComponent();
            //this.ShowInTaskbar = false;
            if (flag) notifyIcon1.Text = "Показать РБ";
            else notifyIcon1.Text = "Скрыть РБ";
            this.notifyIcon1.Click += new EventHandler(notifyIcon1_Click);
            this.Resize += new System.EventHandler(this.Form1_Resize);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) this.ShowInTaskbar = false;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            flag = Convert.ToBoolean(policiesExplorer.GetValue("NoDesktop"));
            if (!flag)
            {
                policiesExplorer.SetValue("NoDesktop", 1);
                notifyIcon1.Text = "Показать РБ";
            }
            else
            {
                policiesExplorer.SetValue("NoDesktop", 0);
                notifyIcon1.Text = "Скрыть РБ";
            }
            flag = !flag;
           // policiesExplorer.Close();
            Process.Start("TASKKILL", "/F /IM Explorer.exe");
            System.Threading.Thread.Sleep(3000);
            Process proc = new Process();
            proc.StartInfo.FileName = "C:\\Windows\\explorer.exe";
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }
    }
}