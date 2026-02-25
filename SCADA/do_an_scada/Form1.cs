using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using SymbolFactoryDotNet; 
using S7.Net;
using System.Diagnostics;
using System.IO;

namespace do_an_scada
{
    public partial class Form1 : Form
    {
        Plc plc_s7;
        CpuType Cpu_Tpye = CpuType.S71200;
        string IP = "192.168.5.3";
        short Rack = 0;
        short Slot = 1;
        CancellationTokenSource cancellationToken;

        private Process pythonProcess;
        private bool isRunning = false; // Biến cờ để kiểm tra trạng thái của tiến trình

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            plc_s7 = new Plc(Cpu_Tpye, IP, Rack, Slot); 
            plc_s7.Open();
        }
        
        public void Read_Data_PLC(CancellationToken sourcetoken) // Hàm read dữ liệu lên
        {
            Task.Factory.StartNew(new System.Action(() =>
            {
                while (true)
                {
                    if (sourcetoken.IsCancellationRequested)
                    {
                        return;
                    }
                    Thread.Sleep(300);
                    Read_Data_Form_PLC();
                    Thread.Sleep(200);
                }
            }), sourcetoken);
        }
        
        void Read_Data_Form_PLC()
        {
            if (plc_s7.IsConnected == true)
            {
                byte[] Input_Data = plc_s7.ReadBytes(DataType.Input, 0, 0, 10);
                if (Input_Data[0].SelectBit(2) == true) //Read I0.2
                {
                    CB.DiscreteValue1 = false;
                    CB.DiscreteValue2 = true;
                }
                else
                {
                    CB.DiscreteValue1 = true;
                    CB.DiscreteValue2 = false;
                }

                if (Input_Data[0].SelectBit(3) == true) //Read I0.3
                {
                    CB1.DiscreteValue1 = false;
                    CB1.DiscreteValue2 = true;
                }
                else
                {
                    CB1.DiscreteValue1 = true;
                    CB1.DiscreteValue2 = false;
                }

                byte[] Read_Data = plc_s7.ReadBytes(DataType.Output, 0, 0, 10);
                
                if (Read_Data[0].SelectBit(0) == true) //Read Q0.0
                {
                    DC.DiscreteValue1 = false;
                    DC.DiscreteValue2 = true;
                    BT.DiscreteValue1 = false;
                    BT.DiscreteValue2 = true;
                }
                else
                {
                    DC.DiscreteValue1 = true;
                    DC.DiscreteValue2 = false;
                    BT.DiscreteValue1 = true;
                    BT.DiscreteValue2 = false;
                }
                
                if (Read_Data[0].SelectBit(1) == true) //Read Q0.1
                {
                    DClen.DiscreteValue1 = false;
                    DClen.DiscreteValue2 = true;
                    dongcoL.DiscreteValue1 = false;
                    dongcoL.DiscreteValue2 = true;
                    muitenlen.DiscreteValue1 = false;
                    muitenlen.DiscreteValue2 = true;
                }
                else
                {
                    DClen.DiscreteValue1 = true;
                    DClen.DiscreteValue2 = false;
                    dongcoL.DiscreteValue1 = true;
                    dongcoL.DiscreteValue2 = false;
                    muitenlen.DiscreteValue1 = true;
                    muitenlen.DiscreteValue2 = false;
                }
                
                if (Read_Data[0].SelectBit(1) == false) //Read Q0.1
                {
                    DCxuong.DiscreteValue1 = false;
                    DCxuong.DiscreteValue2 = true;
                    dongcoX.DiscreteValue1 = false;
                    dongcoX.DiscreteValue2 = true;
                    muitenxuong.DiscreteValue1 = false;
                    muitenxuong.DiscreteValue2 = true;
                }
                else
                {
                    DCxuong.DiscreteValue1 = true;
                    DCxuong.DiscreteValue2 = false;
                    dongcoX.DiscreteValue1 = true;
                    dongcoX.DiscreteValue2 = false;
                    muitenxuong.DiscreteValue1 = true;
                    muitenxuong.DiscreteValue2 = false;
                }

                if (Read_Data[0].SelectBit(3) == true) //Read Q0.3
                {
                    DChut.DiscreteValue1 = false;
                    DChut.DiscreteValue2 = true;
                }
                else
                {
                    DChut.DiscreteValue1 = true;
                    DChut.DiscreteValue2 = false;
                }

                SLnapB.Text = plc_s7.Read("MW6").ToString();
                SLnapO.Text = plc_s7.Read("MW8").ToString();
                SLnapP.Text = plc_s7.Read("MW3").ToString();
                textBox1.Text = plc_s7.Read("DB2.DBW0").ToString();
                textBox2.Text = plc_s7.Read("DB2.DBW2").ToString();
                textBox3.Text = plc_s7.Read("DB15.DBW0").ToString();

                byte[] Sensor_Data = plc_s7.ReadBytes(DataType.Memory, 0, 0, 10);
                if (Sensor_Data[5].SelectBit(0) == true) //Read M5.0
                {
                    denxuong.DiscreteValue1 = false;
                    denxuong.DiscreteValue2 = true;
                }
                else
                {
                    denxuong.DiscreteValue1 = true;
                    denxuong.DiscreteValue2 = false;
                }
               
                if (Sensor_Data[5].SelectBit(1) == true) //Read M5.1
                {
                    denlen.DiscreteValue1 = false;
                    denlen.DiscreteValue2 = true;
                }
                else
                {
                    denlen.DiscreteValue1 = true;
                    denlen.DiscreteValue2 = false;
                }
                
                if (Sensor_Data[5].SelectBit(2) == true) //Read M5.2
                {
                    den0.DiscreteValue1 = false;
                    den0.DiscreteValue2 = true;
                }
                else
                {
                    den0.DiscreteValue1 = true;
                    den0.DiscreteValue2 = false;
                }
                
                if (Sensor_Data[2].SelectBit(5) == true) //Read M2.5
                {
                    den90.DiscreteValue1 = false;
                    den90.DiscreteValue2 = true;
                }
                else
                {
                    den90.DiscreteValue1 = true;
                    den90.DiscreteValue2 = false;
                }

                if (Sensor_Data[5].SelectBit(3) == true) //Read M5.3
                {
                    den135.DiscreteValue1 = false;
                    den135.DiscreteValue2 = true;
                }
                else
                {
                    den135.DiscreteValue1 = true;
                    den135.DiscreteValue2 = false;
                }

                if (Sensor_Data[5].SelectBit(4) == true) //Read M5.4
                {
                    den180.DiscreteValue1 = false;
                    den180.DiscreteValue2 = true;
                }
                else
                {
                    den180.DiscreteValue1 = true;
                    den180.DiscreteValue2 = false;
                }
                

                if (Sensor_Data[5].SelectBit(5) == true) //Read M5.5
                {
                    denhut.DiscreteValue1 = false;
                    denhut.DiscreteValue2 = true;
                }
                else
                {
                    denhut.DiscreteValue1 = true;
                    denhut.DiscreteValue2 = false;
                }
                
                if (Sensor_Data[5].SelectBit(6) == true) //Read M5.6
                {
                    dennha.DiscreteValue1 = false;
                    dennha.DiscreteValue2 = true;
                }
                else
                {
                    dennha.DiscreteValue1 = true;
                    dennha.DiscreteValue2 = false;
                }
                
                if (Sensor_Data[2].SelectBit(1) == true) //Read M2.1
                {
                    den.DiscreteValue1 = false;
                    den.DiscreteValue2 = true;
                }
                else
                {
                    den.DiscreteValue1 = true;
                    den.DiscreteValue2 = false;
                }
            }
        }
        
        private void btConnect_Click(object sender, EventArgs e)
        {
            if (plc_s7.IsConnected == true)
            {
                cancellationToken = new CancellationTokenSource();
                var sourcetoken = cancellationToken.Token;
                Read_Data_PLC(sourcetoken);

                MessageBox.Show("Connect OK", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Connect Error", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                plc_s7.Close();
            }
        }

        private void btDisconnect_Click(object sender, EventArgs e)
        {
            plc_s7.Close();
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            plc_s7.WriteBit(DataType.Memory, 0, 3, 2, true); //Write M3.2
            Thread.Sleep(100);
            plc_s7.WriteBit(DataType.Memory, 0, 3, 2, false);
            Thread.Sleep(200);
        }
        
        private void btStop_Click(object sender, EventArgs e)
        {
            plc_s7.WriteBit(DataType.Memory, 0, 2, 3, true); //Write M2.3
            Thread.Sleep(100);
            plc_s7.WriteBit(DataType.Memory, 0, 2, 3, false);
            Thread.Sleep(200);
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            plc_s7.WriteBit(DataType.Memory, 0, 5, 7, true); //Write M5.7
            Thread.Sleep(100);
            plc_s7.WriteBit(DataType.Memory, 0, 5, 7, false);
            Thread.Sleep(200);
        }

        private void oncam_Click(object sender, EventArgs e)
        {
            string pythonExe = @"C:\Users\huong\AppData\Local\Microsoft\WindowsApps\python3.11.exe";
            string pythonScript = @"D:\object-detection\venv\YOLOnapchai.py";

            var startInfo = new ProcessStartInfo
            {
                FileName = pythonExe,
                Arguments = $"\"{pythonScript}\"",
                UseShellExecute = false, // Do not use shell execute
                CreateNoWindow = false,  // Open window for OpenCV to display
                RedirectStandardOutput = true // Redirect Python script's output
            };

            pythonProcess = new Process
            {
                StartInfo = startInfo
            };
            pythonProcess.Start();
            isRunning = true; // Đặt trạng thái đang chạy
            Task.Run(() =>
            {
                try
                {
                    while (isRunning && !pythonProcess.HasExited)
                    {
                        string line = pythonProcess.StandardOutput.ReadLine();
                        if (line != null)
                        {
                            if (line.StartsWith("FRAME:"))
                            {
                                string base64Data = line.Substring("FRAME:".Length);

                                try
                                {
                                    byte[] imageBytes = Convert.FromBase64String(base64Data);
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        Image image = Image.FromStream(ms);
                                        ptbImg.Invoke((MethodInvoker)delegate
                                        {
                                            ptbImg.Image = image;
                                        });
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error decoding image: " + ex.Message);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in background task: " + ex.Message);
                }
            });
        }

        private void offcam_Click(object sender, EventArgs e)
        {
            if (pythonProcess != null && !pythonProcess.HasExited)
            {
                isRunning = false; // Dừng vòng lặp trong Task
                pythonProcess.Kill(); // Dừng tiến trình Python
                pythonProcess = null; // Đặt null sau khi dừng
            }
        }
    }
}
