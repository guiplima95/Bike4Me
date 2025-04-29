using Bike4Me.Application.Abstractions.Messaging;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bike4Me.Infrastructure.EventBus.Converters;

public class MessageConverter(Assembly? assemblyToScan = null) : JsonConverter<Message>
{
    private readonly Assembly _assemblyToScan = assemblyToScan ?? Assembly.GetExecutingAssembly();

    public override Message? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;

        if (!root.TryGetProperty("MessageType", out var messageTypeProp))
        {
            throw new JsonException("MessageType property not found in JSON.");
        }

        string messageTypeName = messageTypeProp.GetString()!;

        var messageType = _assemblyToScan.GetTypes()
            .FirstOrDefault(t => t.Name.Equals(messageTypeName, StringComparison.OrdinalIgnoreCase)
                              && typeof(Message).IsAssignableFrom(t)
                              && !t.IsAbstract) ?? throw new NotSupportedException($"Tipo de mensagem desconhecido: {messageTypeName}");

        var message = (Message?)JsonSerializer.Deserialize(root.GetRawText(), messageType, options);

        return message;
    }

    public override void Write(Utf8JsonWriter writer, Message value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}