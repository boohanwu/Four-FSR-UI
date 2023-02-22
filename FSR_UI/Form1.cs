using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace FSR_UI
{
    public partial class Form1 : Form
    {
        Image myImage;
        Image coordinate;
        Graphics gra;
        int r = 6;
        string SerialData;
        string[] buffer;
        string[] data;
        string[] baudrate = new string[] { "9600", "115200" };
        double scale = 0.1368; // 140/1023
        int U = 0, D = 0, L = 0, R = 0;
        int V_Force, H_Force;
        int total;
        int x, y;

       

        public Form1()
        {
            InitializeComponent();
            Pen pen = new Pen(Color.Red, r);
            myImage = new Bitmap("C://Users//brian//Desktop//FSR_UI//FSR_UI//square.bmp");//C:\Users\brian\Desktop\FSR_UI\FSR_UI
            coordinate = new Bitmap("C://Users//brian//Desktop//FSR_UI//FSR_UI//coordinate.bmp");
            gra = Graphics.FromImage(myImage);
            gra.DrawEllipse(pen, 150 - r/2, 150 - r/2, r, r);
            pictureBox1.Image = myImage;
            pictureBox2.Image = coordinate;
            serialPort1.BaudRate = 115200;
            progressBar1.Value = 0;
            textBox1.Text = Convert.ToString(U);
            textBox2.Text = Convert.ToString(D);
            textBox3.Text = Convert.ToString(R);
            textBox4.Text = Convert.ToString(L);
            textBox5.Text = Convert.ToString(0);
            comboBox_Port.Items.AddRange(SerialPort.GetPortNames());
            comboBox_Baudrate.Items.AddRange(baudrate);
            comboBox_Port.Text = "COM3";
            comboBox_Baudrate.Text = "115200";
            BTN_Dis.Enabled = false;
            label1.Text = "Waiting to Connect";

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox_Port_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox_Port.Text;
        }

        private void BTN_Dis_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            comboBox_Port.Enabled = true;
            comboBox_Baudrate.Enabled = true;
            BTN_Connect.Enabled = true;
            BTN_Dis.Enabled = false;
            label1.Text = "Waiting to Connect";
        }

        private void BTN_Connect_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox_Port.Text;
            serialPort1.BaudRate = Convert.ToInt32(comboBox_Baudrate.Text);
            serialPort1.Open();
            comboBox_Port.Enabled = false;
            comboBox_Baudrate.Enabled = false;
            BTN_Connect.Enabled = false;
            BTN_Dis.Enabled = true;
            label1.Text = "Connect Sucess!!";
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialData = serialPort1.ReadExisting();
            this.BeginInvoke(new EventHandler(readdata));
        }

        private void readdata(object sender, EventArgs e)
        {
            buffer = SerialData.Split('\n');
            if (buffer.Length > 1)
                data = buffer[buffer.Length - 2].Split(',');
            else
                data = buffer[buffer.Length - 1].Split(',');
            U = Int16.Parse(data[0]);
            D = Int16.Parse(data[1]);
            R = Int16.Parse(data[2]);
            L = Int16.Parse(data[3]);
            H_Force = R - L;
            V_Force = D - U;
            total = U + D + R + L;
            progressBar1.Value = total;
            textBox1.Text = Convert.ToString(U);
            textBox2.Text = Convert.ToString(D);
            textBox3.Text = Convert.ToString(R);
            textBox4.Text = Convert.ToString(L);
            textBox5.Text = Convert.ToString(total);
            x = 150 + Convert.ToInt16(Math.Round(H_Force * scale));
            y = 150 + Convert.ToInt16(Math.Round(V_Force * scale));

            myImage = new Bitmap("C://Users//brian//Desktop//FSR_UI//FSR_UI//square.bmp");
            gra = Graphics.FromImage(myImage);
            Pen pen = new Pen(Color.Red, r);
            gra.DrawEllipse(pen, x-r/2, y-r/2, r, r);
            pictureBox1.Image = myImage;
        }
    }
}
