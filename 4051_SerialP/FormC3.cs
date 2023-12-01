using System;
using System.IO.Ports;
using System.Threading;
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
    //public class PortChat
    //{
        //static bool _continue;
        //static SerialPort _serialPort;
        //public static void Main()
        //{        }
    //}
 //   public class MySerialReader : IDisposable 
 // { }
 //   class SerialPortProg
 // {

    //   SerialPortProg();
    //    }

    public partial class Form1 : Form
    {
        public bool OUT; // needs for checking the state of CTS signal
        public bool _continue;
        int i = 0;
        int offsetIndex = 0;
        byte[] sentData = new byte[10];
        byte[] message = new byte[10];
        byte[] byte_buffer = new byte[2048];
        int receivedValidMessageCounter = 0;
        string trial = "";
        int Freq = 0;

        //public SerialPort _serialPort1;
        // static string name;
        //string message;

        public Form1()
        {
            InitializeComponent();
        }
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
             int bufferSize = serialPort1.BytesToRead;
            if (bufferSize > 0 && offsetIndex + 10 < byte_buffer.Length)
            {
                serialPort1.Read(byte_buffer, offsetIndex, message.Length);
                offsetIndex = offsetIndex + message.Length;     //Read Index
                
            }
            else
            {
                //int a = 0;
            }
        }
            private void button1_Click(object sender, EventArgs e)
        {
            // Makes sure serial port is open before trying to write
            try
            {
                if (!(serialPort1.IsOpen))
                  // Console.WriteLine("Ready for Incoming Data:");
                    serialPort1.Open();
                serialPort1.Write("SI\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening/writing to serial port :: " + ex.Message, "Error!");
            }

           // if (!serialPort1.IsOpen)
             //   serialPort1.Open();

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

        private void button2_Click(object sender, EventArgs e)
        {
            // Set the read/write timeouts
            serialPort1.ReadTimeout = 500;
            serialPort1.WriteTimeout = 500;
            _continue = true;
            // Get a list of serial port names.
            string[] ports = SerialPort.GetPortNames();
                        Console.WriteLine("The following serial ports were found:");
                        // Display each port name to the console.
            foreach (string port in ports)
            {
                //Console.WriteLine(port);
                textBox3.Text = port;
            }
            // Makes sure serial port is open before trying to write
            try
            {
                if (!(serialPort1.IsOpen))
                    // Console.WriteLine("Ready for Incoming Data:");
                // MessageBox.Show("opening the serial port :: " );
                serialPort1.Open();
                serialPort1.Write("SI\r\n");
                //  (RTS = 0, DTR = 0)
                serialPort1.RtsEnable = true; //RTS = 0
                serialPort1.DtrEnable = true; // DTR = 0
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening/writing to serial port :: " + ex.Message, "Error!");
            }

            //while (_continue)
            //{
            //  try
            //{
            timer1.Enabled = true;
                //}
                //catch (TimeoutException) { }
            //}




        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        static private byte CalCheckSum(byte[] byteBuffer)
        {
            byte _CheckSumByte = 0x00;
            for (int a = 0; a < byteBuffer.Length - 1; a++) //CheckSumı headerdan itibaren kontrol ettir!
                _CheckSumByte ^= byteBuffer[a];
                        return _CheckSumByte;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
             int bytesRead = 0;
            // Initialize a buffer to hold the received data
            //serialPort1.Read will return the number of bytes read
                        //serialPort1.Read(result, 0, result.Length);
            //string s = new string(result);
            //char[] result = new char[20];
            //for (int len = 0; len < result.Length;)
            //{
              //  len += serialPort1.Read(result, len, result.Length - len);
            //}
            //byte[] buffer = new byte[serialPort1.ReadBufferSize];
            //bytesRead = serialPort1.ReadBufferSize;         //4096
            //string message = serialPort1.ReadLine();
            string message = serialPort1.ReadExisting();
            //Console.WriteLine(message);
            //textBox2.Text = message + s;
            textBox2.Text = message + bytesRead;
            //MessageBox.Show(s);

            sentData[0] = 0xF1;
            sentData[1]++;
            sentData[2] = 70;
            sentData[3] = 11;
            sentData[4] = 85;
            sentData[5] = 74;
            sentData[6] = 11;
            sentData[7] = 10;
            sentData[8] = 154;
            sentData[9] = CalCheckSum(sentData);
             serialPort1.Write(sentData, 0, 10);
        }
    }
}
