using System.Text.Json.Serialization;

namespace Lagrange.OneBot.Core.Entity.Action;

[Serializable]
public class OneBotSetGroupBothd
{
    [JsonPropertyName("group_id")] public uint GroupId { get; set; }
    
    [JsonPropertyName("bot_id")] public uint BotId { get; set; }

    [JsonPropertyName("data_1")] public string? Data_1 { get; set; }

    [JsonPropertyName("data_2")] public string? Data_2 { get; set; }
    
}