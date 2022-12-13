namespace EasyChallenges.Models.Templates;

using EasyChallenges.Models.Templates.Generated;

public class ModifierTemplate
{
    public float ModifierValue { get; set; }

    public TemplateModifierType ModifierType { get; set; }

    public TemplateStatsType Stat { get; set; }

    public override string ToString()
    {
        return $"{nameof(ModifierValue)}: {ModifierValue}, {nameof(ModifierType)}: {ModifierType}, {nameof(Stat)}: {Stat}";
    }
}
