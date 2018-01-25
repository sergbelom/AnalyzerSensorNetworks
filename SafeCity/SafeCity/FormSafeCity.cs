using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SafeCity
{
    public partial class FormSafeCity : Form
    {
        public FormSafeCity()
        {
            InitializeComponent();
        }

        private void buttonGetData_Click_1(object sender, EventArgs e)
        {
            try
            {
                //при нажатии на кнопку происходит вызов статического метода ReadDataFromMEMS() для считывания показаний с акселерометров
                DataMEMS.ReadDataFromMEMS();
            }
            catch(Exception ex)
            {
                listBoxLog.Items.Add(ex.Message + " " + DateTime.Now.ToLongTimeString());
                listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
            }
        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream fs = null;
            StreamReader sr = null;
            OpenFileDialog info = new OpenFileDialog();
            info.Title = "Openning Pipeline Network";
            info.Filter = "Text Document |*.txt";
            info.ShowDialog();
            try
            {
                fs = new FileStream(info.FileName, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs);

                listBoxLog.Items.Add("Началось считывание данных с файла " + info.FileName + " " + DateTime.Now.ToLongTimeString());
                listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;

                var sensor1Signal = pictureBoxSensor1Signal.CreateGraphics();
                var sensor2Signal = pictureBoxSensor2Signal.CreateGraphics();

                int gx1 = 0;
                int gy1 = 0;
                int gxx1 = 2;

                int bx1 = 0;
                int by1 = 0;
                int bxx1 = 2;

                int rx1 = 0;
                int ry1 = 0;
                int rx11 = 2;

                int gx2 = 0;
                int gy2 = 0;
                int gxx2 = 2;

                int bx2 = 0;
                int by2 = 0;
                int bxx2 = 2;

                int rx2 = 0;
                int ry2 = 0;
                int rxx2 = 2;

                int sensorId;
                int i = 0;
                string[] arrayCoordinates;
                arrayCoordinates = sr.ReadLine().Split(' ');

                while (i < 3600)
                {
                    sensorId = Convert.ToInt32(arrayCoordinates[1]);
                    sensor1Signal.DrawLine(new Pen(Color.Green, 1), gx1, gy1, gxx1, Convert.ToInt32(arrayCoordinates[2]) / 500);
                    gx1 = gxx1;
                    gy1 = Convert.ToInt32(arrayCoordinates[2]) / 500;
                    gxx1 = gxx1 + 2;

                    sensor1Signal.DrawLine(new Pen(Color.Blue, 1), bx1, by1, bxx1, Convert.ToInt32(arrayCoordinates[3]) / 500);
                    bx1 = bxx1;
                    by1 = Convert.ToInt32(arrayCoordinates[3]) / 500;
                    bxx1 = bxx1 + 2;

                    sensor1Signal.DrawLine(new Pen(Color.Red, 1), rx1, ry1, rx11, Convert.ToInt32(arrayCoordinates[4]) / 500);
                    rx1 = rx11;
                    ry1 = Convert.ToInt32(arrayCoordinates[4]) / 500;
                    rx11 = rx11 + 2;

                    arrayCoordinates = sr.ReadLine().Split(' ');

                    if(sensorId != Convert.ToInt32(arrayCoordinates[1]))
                    {
                        int xx22 = Convert.ToInt32(arrayCoordinates[2]) / 500;
                        sensor2Signal.DrawLine(new Pen(Color.Green, 1), gx2, gy2, gxx2, Convert.ToInt32(arrayCoordinates[2]) / 500);
                        gx2 = gxx2;
                        gy2 = Convert.ToInt32(arrayCoordinates[2]) / 500;
                        gxx2 = gxx2 + 2;

                        sensor2Signal.DrawLine(new Pen(Color.Blue, 1), bx2, by2, bxx2, Convert.ToInt32(arrayCoordinates[3]) / 500);
                        bx2 = bxx2;
                        by2 = Convert.ToInt32(arrayCoordinates[3]) / 500;
                        bxx2 = bxx2 + 2;

                        sensor2Signal.DrawLine(new Pen(Color.Red, 1), rx2, ry2, rxx2, Convert.ToInt32(arrayCoordinates[4]) / 500);
                        rx2 = rxx2;
                        ry2 = Convert.ToInt32(arrayCoordinates[4]) / 500;
                        rxx2 = rxx2 + 2;

                        arrayCoordinates = sr.ReadLine().Split(' ');
                    }
                    if (i % 260 == 0)
                    {
                        sensor1Signal.Clear(Color.White);
                        sensor2Signal.Clear(Color.White);
                        gx1 = 0;
                        gxx1 = 2;

                        bx1 = 0;
                        bxx1 = 2;

                        rx1 = 0;
                        rx11 = 2;

                        gx2 = 0;
                        gxx2 = 2;

                        bx2 = 0;
                        bxx2 = 2;

                        rx2 = 0;
                        rxx2 = 2;
                    }
                    i++;
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(ex.Message + " " + DateTime.Now.ToLongTimeString());
                listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
            }
            finally
            {
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();

                listBoxLog.Items.Add("Cчитывание данных с файла " + info.FileName + " завершилось " + DateTime.Now.ToLongTimeString());
                listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
            }
        }
    }
}
