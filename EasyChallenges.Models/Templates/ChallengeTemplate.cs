using RogueGenesia.Data;

namespace EasyChallenges.Models.Templates;

public class ChallengeTemplate
{
    public string Name;

    public Dictionary<string, string> NameLocalization { get; set; } = new();

    public EDifficulty Difficulty;

    public ChallengeModifierTemplate ChallengeModifier;

    public float SoulCoinModifier;

    public int Order;

    public bool IsHardMode;
}
