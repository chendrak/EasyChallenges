namespace EasyChallenges.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Extensions;
using Common.Logging;
using HarmonyLib;
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

        var modSource = templateFile.ModSource ?? MyPluginInfo.PLUGIN_NAME;

        foreach (var template in templateFile.Challenges)
        {
            Log.Info($"Attempting to add {template.Name}. Challenges before: {GameData.ChallengesList.Count}");
            try
            {
                var challengeModifier = template.ChallengeModifier.ToChallengeModifier();
                var challengeSO = ModGenesia.AddCustomChallenge(
                    template.Name, template.Difficulty,
                    template.SoulCoinModifier,
                    challengeModifier, template.IsHardMode,
                    Localization.GetNameTranslations(template).ToIl2CppList(),
                    template.Order
                );

                Log.Info($"Challenges after {GameData.ChallengesList.Count}");

                GameData.ChallengesList = GameData.ChallengesList.AddItem(challengeSO).ToList().ToIl2CppReferenceArray();

                Log.Info($"Added challenge {template.Name}: {Log.StructToString(challengeSO)}");
                Log.Info($"Challenges after 2: {GameData.ChallengesList.Count}");

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
