# Agilent E3648A Control Library

This project provides libraries for controlling the **Agilent E3648A Power Supply** via RS-232 (COM port). The libraries are available in both **C#** and **Python**, making it easy to integrate into your application regardless of your preferred programming language.

## Features
- Initialize and configure the COM port for communication.
- Turn the power supply outputs on and off.
- Set voltage levels for one or both channels.
- Measure output voltage and current.
- Compatible with SCPI commands used by Agilent E3648A.

---

## Available Versions
### C#
- Full-featured implementation using `System.IO.Ports` for serial communication.
- Includes methods for all major functions (turn on/off, set voltage, measure parameters).
- Object-oriented structure for easy integration into larger projects.

### Python
- Python version uses the `pyserial` library for serial communication.
- Lightweight and simple-to-use implementation with the same functionality as the C# version.
- Compatible with Python 3.x.

---

## Usage

### C# Version
To use the C# library:
1. Include the provided C# source file in your project.
2. Create an instance of the `DC` class.
3. Use methods like `com_controller`, `dc_on`, `set_V`, etc.

```csharp
var dc = new DC();
dc.com_controller(1); // Initialize COM1
dc.dc_on();           // Turn on power supply
dc.set_V(5.0);        // Set voltage to 5V
string voltage = dc.meas_V(); // Measure output voltage
Console.WriteLine($"Voltage: {voltage}V");
dc.com_off();         // Close the COM port
