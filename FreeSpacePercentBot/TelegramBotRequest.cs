using System.Text.Json.Serialization;

namespace FreeSpacePercentBot;

public class TelegramBotRequest
{
    [JsonPropertyName("chat_id")]
    public required long ChatId { get; set; }
    [JsonPropertyName("text")]
    public required string Message { get; set; }
}