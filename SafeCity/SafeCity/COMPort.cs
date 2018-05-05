using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SafeCity
{
    public static class COMPort
    {
        public static SerialPort Connect(SerialPort serialPort)
        {
            try
            {
                serialPort = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.Two);
                serialPort.Open();
                return serialPort;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public static void Disconnect(SerialPort serialPort)
        {
            serialPort.Close();
        }
    }
}
