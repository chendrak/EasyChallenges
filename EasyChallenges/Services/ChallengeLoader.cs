namespace EasyChallenges.Services;

using System;
using System.Collections.Generic;
using System.IO;
using Common;
using Common.Extensions;
using Common.Logging;
using Helpers;
using Models.Templates;
using ModGenesia;
using RogueGenesia.Data;

public static class ChallengeLoader
{
    private static Dictionary<string, ChallengeTemplate> successFullyLoadedChallenges = new();
    private static Dictionary<string, ChallengeTemplate> challengesThatFailedToLoad = new();

    public static void LoadChallenges()
    {
        // Scan for *.cards.json files in plugins subfolders
        var challengeJsonFiles = Directory.GetFiles(Paths.Plugins, "*.challenges.json", SearchOption.AllDirectories);
        foreach (var jsonFile in challengeJsonFiles)
        {
            try
            {
                var assetPath = Path.GetDirectoryName(jsonFile);
                AddChallengesFromFile(jsonFile, assetPath!);
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to load challenges from file {jsonFile}: {ex}");
            }
        }
    }

    private static CustomChallengeDescription GetModSourceDescription(string challengeName, string modSource) =>
        new()
        {
            DescriptionType = CustomChallengeDescription.EDescriptionType.Negative,
            Key = $"{challengeName}_ModSource",
            localization = Localization.GetTranslations(new()
            {
                ["en"] = $"Mod: {modSource}"
            }).ToIl2CppList()
        };

    private static void AddChallengesFromFile(string fileName, string assetBasePath)
    {
        if (!File.Exists(fileName))
        {
            Log.Error($"File does not exist: {fileName}");
        }

        Log.Info($"Loading challenges from file {fileName}");

        var json = File.ReadAllText(fileName);
        var templateFile = JsonDeserializer.Deserialize<TemplateFile>(json);

        Log.Info($"Loaded {templateFile.Challenges.Count} challenges");

        var modSource = templateFile.ModSource ?? ModInfo.ModName;
        foreach (var template in templateFile.Challenges)
        {
            Log.Info($"Attempting to add {template.Name}");
            var descCnt = 0;
            var challengeDescriptions = template.Descriptions.ConvertAll(descriptionTemplate => new CustomChallengeDescription
            {
                DescriptionType = descriptionTemplate.Type,
                Key = $"{template.Name}_{descCnt++}",
                localization = Localization.GetChallengeDescriptionTranslations(descriptionTemplate).ToIl2CppList()
            }).ToIl2CppList();

            var modNameChallengeDescription = GetModSourceDescription(template.Name, modSource);

            challengeDescriptions.Add(modNameChallengeDescription);

            try
            {
                var challengeModifier = template.ChallengeModifier.ToChallengeModifier();
                ChallengeAPI.AddCustomChallenge(
                    template.Name, template.Difficulty,
                    template.SoulCoinModifier,
                    challengeModifier, template.IsHardMode,
                    Localization.GetNameTranslations(template).ToIl2CppList(),
                    template.Order,
                    challengeDescriptions
                );

                Log.Info($"Added challenge {template.Name}");
                successFullyLoadedChallenges.Add(template.Name, template);
            }
            catch (Exception ex)
            {
                challengesThatFailedToLoad.Add(template.Name, template);
                Log.Error($"Error adding {template.Name}: {ex}");
            }
        }
    }
}
