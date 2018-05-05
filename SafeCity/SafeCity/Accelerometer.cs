using System;
using System.IO.Ports;
using System.Text;

namespace SafeCity
{
    public static class Accelerometer
    {
        public static int[] GetData(SerialPort serialPort)
        {
            // определяем переменные для считанных сигналов по X Y Z также для метки начала, конца вектора измерений и ID сенсоров
            int byteEnd, byteInit;
            int XL, XH; // высокий и низкий сигнал для X
            int YL, YH; // высокий и низкий сигнал для Y
            int ZH, ZL;  // высокий и низкий сигнал для Z
            int idH, idL; // сигналы для ID сенсоров

            while ((byteInit = serialPort.ReadByte()) != 105) { }

            XH = serialPort.ReadByte();
            XL = serialPort.ReadByte();
            XH = (XH << 8) | XL;

            YH = serialPort.ReadByte();
            YL = serialPort.ReadByte();
            YH = (YH << 8) | YL;

            ZH = serialPort.ReadByte();
            ZL = serialPort.ReadByte();
            ZH = (ZH << 8) | ZL;

            idL = serialPort.ReadByte();
            idH = serialPort.ReadByte();
            idH = (idH << 8) | idL;

            byteEnd = serialPort.ReadByte();
            return new int[] { idH, XH, YH, ZH };
        }
    }
}
