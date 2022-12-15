namespace EasyChallenges.Models.Templates;

using RogueGenesia.Data;

public class ChallengeDescriptionTemplate
{
    public CustomChallengeDescription.EDescriptionType Type { get; set; } =
        CustomChallengeDescription.EDescriptionType.Neutral;
    public Dictionary<string, string> Localizations { get; set; } = new();
}
