using System.Collections.Generic;
using EasyChallenges.Models.Templates;
using RogueGenesia.Data;

namespace EasyChallenges.Helpers;

public static class Localization
{
    public static List<LocalizationData> GetTranslations(Dictionary<string, string> translations)
    {
        var result = new List<LocalizationData>();

        foreach (var translation in translations)
        {
            var ld = new LocalizationData
            {
                Key = translation.Key,
                Value = translation.Value
            };

            result.Add(ld);
        }

        return result;
    }

    public static List<LocalizationData> GetNameTranslations(ChallengeTemplate challengeTemplate) => GetTranslations(challengeTemplate.NameLocalization);

    public static List<LocalizationData> GetChallengeDescriptionTranslations(ChallengeDescriptionTemplate descriptionTemplate) => GetTranslations(descriptionTemplate.Localizations);
}
