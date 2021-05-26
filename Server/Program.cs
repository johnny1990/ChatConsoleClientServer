using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static IPAddress ip;
        static Thread thread;
        static Socket socket;
        static Socket acceptSocket;
        static string serverChatUserName;

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
                int receive = acceptSocket.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, receive);
                Console.WriteLine(Encoding.Default.GetString(buffer));
            }
        }

        static void Main(string[] args)
        {
            thread = new Thread(Receive);
            Console.WriteLine("--------------Chat Server----------------------------");
            Console.WriteLine("Please enter your name and press enter");
            serverChatUserName = Console.ReadLine();

            Console.WriteLine("<" + serverChatUserName + "> Connected to the chat");
            Console.WriteLine("Please enter the name in client console app to connect and have a chat!");

            int port = 1234;

            ip = IPAddress.Parse(GetLocalIp());
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(ip, port));
            socket.Listen(0);
            acceptSocket = socket.Accept();
            thread.Start();

            while (true)
            {   
                byte[] SendDataToClient = Encoding.Default.GetBytes("<" + serverChatUserName + ">" + Console.ReadLine());
                acceptSocket.Send(SendDataToClient, 0, SendDataToClient.Length, 0);
            }
        }
    }
}
