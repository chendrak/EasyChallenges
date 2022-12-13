namespace EasyChallenges.Models.Templates;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class TemplateFile
{
    private const string SchemaFileName = "schema.json";

    [JsonPropertyName("$schema")]
    public string Schema
    {
        get => SchemaFileName;
        // ReSharper disable once ValueParameterNotUsed - This is only here to appease the schema generation gods.
        set { }
    }

    public string? ModSource { get; set; }
    public List<ChallengeTemplate> Challenges { get; set; } = new();
}
