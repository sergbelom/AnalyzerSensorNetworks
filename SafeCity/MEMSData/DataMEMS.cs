using System;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

namespace TestUSBSerialConverter
{
    class DataMEMS
    {
        // поля для выходный данных
        static StreamWriter MEMSdata = new StreamWriter(@"E:\DataFromMems.txt");
        // поле для номера порта
        static string portId = "COM3";
        // поле для COM порта
        static SerialPort MEMSport = new SerialPort(portId, 115200, Parity.None, 8, StopBits.Two);

        // МЕТОД ReadDataFromMEMS:
        // открывает COM порт
        // считывает вектор данных между байтами 102 и 105
        // из сигналов L и H по всем осям делает один для каждой оси
        // выводит значения для каждого сенсора по его ID на консоль и в файл, расположенный в MEMSdata
        public static void ReadDataFromMEMS()
        {
            MEMSport.Open();

            // определяем переменные для считанных сигналов по X Y Z также для метки начала, конца вектора измерений и ID сенсоров
            int byteEnd, byteInit;
            int XL, XH; // высокий и низкий сигнал для X
            int YL, YH; // высокий и низкий сигнал для Y
            int ZH, ZL;  // высокий и низкий сигнал для Z
            int idH, idL; // сигналы для ID сенсоров

            int i = 0;
            while (i < 5000)
            {
                // вектор с даннами имеет отметки 105 и 102
                // читаем байты до появления байта 105
                while ((byteInit = MEMSport.ReadByte()) != 105 && i == 0)
                {
                }
                XL = MEMSport.ReadByte();
                XH = MEMSport.ReadByte();

                XH = (XH << 8) | XL;

                YL = MEMSport.ReadByte();
                YH = MEMSport.ReadByte();

                YH = (YH << 8) | YL;

                ZL = MEMSport.ReadByte();
                ZH = MEMSport.ReadByte();

                ZH = (ZH << 8) | ZL;

                idL = MEMSport.ReadByte();
                idH = MEMSport.ReadByte();

                idH = (idH << 8) | idL;

                byteEnd = MEMSport.ReadByte();

                Console.WriteLine(idH + " " + XH + " " + YH + " " + ZH);

                MEMSdata.WriteLine(idH + " " + XH + " " + YH + " " + ZH);

                i++;
            }

            MEMSdata.Close();
            MEMSport.Close();

        }

    }
}
