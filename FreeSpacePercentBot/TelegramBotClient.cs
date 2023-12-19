using System.Net;
using System.Text;
using System.Text.Json;

namespace FreeSpacePercentBot;

public class TelegramBotClient
{
    private const string Url = "https://api.telegram.org/bot{0}/{1}";
    private readonly string _token;
    private readonly long _chatId;
    public TelegramBotClient(string token, long chatId)
    {
        _token = token;
        _chatId = chatId;
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        var apiUrl = string.Format(Url, _token, "sendMessage");
        using var httpClient = new HttpClient();
        var request = new TelegramBotRequest{ChatId = _chatId, Message = message};
        var json = JsonSerializer.Serialize(request, typeof(TelegramBotRequest), TelegramBotRequestContext.Default);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(apiUrl, content, cancellationToken);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            Console.WriteLine("Error sending message to chat {0}", _chatId);
            Console.WriteLine($"HttpErrorCode {(int)response.StatusCode}");
            return;
        }
        Console.WriteLine("Message sent to chat {0}", _chatId);
    }
}