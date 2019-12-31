/////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// This project demonstrates how to write a simple vJoy feeder in C#
//
// You can compile it with either #define ROBUST OR #define EFFICIENT
// The fuctionality is similar - 
// The ROBUST section demonstrate the usage of functions that are easy and safe to use but are less efficient
// The EFFICIENT ection demonstrate the usage of functions that are more efficient
//
// Functionality:
//	The program starts with creating one joystick object. 
//	Then it petches the device id from the command-line and makes sure that it is within range
//	After testing that the driver is enabled it gets information about the driver
//	Gets information about the specified virtual device
//	This feeder uses only a few axes. It checks their existence and 
//	checks the number of buttons and POV Hat switches.
//	Then the feeder acquires the virtual device
//	Here starts and endless loop that feedes data into the virtual device
//
/////////////////////////////////////////////////////////////////////////////////////////////////////////
#define ROBUST
//#define EFFICIENT

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
// Don't forget to add this
using vJoyInterfaceWrap;
using System.Threading;

namespace FeederDemoCS
{
	class Program
	{
		static Class_Joystick Joystick;
		static Class_Network Network;
        public static Queue<ControlData> Queue_ControlData;
		static void Main(string[] args)
		{
            Console.WriteLine("UDPJoystick APP started");
            Console.WriteLine("=======Powered by HZL=========小超超么么哒->_->\n");
			Joystick = new Class_Joystick();
			Network = new Class_Network();
			Network.BroadcastSelfAddress();
            Joystick.Init(args);
            Queue_ControlData = new Queue<ControlData>();
            Joystick.StartWorking();
            Network.StartCommunication();
		} // Main
	} // class Program
} // namespace FeederDemoCS
