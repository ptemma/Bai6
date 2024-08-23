using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPChat
{
    public partial class Server : Form
    {
        private static TcpListener server;
        private static readonly string Dir = "D:/B2014625/";
        private static readonly string Extension = ".txt";

        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            txtPort.Text = "4444";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(StartMutiThread))
            {
                IsBackground = true
            };

            thread.Start();
            rtxInfo.Text += "Khoi dong server thanh cong!\r\n";
            btnStart.Enabled = false;
            btnStart.Text = "Started";
        }

        private void StartMutiThread()
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, int.Parse(txtPort.Text.Trim()));
            server = new TcpListener(ipe);
            server.Start();

            try
            {
                while (true)
                {
                    Socket socket = server.AcceptSocket();
                    rtxInfo.Text += socket.RemoteEndPoint.ToString() + " Da ket noi! \r\n";
                    Thread t = new Thread(threadSocket => Listener((Socket)threadSocket));
                    t.Start(socket);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Listener(Socket soc)
        {
            try
            {
                NetworkStream stream = new NetworkStream(soc);
                StreamReader sr = new StreamReader(stream);
                StreamWriter sw = new StreamWriter(stream);

                while (true)
                {
                    string data = sr.ReadLine();
                   if (data != null)
                    {
                        string[] arr = data.Split('#');
                        string res = "";
                        switch (arr[0])
                        {
                            case "REG":
                                res = REG(arr[1], arr[2]);
                                sw.WriteLine(res);
                                sw.Flush();
                                break;

                            case "SEN":
                                res = SEN(arr[1], arr[2]);
                                sw.WriteLine(res);
                                sw.Flush();
                                break;

                            case "GET":
                                res = GET(arr[1], arr[2]);
                                sw.WriteLine(res);
                                sw.Flush();
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                rtxInfo.Text += "Co loi xay ra" + ex.Message;
            }
        }

        private string GET(string username, string password)
        {
            string path = Dir + username + Extension;
            if (File.Exists(path))
            {
                var data = File.ReadAllLines(path);
                string res = "";
                if (data[data.Length - 1].ToString() == password)
                {
                    for (int i = 0; i < data.Length - 1; i++)
                    {
                        res += data[i] + '#';
                    }
                    return res;
                }
                else return "104. Sai mat khau dang nhap";
            }
            else return "103. Tai khoan khong ton tai";
        }

        private string SEN(string username, string message)
        {
            string path = Dir + username + Extension;
            if (File.Exists(path))
            {
                try
                {
                    string[] data = File.ReadAllLines(path);
                    File.Delete(path);
                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                    StreamWriter writer = new StreamWriter(fs);
                    writer.WriteLine(message);
                    foreach (string line in data)
                    {
                        writer.WriteLine(line);
                    }
                    writer.Flush();
                    fs.Close();
                }
                catch (Exception e)
                {
                    rtxInfo.Text += "Co loi xay ra" + e.Message;
                }

                return "Gui tin nhan thanh cong";
            }
            else return "103. Tai khoan khong ton tai";
        }

        private string REG(string username, string password)
        {
            string path = Dir + username + Extension;
            if (!File.Exists(path))
            {
                try
                {
                    FileStream fs = new FileStream(path, FileMode.Create);
                    StreamWriter writer = new StreamWriter(fs);
                    writer.WriteLine(password);
                    writer.Flush();
                    fs.Close();
                }
                catch (Exception e)
                {
                    rtxInfo.Text += "Co loi xay ra" + e.Message;
                }
                return "Dang ky thanh cong";
            }
            else return "102. Tai khoan da ton tai";
        }

        private void Server_Load(object sender, EventArgs e)
        {

        }
    }
}
