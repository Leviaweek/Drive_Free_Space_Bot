using System.Text.Json.Serialization;

namespace FreeSpacePercentBot;

public class TelegramBotRequest
{
    [JsonPropertyName("chatId")]
    public required long ChatId { get; set; }
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}