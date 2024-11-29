/*
Library for software control of the Agilent E3648A Power Supply via RS-232 (COM port).
This library provides methods to initialize the connection, configure settings, 
control outputs, set voltage levels, and measure output parameters (voltage and current) 
using SCPI commands.
*/

using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace controller
{
    public class DC
    {
        public SerialPort serialPort1 = new SerialPort(); 

        // Initializes the COM port for correct operation with the Agilent E3648A power supply. RTS and DTR = ON!
        public void com_controller(int n)
        {
            try
            {
                if (!serialPort1.IsOpen)
                {
                    serialPort1.PortName = "COM" + n.ToString();
                    serialPort1.BaudRate = 9600;
                    serialPort1.RtsEnable = true;
                    serialPort1.DtrEnable = true;
                    serialPort1.Open();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing COM port: {ex.Message}");
            }
        }

        // Closes the COM port
        public void com_off()
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing COM port: {ex.Message}");
            }
        }

        // Command to turn on the power supply
        public void dc_on()
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes("OUTP ON\n");
                serialPort1.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending ON command: {ex.Message}");
            }
        }

        // Command to turn off the power supply
        public void dc_off()
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes("OUTP OFF\n");
                serialPort1.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OFF command: {ex.Message}");
            }
        }

        // Command to set the power supply voltage level
        public void set_V(double V)
        {
            try
            {
                double V_real = V / 2;
                string V_string = V_real.ToString("F1").Replace(",", ".");
                string command = "VOLT " + V_string + " \n";

                // Set voltage for output channel 1
                serialPort1.Write(Encoding.ASCII.GetBytes("INST:SEL OUT1\n"), 0, command.Length);
                Thread.Sleep(500); // Delay for device to process
                serialPort1.Write(Encoding.ASCII.GetBytes(command), 0, command.Length);

                // Set voltage for output channel 2
                serialPort1.Write(Encoding.ASCII.GetBytes("INST:SEL OUT2\n"), 0, command.Length);
                Thread.Sleep(500);
                serialPort1.Write(Encoding.ASCII.GetBytes(command), 0, command.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting voltage: {ex.Message}");
            }
        }

        // Command to measure the output voltage
        public string meas_V()
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes("MEAS:VOLT?\n");
                serialPort1.Write(bytes, 0, bytes.Length);
                return serialPort1.ReadExisting();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error measuring voltage: {ex.Message}");
                return string.Empty;
            }
        }

        // Command to measure the output current
        public string meas_I()
        {
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes("MEAS:CURR?\n");
                serialPort1.Write(bytes, 0, bytes.Length);
                return serialPort1.ReadExisting();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error measuring current: {ex.Message}");
                return string.Empty;
            }
        }

        // Handler for incoming serial data
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                Console.WriteLine("Data Received:");
                Console.WriteLine(indata);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling received data: {ex.Message}");
            }
        }
    }
}
