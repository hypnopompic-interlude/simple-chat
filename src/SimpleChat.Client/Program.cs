// See https://aka.ms/new-console-template for more information
using SimpleChat.Client;

Console.WriteLine("Hello, Chat!");

var client = ChatClient.Instance;

Console.WriteLine("Type a name then press enter.");

int counter = 0;

string userName = string.Empty;

while (counter++ < 42 && client.IsConnected is false)
{
    userName = Console.ReadLine();

    Console.WriteLine(userName);

    await client.ConnectAsync(userName);
}

if (counter == 42) return;

Console.WriteLine("Send message to chat");

while (client is not null && client.IsConnected)
{
    var text = Console.ReadLine();

    await client.SendMessageAsync(text);
}

Console.WriteLine(string.Format("{0} has leaved chat"), userName);
