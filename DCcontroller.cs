using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace controller
{


    public  class DC
    {

        public  SerialPort serialPort1 = new SerialPort(); 
        
        public  void com_controller(int n) // Инициализация COM-порта для корректной работы с  
        {                                 // блоком питания Agilent E3648A. RTS, DTR = Вкл. !!!
            if (!serialPort1.IsOpen)
            {
                serialPort1.PortName = "COM" + n.ToString();
                serialPort1.Open();
                serialPort1.RtsEnable = true;
                serialPort1.DtrEnable = true;
                serialPort1.BaudRate = 9600;
            }
        }

        public  void com_off(int n) // закрытие COM-порта
        {
            serialPort1.Close();
        }

        public  void dc_on() // команда для включения блока питания
        {
            byte[] bytes = Encoding.ASCII.GetBytes("OUTP ON\n");
            serialPort1.Write(bytes, 0, bytes.Length);
        }

        public  void dc_off() // команда для включения блока питания
        {
            byte[] bytes = Encoding.ASCII.GetBytes("OUTP OFF\n");
            serialPort1.Write(bytes, 0, bytes.Length);
        }


        public  void set_V(double V) // команда для задания уровня напряжения питания
        {
            double V_real = V / 2 + 0.0;
            string V_string = V_real.ToString("F1");
            String[] substrings = V_string.Split(',');
            V_string = substrings[0] + '.' + substrings[1];
            string s = "VOLT " + V_string + " \n";
            byte[] bytes = Encoding.ASCII.GetBytes("INST:SEL OUT1" + "\n");
            serialPort1.Write(bytes, 0, bytes.Length);
            Thread.Sleep(500); // задержка в 500 мс
            bytes = Encoding.ASCII.GetBytes(s);
            serialPort1.Write(bytes, 0, bytes.Length);
            Thread.Sleep(500);
            bytes = Encoding.ASCII.GetBytes("INST:SEL OUT2" + "\n");
            serialPort1.Write(bytes, 0, bytes.Length);
            Thread.Sleep(500);
            bytes = Encoding.ASCII.GetBytes(s);
            serialPort1.Write(bytes, 0, bytes.Length);
        }


    public  string meas_V() // команда для измерение  напряжения питания
        {
            byte[] bytes = Encoding.ASCII.GetBytes("MEAS:VOLT?" + Environment.NewLine);
            serialPort1.Write(bytes, 0, bytes.Length);
            string data = serialPort1.ReadExisting().ToString();
            return data;
        }

        public  string meas_I() // команда для измерения  тока потребления
        {
            byte[] bytes = Encoding.ASCII.GetBytes("MEAS:CURR?" + Environment.NewLine);
            serialPort1.Write(bytes, 0, bytes.Length);
            string data = serialPort1.ReadExisting().ToString();
            return data;
        }

        private  void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Console.WriteLine("Data Received:");
            Console.Write(indata);
        }
    }


}