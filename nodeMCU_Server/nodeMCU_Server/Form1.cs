using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Runtime.InteropServices;
using nodeCU_Server;

namespace nodeMCU_Server
{   
    public partial class Form1 : Form
    {
        private gmksoft_server Server;

        private delegate void UpdateTxtBox(RichTextBox txtbox, string txt);

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public Form1()
        {
            InitializeComponent();
            this.Text = "Server Listener";
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string StringHost;
            string StrIP;

            try
            {
                StringHost = Dns.GetHostName();
                StrIP = Dns.GetHostByName(StringHost).AddressList[0].ToString();
                label1.Text = "Host Name:" + StringHost + "  " + "IP Address:" + StrIP;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception thrown: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Server = new gmksoft_server();

            Server.Message += RecInfo;
        }

        private void UpdateTxt(RichTextBox txtbox, string txt)
        {

            if (InvokeRequired)
                txtbox.Invoke(new UpdateTxtBox(UpdateTxt), new object[] { richTextBox1, txt });               
            else if (txt != null)
                txtbox.AppendText(txt + "\n");

        }
        private void RecInfo(gmksoft_server sender, string data)
        {
            UpdateTxt(richTextBox1, data);
        }

        private void btn_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_maximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
