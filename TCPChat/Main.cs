using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPChat
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            Server sv = new Server();
            sv.Show();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            Client clt = new Client();
            clt.Show();
        }
    }
}
