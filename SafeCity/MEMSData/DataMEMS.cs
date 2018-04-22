using System;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

namespace SafeCity
{
    public class DataMEMS
    {

        // поля для выходный данных
        private static StreamWriter MEMSdata = new StreamWriter(@"E:\DataFromMems.txt");
        // поле для номера порта
        public static string portId;// = "COM3";
        // поле для COM порта
        public static SerialPort MEMSport;// = new SerialPort(portId, 115200, Parity.None, 8, StopBits.Two);

        public static void OpenPort( bool isOpen )
        {
            if (isOpen)
            {
                MEMSport.Open();
            }
            else
            {
                MEMSdata.Close();
            }
        }


        // МЕТОД ReadDataFromMEMS:
        // открывает COM порт
        // считывает вектор данных между байтами 102 и 105
        // из сигналов L и H по всем осям делает один для каждой оси
        // выводит значения для каждого сенсора по его ID на консоль и в файл, расположенный в MEMSdata
        public static int[] ReadDataFromMEMS()
        {            

            // определяем переменные для считанных сигналов по X Y Z также для метки начала, конца вектора измерений и ID сенсоров
            int byteEnd, byteInit;
            int XL, XH; // высокий и низкий сигнал для X
            int YL, YH; // высокий и низкий сигнал для Y
            int ZH, ZL;  // высокий и низкий сигнал для Z
            int idH, idL; // сигналы для ID сенсоров

            int[] resultDataMems = new int[4];

            //int i = 0;
            //while (i < 100)
            //{
            // вектор с даннами имеет отметки 105 и 102
            // читаем байты до появления байта 105
            /*while ((byteInit = MEMSport.ReadByte()) != 105 && i == 0)
            {
            }*/

                while ((byteInit = MEMSport.ReadByte()) != 105)
                {
                }
                // ось X
                XH = MEMSport.ReadByte();
                XL = MEMSport.ReadByte();

                XH = (XH << 8) | XL;

                YH = MEMSport.ReadByte();
                YL = MEMSport.ReadByte();

                YH = (YH << 8) | YL;

                ZH = MEMSport.ReadByte();
                ZL = MEMSport.ReadByte();

                ZH = (ZH << 8) | ZL;

                idL = MEMSport.ReadByte();
                idH = MEMSport.ReadByte();
                idH = (idH << 8) | idL;

                byteEnd = MEMSport.ReadByte();
             
                //Console.WriteLine(idH + " " + XH + " " + YH + " " + ZH);
                //TODO: разобарться с котсылем: почему в X, Y и Z попадают значения > 65000
                // если измерение > 65000 то исключается вся строка измерений

                MEMSdata.WriteLine( idH + " " + XH + " " + YH + " " + ZH);
                // должен быть вывод в файл одновременно с отрисовкой, при этом по кнопке Run этот файл должен пересоздаваться
                // далее выходной файл будет является входным для анализа в R

                //resultDataMems[0] = i;
                resultDataMems[0] = idH;
                resultDataMems[1] = XH;
                resultDataMems[2] = YH;
                resultDataMems[3] = ZH;

                //i++;
                // сдлеать проверку после слип на новы ID

                // }
            
            //MEMSport.Close();

            return resultDataMems;

        }
    }
}
