using RogueGenesia.Data;

namespace EasyChallenges.Models.Templates;

public class ChallengeTemplate
{
    public string Name;

    public Dictionary<string, string> NameLocalization { get; set; } = new();

    public List<ChallengeDescriptionTemplate> Descriptions { get; set; } = new();

    public EDifficulty Difficulty = EDifficulty.F;

    public GameData.EGameMode GameMode = GameData.EGameMode.RogMode;

    public ChallengeModifierTemplate ChallengeModifier;

    public float SoulCoinModifier = 1.0f;

    public int Order = 9;

    public bool IsHardMode;
}
