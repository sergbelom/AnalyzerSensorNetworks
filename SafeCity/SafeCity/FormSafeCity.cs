using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace SafeCity
{
    public partial class FormSafeCity : Form
    {
        //SerialPort serialPort = new SerialPort();
        private bool connect = true;
        public FormSafeCity()
        {
            InitializeComponent();
            chart1.Series["Series1"].Label = "";
            chart1.Series["Series1"].IsVisibleInLegend = false;
            chart2.Series["Series1"].IsVisibleInLegend = false;
            chart3.Series["Series1"].IsVisibleInLegend = false;
            chart4.Series["Series1"].IsVisibleInLegend = false;
            chart5.Series["Series1"].IsVisibleInLegend = false;
            chart6.Series["Series1"].IsVisibleInLegend = false;
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
                listBoxLog.Items.Add(DateTime.Now.ToLongTimeString() + " " + ex.Message);
                listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
            }
        }

        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series["Series1"].ChartType = SeriesChartType.Area;
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

                listBoxLog.Items.Add(DateTime.Now.ToLongTimeString() + " Началось считывание данных с файла " + info.FileName);
                listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;

                var sensor1Signal = pictureBoxSensor1Signal.CreateGraphics();
                var sensor2Signal = pictureBoxSensor2Signal.CreateGraphics();

                int greenXSensor1 = 0;
                int greenYSensor1 = 0;
                int greenShiftSensor1 = 2;

                int blueXSensor1 = 0;
                int blueYSensor1 = 0;
                int blueShiftSensor1 = 2;

                int redXSensor1 = 0;
                int redYSensor1 = 0;
                int redx11 = 2;

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
                sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 60), new Point(800, 60));
                sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 120), new Point(800, 120));
                sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 180), new Point(800, 180));
                sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 60), new Point(800, 60));
                sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 120), new Point(800, 120));
                sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 180), new Point(800, 180));

                while (i < 3800)
                {
                    sensorId = Convert.ToInt32(arrayCoordinates[0]);
                    sensor1Signal.DrawLine(new Pen(Color.Green, 1), greenXSensor1, greenYSensor1, greenShiftSensor1, Convert.ToInt32(arrayCoordinates[1]) / 250);
                    greenXSensor1 = greenShiftSensor1;
                    greenYSensor1 = Convert.ToInt32(arrayCoordinates[1]) / 250;
                    greenShiftSensor1 = greenShiftSensor1 + 2;

                    sensor1Signal.DrawLine(new Pen(Color.Blue, 1), blueXSensor1, blueYSensor1, blueShiftSensor1, Convert.ToInt32(arrayCoordinates[2]) / 250);
                    blueXSensor1 = blueShiftSensor1;
                    blueYSensor1 = Convert.ToInt32(arrayCoordinates[2]) / 250;
                    blueShiftSensor1 = blueShiftSensor1 + 2;

                    sensor1Signal.DrawLine(new Pen(Color.Red, 1), redXSensor1, redYSensor1, redx11, Convert.ToInt32(arrayCoordinates[3]) / 250);
                    redXSensor1 = redx11;
                    redYSensor1 = Convert.ToInt32(arrayCoordinates[3]) / 250;
                    redx11 = redx11 + 2;

                    arrayCoordinates = sr.ReadLine().Split(' ');

                    if(sensorId != Convert.ToInt32(arrayCoordinates[0]))
                    {
                        int xx22 = Convert.ToInt32(arrayCoordinates[1]) / 500;
                        sensor2Signal.DrawLine(new Pen(Color.Green, 1), gx2, gy2, gxx2, Convert.ToInt32(arrayCoordinates[1]) / 250);
                        gx2 = gxx2;
                        gy2 = Convert.ToInt32(arrayCoordinates[1]) / 250;
                        gxx2 = gxx2 + 2;

                        sensor2Signal.DrawLine(new Pen(Color.Blue, 1), bx2, by2, bxx2, Convert.ToInt32(arrayCoordinates[2]) / 250);
                        bx2 = bxx2;
                        by2 = Convert.ToInt32(arrayCoordinates[2]) / 250;
                        bxx2 = bxx2 + 2;

                        sensor2Signal.DrawLine(new Pen(Color.Red, 1), rx2, ry2, rxx2, Convert.ToInt32(arrayCoordinates[3]) / 250);
                        rx2 = rxx2;
                        ry2 = Convert.ToInt32(arrayCoordinates[3]) / 250;
                        rxx2 = rxx2 + 2;

                        arrayCoordinates = sr.ReadLine().Split(' ');
                    }
                    if (i % 250 == 0)
                    {
                        sensor1Signal.Clear(Color.White);
                        sensor2Signal.Clear(Color.White);
                        sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 60), new Point(800, 60));
                        sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 120), new Point(800, 120));
                        sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 180), new Point(800, 180));
                        sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 60), new Point(800, 60));
                        sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 120), new Point(800, 120));
                        sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 180), new Point(800, 180));
                        greenXSensor1 = 0;
                        greenShiftSensor1 = 2;

                        blueXSensor1 = 0;
                        blueShiftSensor1 = 2;

                        redXSensor1 = 0;
                        redx11 = 2;

                        gx2 = 0;
                        gxx2 = 2;

                        bx2 = 0;
                        bxx2 = 2;

                        rx2 = 0;
                        rxx2 = 2;
                    }
                    i++;
                    Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                listBoxLog.Items.Add(DateTime.Now.ToLongTimeString() + " " + ex.Message);
                listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
            }
            finally
            {
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();

                listBoxLog.Items.Add(DateTime.Now.ToLongTimeString() + "Cчитывание данных с файла " + info.FileName + " завершилось");
                listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            Connect();
        }
        private void Connect()
        {
            if (connect == true)
            {
                buttonConnect.Text = "        Disconnect";
                buttonConnect.ForeColor = Color.White;
                buttonConnect.BackColor = Color.Red;
                buttonConnect.Image = Properties.Resources.button_off;
                connectToolStripMenuItem.Text = "Disconnect";
                connectToolStripMenuItem.Image = Properties.Resources.button_red;
                connect = false;
                try
                {
                    DataMEMS.MEMSport = new SerialPort("COM4", 115200, Parity.None, 8, StopBits.Two);
                    DataMEMS.OpenPort(true);
                    listBoxLog.Items.Add(DateTime.Now.ToLongTimeString() + " Подключение к COM3 прошло успешно");
                    listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
                }
                catch (Exception ex)
                {
                    listBoxLog.Items.Add(DateTime.Now.ToLongTimeString() + " " + ex.Message);
                    listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
                }
            }
            else
            {
                buttonConnect.Text = "     Connect";
                buttonConnect.ForeColor = Color.White;
                buttonConnect.BackColor = Color.FromArgb(44, 48, 51);
                buttonConnect.Image = Properties.Resources.button_off;
                connectToolStripMenuItem.Text = "Connect";
                connectToolStripMenuItem.Image = Properties.Resources.button_on;
                connect = true;
                DataMEMS.OpenPort(false);
                var sensor1Signal = pictureBoxSensor1Signal.CreateGraphics();
                var sensor2Signal = pictureBoxSensor2Signal.CreateGraphics();
                sensor1Signal.Clear(Color.White);
                sensor2Signal.Clear(Color.White);
                listBoxLog.Items.Add(DateTime.Now.ToLongTimeString() + " Отключение от COM3 прошло успешно");
                listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
            }
        }
        private void buttonRun_Click(object sender, EventArgs e)
        {
            //List<Complex> listComplexX = new List<Complex>();
            //List<Complex> listComplexY = new List<Complex>();
            //List<Complex> listComplexZ = new List<Complex>();

            List<double> listX = new List<double>();
            List<double> listY = new List<double>();
            List<double> listZ = new List<double>();

            var sensor1Signal = pictureBoxSensor1Signal.CreateGraphics();
            var sensor2Signal = pictureBoxSensor2Signal.CreateGraphics();

            int greenXSensor1 = 0;
            int greenYSensor1 = 0;
            int greenShiftSensor1 = 2;

            int blueXSensor1 = 0;
            int blueYSensor1 = 0;
            int blueShiftSensor1 = 2;

            int redXSensor1 = 0;
            int redYSensor1 = 0;
            int redShiftSensor1 = 2;

            int greenXSensor2 = 0;
            int greenYSensor2 = 0;
            int greenShiftSensor2 = 2;

            int blueXSensor2 = 0;
            int blueYSensor2 = 0;
            int blueShiftSensor2 = 2;

            int redXSensor2 = 0;
            int redYSensor2 = 0;
            int redShiftSensor2 = 2;

            int sensorId1 = 0;
            int i = 0;
            int[] arrayCoordinates;
            sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 60), new Point(800, 60));
            sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 120), new Point(800, 120));
            sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 180), new Point(800, 180));
            sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 60), new Point(800, 60));
            sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 120), new Point(800, 120));
            sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 180), new Point(800, 180));
            while (i < 1000)
            {
                arrayCoordinates = DataMEMS.ReadDataFromMEMS();
                if (i == 0)
                    sensorId1 = arrayCoordinates[0];
                if (arrayCoordinates[0] == sensorId1)
                {
                    sensor1Signal.DrawLine(new Pen(Color.Green, 1), greenXSensor1, greenYSensor1, greenShiftSensor1, arrayCoordinates[1] / 250);
                    greenXSensor1 = greenShiftSensor1;
                    greenYSensor1 = arrayCoordinates[1] / 250;
                    greenShiftSensor1 = greenShiftSensor1 + 2;
                    //listComplexX.Add(new Complex(arrayCoordinates[1], 0));
                    listX.Add((double)arrayCoordinates[1]);

                    sensor1Signal.DrawLine(new Pen(Color.Blue, 1), blueXSensor1, blueYSensor1, blueShiftSensor1, arrayCoordinates[2] / 250);
                    blueXSensor1 = blueShiftSensor1;
                    blueYSensor1 = arrayCoordinates[2] / 250;
                    blueShiftSensor1 = blueShiftSensor1 + 2;
                    //listComplexY.Add(new Complex(arrayCoordinates[2], 0));
                    listY.Add((double)arrayCoordinates[2]);

                    sensor1Signal.DrawLine(new Pen(Color.Red, 1), redXSensor1, redYSensor1, redShiftSensor1, arrayCoordinates[3] / 250);
                    redXSensor1 = redShiftSensor1;
                    redYSensor1 = arrayCoordinates[3] / 250;
                    redShiftSensor1 = redShiftSensor1 + 2;
                    //listComplexZ.Add(new Complex(arrayCoordinates[3], 0));
                    listZ.Add((double)arrayCoordinates[3]);
                }
                else
                {
                    sensor2Signal.DrawLine(new Pen(Color.Green, 1), greenXSensor2, greenYSensor2, greenShiftSensor2, arrayCoordinates[1] / 250);
                    greenXSensor2 = greenShiftSensor2;
                    greenYSensor2 = arrayCoordinates[1] / 250;
                    greenShiftSensor2 = greenShiftSensor2 + 2;

                    sensor2Signal.DrawLine(new Pen(Color.Blue, 1), blueXSensor2, blueYSensor2, blueShiftSensor2, arrayCoordinates[2] / 250);
                    blueXSensor2 = blueShiftSensor2;
                    blueYSensor2 = arrayCoordinates[2] / 250;
                    blueShiftSensor2 = blueShiftSensor2 + 2;

                    sensor2Signal.DrawLine(new Pen(Color.Red, 1), redXSensor2, redYSensor2, redShiftSensor2, arrayCoordinates[3] / 250);
                    redXSensor2 = redShiftSensor2;
                    redYSensor2 = arrayCoordinates[3] / 250;
                    redShiftSensor2 = redShiftSensor2 + 2;
                }
                //listBoxLog.Items.Add(DateTime.Now.ToLongTimeString() + " " + arrayCoordinates[0] + " " + arrayCoordinates[1] + " " + arrayCoordinates[2] + " " + arrayCoordinates[3]);
                //listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
                if (i % 250 == 0)
                {
                    sensor1Signal.Clear(Color.White);
                    sensor2Signal.Clear(Color.White);
                    sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 60), new Point(800, 60));
                    sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 120), new Point(800, 120));
                    sensor1Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 180), new Point(800, 180));
                    sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 60), new Point(800, 60));
                    sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 120), new Point(800, 120));
                    sensor2Signal.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 180), new Point(800, 180));

                    greenXSensor1 = 0;
                    greenShiftSensor1 = 2;

                    blueXSensor1 = 0;
                    blueShiftSensor1 = 2;

                    redXSensor1 = 0;
                    redShiftSensor1 = 2;

                    greenXSensor2 = 0;
                    greenShiftSensor2 = 2;

                    blueXSensor2 = 0;
                    blueShiftSensor2 = 2;

                    redXSensor2 = 0;
                    redShiftSensor2 = 2;
                }
                i++;
            }

            //Complex[] samplesX = listComplexX.ToArray();
            //Complex[] samplesY = listComplexY.ToArray();
            //Complex[] samplesZ = listComplexZ.ToArray();

            //прореживаю листы до 2^n
            int n = 14;
            while (listX.Count / Math.Pow(2,n) < 1 )
            {
                n--;
            }
            int countList = listX.Count;
            int TwoPower = (int)Math.Pow(2, n);
            Random rnd = new Random();
            for (int k = 0; k < (countList - TwoPower); k++)
            {
                listX.RemoveAt(rnd.Next(k));
                listY.RemoveAt(rnd.Next(k));
                listZ.RemoveAt(rnd.Next(k));
            }

            double[] FourierResultX = Fourier.FFT(listX.ToArray());
            double[] FourierResultY = Fourier.FFT(listY.ToArray());
            double[] FourierResultZ = Fourier.FFT(listZ.ToArray());


            //chart1.ChartAreas[0].AxisX.Minimum = -1000;
            //chart1.ChartAreas[0].AxisX.Maximum = 10;
            //chart1.ChartAreas[0].AxisX.Interval = 1;
            //chart1.ChartAreas[0].AxisY.Minimum = -1000;
            //chart1.ChartAreas[0].AxisY.Maximum = 100;

            //Fourier.Forward(samplesX, FourierOptions.Matlab);
            //double[] FourierResultX = Fourier.FFT(XSamples);
            //double[] FourierResultY = Fourier.FFT(YSamples);
            //double[] FourierResultZ = Fourier.FFT(ZSamples);            

            for (int j = 0; j < FourierResultX.Length; j++)
            {
                chart1.Series["Series1"].Points.AddXY
                        (j, FourierResultX[j]);
            }

            for (int j = 0; j < FourierResultY.Length; j++)
            {
                chart2.Series["Series1"].Points.AddXY
                        (j, FourierResultY[j]);
            }

            for (int j = 0; j < FourierResultZ.Length; j++)
            {
                chart3.Series["Series1"].Points.AddXY
                        (j, FourierResultZ[j]);
            }     

        //    Fourier.Forward(samplesY, FourierOptions.Matlab);
        //    for (int j = 0; j < samplesY.Length / 10; j++)
        //    {
        //        chart2.Series["Series1"].Points.AddXY
        //                (j, samplesY[j * 10].Magnitude);

        //    }
        //    Fourier.Forward(samplesZ, FourierOptions.Matlab);
        //    for (int j = 0; j < samplesZ.Length / 10; j++)
        //    {

        //        chart3.Series["Series1"].Points.AddXY
        //                (j * 10, samplesZ[j * 10].Magnitude);

        //    }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAboutProgram formAboutProgram = new FormAboutProgram();
            formAboutProgram.Show();
        }

        private void pictureBoxInfoSensor1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Green, 200, 2, 10, 10);
            e.Graphics.DrawString("X", new Font("Times New Roman", 10), Brushes.Black, 220, 2);
            e.Graphics.FillRectangle(Brushes.Blue, 250, 2, 10, 10);
            e.Graphics.DrawString("Y", new Font("Times New Roman", 10), Brushes.Black, 270, 2);
            e.Graphics.FillRectangle(Brushes.Red, 300, 2, 10, 10);
            e.Graphics.DrawString("Z", new Font("Times New Roman", 10), Brushes.Black, 320, 2);
        }

        private void pictureBoxInfoSensor2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Green, 200, 2, 10, 10);
            e.Graphics.DrawString("X", new Font("Times New Roman", 10), Brushes.Black, 220, 2);
            e.Graphics.FillRectangle(Brushes.Blue, 250, 2, 10, 10);
            e.Graphics.DrawString("Y", new Font("Times New Roman", 10), Brushes.Black, 270, 2);
            e.Graphics.FillRectangle(Brushes.Red, 300, 2, 10, 10);
            e.Graphics.DrawString("Z", new Font("Times New Roman", 10), Brushes.Black, 320, 2);
        }

        private void pictureBoxSensor1Signal_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 60), new Point(800, 60));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 120), new Point(800, 120));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 180), new Point(800, 180));
        }

        private void pictureBoxSensor2Signal_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 60), new Point(800, 60));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 120), new Point(800, 120));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(233, 233, 233)), new Point(0, 180), new Point(800, 180));
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }
    }
}
