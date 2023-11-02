using System.Net;

string serverString = "127.0.0.1:8080";

IPEndPoint.TryParse(serverString, out IPEndPoint? endPoint);

if(endPoint is null)
    return;