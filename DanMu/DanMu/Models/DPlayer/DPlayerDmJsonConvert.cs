using System.Text.Json;
using System.Text.Json.Serialization;

namespace DanMu.Models.DPlayer;

public class DPlayerDmJsonConvert : JsonConverter<DPlayerDm>
{
  public override DPlayerDm? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();
    reader.Read();
    var time = reader.GetSingle();
    reader.Read();
    var type = reader.GetInt32();
    reader.Read();
    var color = reader.GetUInt32();
    reader.Read();
    var author = reader.GetString();
    reader.Read();
    var text = reader.GetString();
    reader.Read();
    if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException();
    return new DPlayerDm
    {
      Time = time,
      Type = type,
      Color = color,
      Author = author ?? string.Empty,
      Text = text ?? string.Empty
    };
  }

  public override void Write(Utf8JsonWriter writer, DPlayerDm value, JsonSerializerOptions options)
  {
    writer.WriteStartArray();

    writer.WriteNumberValue(value.Time);
    writer.WriteNumberValue(value.Type);
    writer.WriteNumberValue(value.Color);
    writer.WriteStringValue(value.Author);
    writer.WriteStringValue(value.Text);

    writer.WriteEndArray();
  }
}

public class DPlayerDmListJsonConverter : JsonConverter<List<DPlayerDm>>
{
  public override List<DPlayerDm> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.StartArray)
      throw new JsonException();

    var op = new JsonSerializerOptions(options)
    {
      Converters = { new DPlayerDmJsonConvert() }
    };

    var list = new List<DPlayerDm>();

    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndArray)
        break;

      var item = JsonSerializer.Deserialize<DPlayerDm>(ref reader, op);
      if (item != null) list.Add(item);
    }

    return list;
  }

  public override void Write(Utf8JsonWriter writer, List<DPlayerDm> value, JsonSerializerOptions options)
  {
    var op = new JsonSerializerOptions(options)
    {
      Converters = { new DPlayerDmJsonConvert() }
    };
    writer.WriteStartArray();

    foreach (var item in value) JsonSerializer.Serialize(writer, item, op);

    writer.WriteEndArray();
  }
}