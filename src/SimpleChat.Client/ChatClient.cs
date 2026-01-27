using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using SimpleChat.Shared.Chats;

namespace SimpleChat.Client;

public class ChatClient : IChatHubReceiver
{
    public static ChatClient Instance => new();

    private IChatHub _joinHub;
    private IChatHub _workerHub;
    private GrpcChannel _channel;
    private string _bearer;
    private readonly IMessageBuilder _messageBuilder = new ChatMessageBuilder();
    private readonly string _host = "http://localhost:5013"; // "https://localhost:7169"


    public bool IsConnected => string.IsNullOrWhiteSpace(_bearer) is false;

    public async Task ConnectAsync(string userName)
    {
        try
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler = new HttpClientHandler();

            httpHandler.UseProxy = false;
            httpHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _channel = GrpcChannel.ForAddress(_host, new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            _joinHub = await StreamingHubClient.ConnectAsync<IChatHub, IChatHubReceiver>(_channel, this);

            await _joinHub.JoinAsync(userName);
        }
        catch (RpcException ex) 
        {
            Console.WriteLine(ex.Message);

            Console.WriteLine("Chat is unavailable. server connection error. Try again later");
        }
        finally
        {
            _joinHub.DisposeAsync().GetAwaiter().GetResult();
        }
    }

    public async Task SendMessageAsync(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            OnError(new ErrorModel { Code = "Error.MessageEmpty", Message = "Message is Empty" });

            return;
        }

        await _workerHub.SendMessageAsync(new SendMessageRequest { Content = message});
    }

    public async Task DisconnectAsync()
    {
        await _workerHub.LeaveAsync();
        await _workerHub.DisposeAsync();
        await _channel.ShutdownAsync();
    }

    public void OnError(ErrorModel error)
    {
        Console.WriteLine(string.Format("Error: {0} - {1}"), error.Code, error.Message);
    }

    public void OnReceiveMessage(MessageRecievedEvent message)
    {
        var result = _messageBuilder.WithPrefix(message.Message.Author).WithContent(message.Message.Text).Build();

        if (result.IsFailure) return;
        
        Console.WriteLine(result.Value);
    }

    public void OnUserJoined(string token)
    {
        _bearer = token;

        //Console.WriteLine(token);

        var headers = new Metadata();
        
        headers.Add("Authorization", string.Format("Bearer {0}", _bearer)); 

        var callOptions = new CallOptions(headers: headers);

        _workerHub = StreamingHubClient
            .ConnectAsync<IChatHub, IChatHubReceiver>(_channel, this, _host, callOptions)
            .GetAwaiter()
            .GetResult();
    }
}