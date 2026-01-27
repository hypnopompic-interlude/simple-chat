// See https://aka.ms/new-console-template for more information
using SimpleChat.Client;

Console.WriteLine("Hello, Chat!");

var client = ChatClient.Instance;


int counter = 0;

string userName = string.Empty;

while (counter++ < 42 && client.IsConnected is false)
{
    Console.WriteLine("Type a name then press enter.");

    userName = Console.ReadLine();

    await client.ConnectAsync(userName);    

    if (client.IsConnected) break;
}
Console.WriteLine("You are connected to chat");

if (counter == 42) return;

Console.WriteLine("Send message to chat");

while (client is not null && client.IsConnected)
{
    var text = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(text)) continue;

    await client.SendMessageAsync(text);
}

Console.WriteLine(string.Format("{0} has leaved chat"), userName);
