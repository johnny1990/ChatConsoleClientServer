using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Program
    {
        static IPAddress ip;
        static Socket socket;
        static Thread thread;
        static string chatUserName;

        static string GetLocalIp()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry entry = Dns.GetHostEntry(hostName);
            IPAddress[] address = entry.AddressList;
            return address[address.Length - 1].ToString();
        }

        static void Receive()
        {
            while (true)
            {
                Thread.Sleep(50);
                byte[] buffer = new byte[300];
                int receive = socket.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, receive);
                Console.WriteLine(Encoding.Default.GetString(buffer));
            }
        }

        static void Main(string[] args)
        {
            thread = new Thread(Receive);
            Console.WriteLine("------------------Chat Client------------------------");
            Console.WriteLine("Please enter your name and press enter");


            chatUserName = Console.ReadLine();
            ip = IPAddress.Parse(GetLocalIp());
            int port = 1234;


            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(ip, port));
            thread.Start();

            byte[] data = Encoding.Default.GetBytes("<" + chatUserName + "> Connected to the chat");
            socket.Send(data, 0, data.Length, 0);

            while (socket.Connected)
            {  
                byte[] SendDataToServer = Encoding.Default.GetBytes("<" + chatUserName + ">" + Console.ReadLine());
                socket.Send(SendDataToServer, 0, SendDataToServer.Length, 0);
            }
        }
    }
}
