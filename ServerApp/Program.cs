using System.Net;
using System.Net.Sockets;
using System.Text;

string serverstr = "127.0.0.1";
int port = 8080;

IPAddress.TryParse(serverstr, out IPAddress? ipAdress);

if(ipAdress is null)
    return;

IPEndPoint endPoint = new IPEndPoint(
    ipAdress, port
    );

using Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

serverSocket.Bind(endPoint);
serverSocket.Listen();

using Socket clientSocket = serverSocket.Accept();

clientSocket.Send(Encoding.ASCII.GetBytes("(num is in [0; 100])"));
byte[] buffer = new byte[1024];

int num = Random.Shared.Next(0, 100);
Console.WriteLine($"{clientSocket}: num = {num}");

for (int i = 0; i < 5; i++)
{
    clientSocket.Receive(buffer);
    string message = Encoding.ASCII.GetString(buffer);

    if (int.TryParse(message, out int result) && result == num)
    {
        buffer = new byte[1024];
        clientSocket.Send(Encoding.ASCII.GetBytes($"you win"));
        clientSocket.Close();
        clientSocket.Disconnect(false);
        break;
    }
    else
    {
        clientSocket.Send(Encoding.ASCII.GetBytes($"you have {4 - i} trial(s)"));
        continue;
    }
    

}


