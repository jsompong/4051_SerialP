using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _4051_SerialP
{
    public partial class Form1 : Form
    {
        public bool OUT; // needs for checking the state of CTS signal
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
                serialPort1.Open();

            textBox1.Text = " ";

            // CHANNEL X0 (RTS = 0, DTR = 0)
            serialPort1.RtsEnable = true; //RTS = 0
            serialPort1.DtrEnable = true; // DTR = 0
            WriteStatus();
            // CHANNEL X1 (RTS = 1, DTR = 0)
            serialPort1.RtsEnable = false; //RTS = 1
            serialPort1.DtrEnable = true; // DTR = 0
            WriteStatus();
            // CHANNEL X2 (RTS = 0, DTR = 1)
            serialPort1.RtsEnable = true; //RTS = 0
            serialPort1.DtrEnable = false; // DTR = 1
            WriteStatus();
            // CHANNEL X3 (RTS = 1, DTR = 1)
            serialPort1.RtsEnable = false; //RTS = 1
            serialPort1.DtrEnable = false; // DTR = 1
            WriteStatus();
            serialPort1.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void WriteStatus()
        {
            OUT = serialPort1.CtsHolding;
            if (!OUT) textBox1.Text = textBox1.Text + "1";
            else textBox1.Text = textBox1.Text + "0";
        }

    }
}
