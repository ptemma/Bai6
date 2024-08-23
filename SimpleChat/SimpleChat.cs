using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleChat
{
    public partial class SimpleChat : Form
    {
        private UdpClient udpClient = null;
        Thread thread = null;

        public SimpleChat()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            InputConfig(true);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                udpClient = new UdpClient(int.Parse(txtPortL.Text.Trim()));
                thread = new Thread(new ThreadStart(Receive))
                {
                    IsBackground = true
                };
                thread.Start();
                InputConfig(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("connect "+ ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] msg = new byte[1024];
                msg = Encoding.UTF8.GetBytes(txtMsg.Text);
                udpClient.Send(msg, msg.Length, txtIPR.Text.Trim(), int.Parse(txtPortR.Text.Trim()));
                rtxMsg.Text += "Send: " + txtMsg.Text + "\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] msg = new byte[1024];
                    IPEndPoint ipe = new IPEndPoint(IPAddress.Any, int.Parse(txtPortL.Text.Trim()));
                    msg = udpClient.Receive(ref ipe);
                    string s = Encoding.UTF8.GetString(msg, 0, msg.Length);
                    if (cbFilter.Checked) s = MsgFilter(s);
                    rtxMsg.Text += "Receive: " + s + "\r\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string MsgFilter(string s)
        {
            string[] regex = { ", ", "," };
            string[] filters = txtFilter.Text.Trim().Split(regex, StringSplitOptions.RemoveEmptyEntries);
            foreach (var filter in filters)
            {
                if (s.Contains(filter)) s = "Filtered";
            }
            return s;
        }



        private void InputConfig(bool state)
        {
            btnConnect.Enabled = state;
            txtIPR.ReadOnly = !state;
            txtPortL.ReadOnly = !state;
            txtPortR.ReadOnly = !state;

            btnSend.Enabled = !state;
        }

        private void SimpleChat_Load(object sender, EventArgs e)
        {

        }
    }
}
