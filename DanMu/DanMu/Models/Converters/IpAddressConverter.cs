using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DanMu.Models.Converters;

public class IpAddressConverter : JsonConverter<IPAddress>
{
    public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            if (reader.TokenType == JsonTokenType.String)
                return IPAddress.Parse(reader.GetString() ?? string.Empty);
        reader.Read();

        var e = new Queue<byte>();

        while (reader.TokenType != JsonTokenType.EndArray)
        {
            e.Enqueue(JsonSerializer.Deserialize<byte>(ref reader, options)!);
            reader.Read();
        }

        return new IPAddress(e.ToArray());
    }

    public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.GetAddressBytes());
    }
}