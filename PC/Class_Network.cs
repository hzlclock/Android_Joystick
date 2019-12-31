using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using FeederDemoCS;
namespace FeederDemoCS
{
    class Class_Network
    {
        Socket Socket_ListenRequest, Socket_ListenCommand, Socket_Broadcast;
        IPEndPoint Local_EndPoint, Local_ListenEndPoint;
        IPAddress Local_Address;
        public void BroadcastSelfAddress()
        {
            Socket_Broadcast = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Socket_ListenRequest = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress[] Local_Addresses = Dns.GetHostAddresses(Dns.GetHostName());
            int count = 1;

            //Get my IP and the user select one he wants to bind
            Console.WriteLine("你的电脑有如下的IP地址\n请选择一个用来接收数据的IP地址，如果你不知道选择哪个，建议选择192.168.xxx.xxx\n\n请[允许]程序通过您的[防火墙]！！！！！\n");
            foreach (var ip in Local_Addresses)
            {
                Console.Write(count + ".  ");
                count++;
                Console.WriteLine(ip.ToString());
            }
            IPEndPoint Local_BroadCastEndPoint;
            while (true)
            {
                try
                {
                    int selection = int.Parse(Console.ReadLine());
                    Local_Address = Local_Addresses[selection - 1];
                    Local_EndPoint = new IPEndPoint(Local_Address, 7260);
                    Local_BroadCastEndPoint = new IPEndPoint(Local_Address, 9607);
                    break;
                }
                catch { }
                finally { };
            }

            //bind the Address and start listen for any infomation
            Socket_ListenRequest.Bind(Local_EndPoint);
            Socket_Broadcast.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            Console.WriteLine("Listening for devices' probes");
            while (true)
            {
                byte[] buffer = new byte[102400];
                string receivedString;
                Socket_ListenRequest.Receive(buffer);
                receivedString = Encoding.ASCII.GetString(buffer);
                if (receivedString.Contains("<Request>"))//send my address
                {
                    Console.WriteLine("收到连接请求，如果设备上出现是否连接的消息窗口，请确认");
                    IPEndPoint ipeBroadcast = new IPEndPoint(IPAddress.Broadcast, 9607);
                    buffer = Encoding.ASCII.GetBytes("<ip=" + Local_EndPoint.Address.ToString() + ">");
                    Socket_Broadcast.SendTo(buffer, ipeBroadcast);
                    receivedString = "";
                }
                if (receivedString.Contains("<Connect>"))//device try to connect
                {
						 Console.WriteLine("设备已连接");
                    Socket_ListenRequest.Close();
                    return;
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(50));
            }
        }

        public void StartCommunication()
        {
            Socket_ListenCommand = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //Local_ListenEndPoint = new IPEndPoint(Local_Address, 4260);
            IPEndPoint local_DataReceiveEndPoint = new IPEndPoint(Local_Address, 9696);
            Socket_ListenCommand.Bind(local_DataReceiveEndPoint);
            int length;
            string s;
            byte[] buffer = new byte[1024000];
            while (true)
            {
                length = Socket_ListenCommand.Receive(buffer);
                s = Encoding.ASCII.GetString(buffer, 0, length);
                Console.WriteLine("DataReceived:\n" + s);
                try
                {
                    ControlData data = new ControlData(s);
                    FeederDemoCS.Program.Queue_ControlData.Enqueue(data);
                }
                catch { Console.WriteLine("Invalid Data"); }
                finally { };
            }
        }
    }
}
