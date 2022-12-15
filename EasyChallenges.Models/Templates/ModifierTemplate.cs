namespace EasyChallenges.Models.Templates;

using Generated;

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
