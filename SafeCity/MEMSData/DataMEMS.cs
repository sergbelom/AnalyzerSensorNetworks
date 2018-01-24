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

        private static StreamWriter MEMSdata = new StreamWriter(@"E:\DataFromMems" + DateTime.Now.ToString("d-M-yyyy HH-mm-ss") + ".txt");

        //TODO: сделать задание значения полю MEMSdata через свойсвто ниже (при текущей реализации свойства файл не создается)
        /*public StreamWriter MEMSData
        {
            set
            {
                MEMSdata = new StreamWriter(@"E:\DataFromMems" + DateTime.Now.ToString("d-M-yyyy HH-mm-ss") + ".txt");
            }
        }*/

        // поле для номера порта
        private static string portId = "COM3";

        //TODO: сделать задание значения полю portId через свойсвто ниже
        /*public string PortId
        {
            set
            {
                portId = "COM3";
            }
        }*/

        // поле для COM порта
        private static SerialPort MEMSport = new SerialPort(portId, 115200, Parity.None, 8, StopBits.Two);

        //TODO: сделать задание значения полю MEMSport через свойсвто

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
                // ось X
                XL = MEMSport.ReadByte();
                XH = MEMSport.ReadByte();
                XH = (XH << 8) | XL;
                // ось Y
                YL = MEMSport.ReadByte();
                YH = MEMSport.ReadByte();
                YH = (YH << 8) | YL;
                // ось Z
                ZL = MEMSport.ReadByte();
                ZH = MEMSport.ReadByte();
                ZH = (ZH << 8) | ZL;
                // ID сенсоров
                idL = MEMSport.ReadByte();
                idH = MEMSport.ReadByte();
                idH = (idH << 8) | idL;

                byteEnd = MEMSport.ReadByte();

                Console.WriteLine(idH + ";" + XH + ";" + YH + ";" + ZH);

                MEMSdata.WriteLine(idH + ";" + XH + ";" + YH + ";" + ZH);

                i++;
            }

            MEMSdata.Close();
            MEMSport.Close();

        }

    }
}
