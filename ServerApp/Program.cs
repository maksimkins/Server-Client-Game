using System;
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

byte[] buffer = new byte[1024];

Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
serverSocket.Bind(endPoint);
serverSocket.Listen();

while (true)
{
    try
    {
        Socket clientSocket = await serverSocket.AcceptAsync();

        Console.WriteLine($"{clientSocket.RemoteEndPoint} connected");

        Thread thread = new Thread(async () =>
        {
            try
            {
                await clientSocket.SendAsync(Encoding.ASCII.GetBytes("(num is in [0; 100])"));

                int num = Random.Shared.Next(0, 100);
                Console.WriteLine($"{clientSocket}: num = {num}");

                for (int i = 0; i < 5; i++)
                {
                    await clientSocket.ReceiveAsync(buffer);
                    string message = Encoding.ASCII.GetString(buffer);

                    if (int.TryParse(message, out int result) && result == num)
                    {
                        buffer = new byte[1024];
                        await clientSocket.SendAsync(Encoding.ASCII.GetBytes($"you win"));
                        await Console.Out.WriteLineAsync($"{clientSocket.RemoteEndPoint} disconnected");
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        break;
                    }
                    else
                    {
                        await clientSocket.SendAsync(Encoding.ASCII.GetBytes($"you have {4 - i} trial(s)"));
                        continue;
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        });
        thread.Start();
    }
    catch(Exception ex)
    {
        await Console.Out.WriteLineAsync($"error occured: {ex.Message}");
    }
    
}

    



