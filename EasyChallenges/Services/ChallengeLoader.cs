namespace EasyChallenges.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public static void Initialize()
    {
        GameEvents.OnGameLaunchEvent += OnGameStarted;
    }

    private static void OnGameStarted()
    {
        Log.Info($"ChallengeLoader.OnGameStarted");
        LoadChallenges();
        GameData.RefreshDifficultyAndChallenges();
        GameData.LoadPersitentData();
    }

    private static void LoadChallenges()
    {
        // Scan for *.cards.json files in plugins subfolders
        var challengeJsonFiles = Directory.GetFiles(Paths.Plugins, "*.challenges.json", SearchOption.AllDirectories);
        foreach (var jsonFile in challengeJsonFiles)
        {
            try
            {
                AddChallengesFromFile(jsonFile);
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

    private static void AddChallengesFromFile(string fileName)
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

            var challengeDescriptions = new List<CustomChallengeDescription>();

            var startingCardDescription = CustomChallengeModifierHolder.GetStartingCardsDescription(template.Name, template.ChallengeModifier.StartingCards);
            if (startingCardDescription != null)
            {
                challengeDescriptions.Add(startingCardDescription);
            }

            var banishedCardDescription = CustomChallengeModifierHolder.GetBanishedCardsDescription(template.Name, template.ChallengeModifier.BanishedCards);
            if (banishedCardDescription != null)
            {
                challengeDescriptions.Add(banishedCardDescription);
            }

            var convertedDescriptions = template.Descriptions.ConvertAll(descriptionTemplate => new CustomChallengeDescription
            {
                DescriptionType = descriptionTemplate.Type,
                Key = $"{template.Name}_{descCnt++}",
                localization = Localization.GetChallengeDescriptionTranslations(descriptionTemplate).ToIl2CppList()
            });

            challengeDescriptions.AddRange(convertedDescriptions);

            var modNameChallengeDescription = GetModSourceDescription(template.Name, modSource);

            challengeDescriptions.Add(modNameChallengeDescription);

            var banishedStatsDesc = CustomChallengeModifierHolder.GetBanishedCardStatsDescription(template.Name,
                template.ChallengeModifier.BanishedCardStats);

            if (banishedStatsDesc != null)
            {
                challengeDescriptions.Add(banishedStatsDesc);
            }

            try
            {
                var challengeModifier = template.ChallengeModifier.ToChallengeModifier();
                ChallengeAPI.AddCustomChallenge(
                    template.Name,
                    (int)template.GameMode,
                    (int)template.Difficulty - 1,
                    template.SoulCoinModifier,
                    challengeModifier, template.IsHardMode,
                    Localization.GetNameTranslations(template).ToIl2CppList(),
                    template.Order,
                    challengeDescriptions.ToIl2CppList()
                );

                CustomChallengeModifierHolder.SetBanishedCardsForChallenge(template.Name, template.ChallengeModifier.BanishedCards);

                if (template.ChallengeModifier.BanishedCardStats.Count > 0)
                {
                    var banishedCardsByStat = template.ChallengeModifier.BanishedCardStats
                        .SelectMany(stat => CardHelper.GetCardsForStat((StatsType)stat))
                        .Select(card => card.name)
                        .ToList();

                    if (banishedCardsByStat.Count > 0)
                    {
                        Log.Info($"Banished cards by stat: {banishedCardsByStat.Count}");
                        CustomChallengeModifierHolder.AddBanishedCardsForChallenge(template.Name, banishedCardsByStat);
                    }
                }

                CustomChallengeModifierHolder.SetStartingCardsForChallenge(template.Name, template.ChallengeModifier.StartingCards);

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
