using System.Collections.Generic;
using EasyChallenges.Models.Templates;
using RogueGenesia.Data;

namespace EasyChallenges.Helpers;

using Common.Logging;

public static class Localization
{
    public static List<LocalizationData> GetTranslations(Dictionary<string, string> translations)
    {
        var result = new List<LocalizationData>();

        foreach (var (localizationKey, translation) in translations)
        {
            var locale = GetLocaleForKey(localizationKey);

            if (locale == null)
            {
                Log.Warn($"\tLocale {localizationKey} not supported!");
                continue;
            }

            var ld = new LocalizationData
            {
                Key = locale,
                Value = translation
            };

            result.Add(ld);
        }

        return result;
    }

    public static List<LocalizationData> GetNameTranslations(ChallengeTemplate challengeTemplate) => GetTranslations(challengeTemplate.NameLocalization);

    public static List<LocalizationData> GetChallengeDescriptionTranslations(ChallengeDescriptionTemplate descriptionTemplate) => GetTranslations(descriptionTemplate.Localizations);

    private static UnityEngine.Localization.Locale GetLocaleForKey(string localizationKey)
    {
        foreach (var locale in ModGenesia.ModGenesia.GetLocales())
        {
            if (locale.Identifier.Code == localizationKey)
            {
                return locale;
            }
        }

        return null;
    }
}
