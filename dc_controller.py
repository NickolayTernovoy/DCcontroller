"""
Library for software control of the Agilent E3648A power supply via RS-232 (COM port).
This library provides functions to initialize the connection, control the power supply outputs, 
set voltage levels, and measure output parameters (voltage and current) using SCPI commands.
"""

import serial
import time


class DC:
    def __init__(self):
        self.serial_port = serial.Serial()  # Initializing the serial port

    def com_controller(self, n):  
        # Initializes the COM port for correct operation with the Agilent E3648A power supply.
        # RTS, DTR = ON
        if not self.serial_port.is_open:
            self.serial_port.port = f"COM{n}"
            self.serial_port.baudrate = 9600
            self.serial_port.rtscts = True
            self.serial_port.dtr = True
            self.serial_port.open()

    def com_off(self):  
        # Closes the COM port
        self.serial_port.close()

    def dc_on(self):  
        # Command to turn on the power supply
        command = "OUTP ON\n"
        self.serial_port.write(command.encode('ascii'))

    def dc_off(self):  
        # Command to turn off the power supply
        command = "OUTP OFF\n"
        self.serial_port.write(command.encode('ascii'))

    def set_voltage(self, V):  
        # Command to set the power supply voltage level
        V_real = V / 2 + 0.0
        V_string = f"{V_real:.1f}"
        V_string = V_string.replace(",", ".")  # Ensures proper format for the device
        command = f"VOLT {V_string} \n"

        # Setting voltage for output channel 1
        self.serial_port.write("INST:SEL OUT1\n".encode('ascii'))
        time.sleep(0.5)  # Delay of 500 ms
        self.serial_port.write(command.encode('ascii'))
        time.sleep(0.5)

        # Setting voltage for output channel 2
        self.serial_port.write("INST:SEL OUT2\n".encode('ascii'))
        time.sleep(0.5)
        self.serial_port.write(command.encode('ascii'))

    def meas_voltage(self):  
        # Command to measure the output voltage
        self.serial_port.write("MEAS:VOLT?\n".encode('ascii'))
        data = self.serial_port.read_all().decode('ascii')
        return data.strip()

    def meas_current(self):  
        # Command to measure the output current
        self.serial_port.write("MEAS:CURR?\n".encode('ascii'))
        data = self.serial_port.read_all().decode('ascii')
        return data.strip()

    def data_received_handler(self):  
        # Handler for incoming serial data
        data = self.serial_port.read_all().decode('ascii')
        print("Data Received:")
        print(data)
