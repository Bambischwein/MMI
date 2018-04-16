using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MMITest
{
    public partial class StartGui : Form
    {
        public StartGui()
        {
            InitializeComponent();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton1.Enabled = true;
            radioButton1.Text = "Gewichtet";
        }
    }
}
