using System.Text;
using System.Text.Json.Nodes;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Compression;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(LightAppElem))]
public class LightAppEntity : IMessageEntity
{
    public string AppName { get; set; } = string.Empty;
    public string BizSrc { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;
    
    public LightAppEntity() { }

    public LightAppEntity(string payload)
    {
        Payload = payload;
        var j = JsonNode.Parse(payload);
        string? app = j?["app"]?.ToString();
        if (app != null) AppName = app;
        string? bizsrc = j?["bizsrc"]?.ToString();
        if (bizsrc != null) BizSrc = bizsrc;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        using var payload = new BinaryPacket()
            .WriteByte(0x01)
            .WriteBytes(ZCompression.ZCompress(Encoding.UTF8.GetBytes(Payload)));

        return new Elem[]
        {
            new()
            {
                LightAppElem = new LightAppElem
                {
                    Data = payload.ToArray(),
                    MsgResid = null
                }
            }
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.LightAppElem is { } lightApp)
        {
            var payload = ZCompression.ZDecompress(lightApp.Data.AsSpan(1), false);
            string json = Encoding.UTF8.GetString(payload);
            var j = JsonNode.Parse(json);
            string? app = j?["app"]?.ToString();
            string? bizsrc = j?["bizsrc"]?.ToString();
            var appEntity = new LightAppEntity
            {
                Payload = json
            };
            if (app != null)
            {
                appEntity.AppName = app;
            }
            if (bizsrc != null)
            {
                appEntity.BizSrc = bizsrc;
            }
            return appEntity;
        }

        return null;
    }

    public string ToPreviewString()
    {
        return $"[{nameof(LightAppEntity)}: {AppName}]";
    }
}
