using System.Text.Json;
using System.Text.Json.Serialization;

namespace VianaHub.Global.Gerit.Api.Converters;

/// <summary>
/// Conversor JSON customizado para serializar Guids sem hífens.
/// Facilita a cópia por duplo clique para os usuários.
/// </summary>
public class GuidNoDashesConverter : JsonConverter<Guid>
{
    /// <summary>
    /// Lê um Guid do JSON, aceitando formato com ou sem hífens.
    /// </summary>
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
            return Guid.Empty;

        // Aceita ambos os formatos:
        // - Com hífens: f3890433-c100-4470-a322-0a14f2868782
        // - Sem hífens: f3890433c1004470a3220a14f2868782
        return Guid.Parse(value);
    }

    /// <summary>
    /// Escreve um Guid no JSON sem hífens (formato "N").
    /// Exemplo: f3890433c1004470a3220a14f2868782
    /// </summary>
    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        // Formato "N" = 32 caracteres hexadecimais sem hífens
        writer.WriteStringValue(value.ToString("N"));
    }
}

