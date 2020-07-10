using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using SnpSystem.Vision.VisionConfigurationHelper;

namespace SnpSmartVision
{
    #region 알람포트용 시리얼통신
    class ControlPort
    {
        public SerialPort port = null;
        string portName;
        RootConfigFile rootFile;

        public ControlPort(RootConfigFile rootFile)
        {
            this.portName = rootFile.Controlport;
            this.rootFile = rootFile;
        }

        public bool PortOpen()
        {
            if (rootFile.Alarm == false) return false;
            
            try
            {
                port = new SerialPort(portName);
                port.PortName = portName;
                port.BaudRate = 38400;
                port.DataBits = 8;
                port.StopBits = StopBits.One;
                port.Parity = Parity.None;
                port.Handshake = Handshake.None;
                port.ReadBufferSize = 100;
                port.Open();
                port.DiscardInBuffer();
                return true;

            }
            catch
            {
                port = null;
                MessageBox.Show("알람용 포트를 열 수 없습니다", "알람포트 열기 오류", MessageBoxButtons.OK);
                return false;
            }
        }

        public bool PortClose()
        {
            if (port == null) return true;
            if (port.IsOpen) port.Close();
            return true;
        }


        // @55AA55AA ;
        public bool SendAlarm()
        {
            if (rootFile.Alarm == false) return false;
            if (port == null) return false;
            if (port.IsOpen == false) return false;

            byte[] txbuffer = new byte[64];
            string txFrame = "@0155AAFE;";
            
            txFrame += rootFile.Lane.ToString("D1");
   
            txbuffer = Encoding.Default.GetBytes(txFrame);
            port.Write(txbuffer, 0, 10);
            return true;
        }

    }
    #endregion
    #region 출력포트용 시리얼 통신
    class SerialOut
    {
        public SerialPort port = null;
        string portName;
        RootConfigFile rootFile;

        public int PacketSize;
        public string SendingPacket;

        public SerialOut(RootConfigFile rootFile)
        {
            this.portName = rootFile.Outport;
            this.rootFile = rootFile; 
        }
        public bool PortOpen()
        {
            try
            {
                port = new SerialPort(portName);
                port.PortName = portName;
                port.BaudRate = 38400;
                port.DataBits = 8;
                port.StopBits = StopBits.One;
                port.Parity = Parity.None;
                port.Handshake = Handshake.None;
                port.ReadBufferSize = 100;
                port.Open();
                port.DiscardInBuffer();
                return true;

            }
            catch
            {
                port = null;
                MessageBox.Show("출력포트를 열 수 없습니다", "출력포트 열기 오류", MessageBoxButtons.OK);
                return false;
            }
        }

        public bool PortClose()
        {
            if (port == null) return true;
            if (port.IsOpen) port.Close();
            return true;
        }
      
        //Protocol to Master PC
        //Header:  @
        //1st Lane No:    1byte
        //2nd Lane No:    1byte
        //1st Lane AR:    3byte
        //1st Lane AREA:  5byte
        //1st Lane Color: 3byte
        //2nd Lane AR:    3byte
        //2nd Lane AREA:  5byte
        //2nd Lane Color: 3byte
        //TRAILER: ; 

        public bool SendDatabyStandardProtocol(ProcessingValue[] value)
        {
            if (port == null) return false;
            if (port.IsOpen == false) return false;
            byte[] txbuffer = new byte[64];
            string txFrame = "@";
            txFrame += rootFile.Lane.ToString("D1");
            for (int i = 0; i < rootFile.Lane; i++)
            {
                int color = value[i].ColorRatio/10;
                if (color >= 100) color = 99;
                txFrame += color.ToString("D2");
            }
            txFrame += ";";
            txbuffer = Encoding.Default.GetBytes(txFrame);
            if (rootFile.Lane == 1) port.Write(txbuffer, 0, PacketSize=5);
            else if (rootFile.Lane == 2) port.Write(txbuffer, 0, PacketSize=7);
           
            SendingPacket = txFrame;
            return true;
        }
        public bool SendDatabyExtendedProtocol(ProcessingValue[] value)
        {
            if (port == null) return false;
            if (port.IsOpen == false) return false;

            byte[] txbuffer = new byte[64];
            string txFrame = "@";

            txFrame += rootFile.LaneNumber[0].ToString("D1");
            int laneCount = rootFile.Lane;
            if (laneCount == 1)   txFrame += 0.ToString("D1");
            else                  txFrame += rootFile.LaneNumber[0].ToString("D1");

            if (value[0].noFruit == false)
            {
                txFrame += (value[0].AspectRatio/10).ToString("D3");
                txFrame += value[0].Size.ToString("D5");
                txFrame += (value[0].ColorRatio/10).ToString("D3");
            }
            else
            {
                txFrame += 0.ToString("D3");
                txFrame += 0.ToString("D5");
                txFrame += 0.ToString("D3");
            }

            if (laneCount == 1 || value[1].noFruit==true)
            {
                txFrame += 0.ToString("D3");
                txFrame += 0.ToString("D5");
                txFrame += 0.ToString("D3");
            }
            else
            {
                txFrame += (value[1].AspectRatio/10).ToString("D3");
                txFrame += value[1].Size.ToString("D5");
                txFrame += (value[1].ColorRatio/10).ToString("D3");
            }
            txFrame += ";";
            txbuffer = Encoding.Default.GetBytes(txFrame);
            port.Write(txbuffer, 0, PacketSize=26);
            SendingPacket = txFrame;
            return true;
        }
    }
}
    #endregion
