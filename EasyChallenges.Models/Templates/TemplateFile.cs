namespace EasyChallenges.Models.Templates;

using System.Collections.Generic;

public class TemplateFile
{
    public string? ModSource { get; set; }
    public List<ChallengeTemplate> Challenges { get; set; } = new();
}
