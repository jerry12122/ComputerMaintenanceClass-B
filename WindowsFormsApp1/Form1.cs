using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;
using LibUsbDotNet.Main;
using LibUsbDotNet;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        byte[] green = new byte[8] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
        int btn, cnt;
        UsbDevice dev;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //---------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            btn = 1;
            cnt = 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            btn = 2;
            cnt = 0;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            btn = 3;
        }
        //---------------------------------------------------------------------------------
        private OvalShape G(int i)
        {
            OvalShape[] a = new OvalShape[] { ovalShape1, ovalShape2, ovalShape3, ovalShape4, ovalShape5, ovalShape6, ovalShape7, ovalShape8 };
            return a[7-i];
        }
        private OvalShape R(int i)
        {
            OvalShape[] a = new OvalShape[] { ovalShape9, ovalShape10, ovalShape11, ovalShape12, ovalShape13, ovalShape14, ovalShape15, ovalShape16 };
            return a[7-i];
        }
        //---------------------------------------------------------------------------------
        public void ShowG(int n)
        {

            for (int i = 0; i < 8; i++)
            {
                G(i).BackStyle = BackStyle.Opaque;
                if (n % 2 == 1)
                {
                    G(i).FillColor = Color.FromArgb(255, 0, 255, 0);
                }
                else
                {
                    G(i).FillColor = Color.FromArgb(255, 0, 128, 0);
                }
                n = n / 2;
            }
        }
        public void ShowR(int n)
        {

            for (int i = 0; i < 8; i++)
            {
                R(i).BackStyle = BackStyle.Opaque;
                if (n % 2 == 1)
                {
                    R(i).FillColor = Color.FromArgb(255, 255, 0, 0);
                }
                else
                {
                    R(i).FillColor = Color.FromArgb(255, 128, 0, 0);
                }
                n = n / 2;
            }
        }
        //---------------------------------------------------------------------------------

        private static void Usbout(UsbDevice dev, byte data, byte ctrl)
        {

            int transferred = 0;
            byte[] dataBuffer = new byte[2] { data, ctrl };
            UsbSetupPacket UsbSetup = new UsbSetupPacket(32, 9, 0, 0, 0);
            dev.ControlTransfer(ref UsbSetup, dataBuffer, 8 , out transferred);

        }
        //---------------------------------------------------------------------------------

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "Current Time:" + DateTime.Now.ToString("HH:mm:ss");
            dev = UsbDevice.OpenUsbDevice(new UsbDeviceFinder(0x1234, 0x6789));

            
            if (dev == null)
            {
                for (int i = 0; i < 8; i++)
                {
                    R(i).BackStyle = BackStyle.Transparent;
                    G(i).BackStyle = BackStyle.Transparent;
                }
                if (btn == 3)
                {
                    this.Close();
                }
            }
            else
            {
                ShowG(0);
                ShowR(0);
                Usbout(dev, 0, 0);
                Usbout(dev, 0, 0x10);
                if (btn == 1 && cnt < green.Length)
                {
                    ShowG(green[cnt]);
                    Usbout(dev, green[cnt], 0);
                    cnt++;
                }
                else if (btn == 2 && cnt < 8)
                {
                    int n = (int)Math.Pow(2, cnt);
                    ShowR(n);
                    Usbout(dev, (byte)n, 0x20);
                    Usbout(dev, (byte)n, 0x30);
                    cnt++;
                }
                else if (btn == 3)
                {
                    dev.Close();
                    this.Close();
                }

            }
        }
    }
}
