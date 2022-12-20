namespace EasyChallenges.Models.Extensions;

using RogueGenesia.Data;
using Templates;
using Templates.Generated;

public static class ChallengeTemplateExtensions
{
    public static StatsModifier ConvertModifiersToStatsModifier(this ChallengeModifierTemplate challengeTemplate)
    {
        var statsMod = new StatsModifier();

        foreach (var modifier in challengeTemplate.Modifiers)
        {
            statsMod.ModifiersList.Add(modifier.CreateStatModifier());
        }

        return statsMod;
    }

    /// <summary>
    /// Converts this ModifierTemplate into something the game can understand.
    /// </summary>
    /// <returns>A <c>StatModifier</c> based on this <c>ModifierTemplate</c></returns>
    public static StatModifier CreateStatModifier(this ModifierTemplate modifierTemplate)
    {
        var singMod = new SingularModifier
        {
            Value = modifierTemplate.ModifierValue,
            ModifierType = modifierTemplate.ModifierType.CastTo<ModifierType>()
        };

        var statModifier = new StatModifier
        {
            Value = singMod,
            Key = modifierTemplate.Stat.ToString()
        };

        return statModifier;
    }
}
