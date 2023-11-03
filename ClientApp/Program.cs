using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

string serverstr = "127.0.0.1";
int port = 8080;

IPAddress.TryParse(serverstr, out IPAddress? ipAdress);

if (ipAdress is null)
    return;

IPEndPoint endPoint = new IPEndPoint(
    ipAdress, port
    );

Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

serverSocket.Connect(endPoint);

byte[] buffer = new byte[1024];
serverSocket.Receive(buffer);
Console.WriteLine(Encoding.ASCII.GetString(buffer));


for (int i = 0; i < 5; i++)
{
    //if (!serverSocket.Connected)
    //{
    //    break;
    //}

    Array.Clear(buffer);
    Console.Write("input num: ");
    string num = Console.ReadLine() ?? "";

    serverSocket.Send(Encoding.ASCII.GetBytes(num));
    serverSocket.Receive(buffer);
    Console.WriteLine(Encoding.ASCII.GetString(buffer));

}


