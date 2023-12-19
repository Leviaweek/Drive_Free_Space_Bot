using System.Text.Json.Serialization;

namespace FreeSpacePercentBot;

[JsonSerializable(typeof(TelegramBotRequest))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(long))]
public partial class TelegramBotRequestContext: JsonSerializerContext
{
    
}