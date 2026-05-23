using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gerenciamento_Escolar.Models;

public class JsonDateConverter : JsonConverter<DateOnly>
{
    private const string formatoBrasileiro = "dd/MM/yyyy";
    
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? texto = reader.GetString();

        if (string.IsNullOrWhiteSpace(texto))
        {
            return default;
        }

        return DateOnly.ParseExact(texto, formatoBrasileiro);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(formatoBrasileiro));
    }
}