using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPChat
{
    public partial class Client : Form
    {
        TcpClient tcpClient;
        StreamReader sr;
        StreamWriter sw;


        public Client()
        {
            InitializeComponent();
            txtPass.PasswordChar = '*';
            FormClosing += new FormClosingEventHandler(CloseHandler);
            txtIP.Text = "127.0.0.1";
            txtPort.Text = "4444";
            ButtonConfig(true);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(txtIP.Text.Trim()), int.Parse(txtPort.Text.Trim()));
                tcpClient = new TcpClient();
                tcpClient.Connect(ipe);

                sr = new StreamReader(tcpClient.GetStream());
                sw = new StreamWriter(tcpClient.GetStream());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ButtonConfig(false);
        }

        private void CloseHandler(Object sender, FormClosingEventArgs e)
        {
            sr.Close(); sw.Close(); tcpClient.Close();
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            string data = "REG#"+ txtUser.Text.Trim()+"#"+txtPass.Text.Trim();
            SendData(data);
        }

        private void btnSen_Click(object sender, EventArgs e)
        {
            string data = "SEN#" + txtUser.Text.Trim() + "#" + txtMsg.Text.Trim();
            SendData(data);
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            string data = "GET#" + txtUser.Text.Trim() + "#" + txtPass.Text.Trim();
            SendData(data);
        }

        private void SendData(string data)
        {
            try
            {
                sw.WriteLine(data);
                sw.Flush();
                string response = sr.ReadLine();
                if (response != null)
                {
                    string[] msg = response.Split('#');
                    if (msg.Count() > 1)
                    {
                        foreach (var item in msg)
                        {
                            if (item != "") rtxMsg.Text += "Tin nhan moi: " + item + "\r\n";
                        }
                    }
                    else
                    {
                        rtxMsg.Text += response + "\r\n";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ButtonConfig(bool state)
        {
            btnConnect.Enabled = state;
            btnGet.Enabled = !state;
            btnReg.Enabled = !state;
            btnSen.Enabled = !state; 
        }
    }
}
