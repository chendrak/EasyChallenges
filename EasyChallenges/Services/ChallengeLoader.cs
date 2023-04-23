namespace EasyChallenges.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Logging;
using Helpers;
using Models.Templates;
using ModGenesia;
using RogueGenesia.Data;

public static class ChallengeLoader
{
    private static Dictionary<string, ChallengeTemplate> successFullyLoadedChallenges = new();
    private static Dictionary<string, ChallengeTemplate> challengesThatFailedToLoad = new();

    private static Dictionary<string, int> validDifficulties = new();
    private static Dictionary<string, int> validGameModes = new();

    public static void Initialize(List<string> modPaths)
    {
        initInternalLists();

        LoadChallenges(modPaths);
    }

    private static void initInternalLists()
    {
        var gameModes = GameDataGetter.GetAllGameMode();

        validDifficulties = gameModes
            .SelectMany(gameMode => gameMode.DifficultyList.ToArray())
            .ToDictionary(x => x.name, x => x.DifficultyID);

        validGameModes = gameModes.ToDictionary(x => x.name, x => x.GameModeID);

        Log.Debug($"Valid Game Modes: {string.Join(", ", validGameModes.Keys)}");
        Log.Debug($"Valid Difficulties: {string.Join(", ", validDifficulties.Keys)}");
    }

    private static bool IsValidDifficulty(string difficulty) => validDifficulties.Keys.Contains(difficulty);
    private static bool IsValidGameMode(string gameMode) => validGameModes.Keys.Contains(gameMode);

    private static int fixDifficulty(string difficulty)
    {
        if (IsValidDifficulty(difficulty))
            return validDifficulties[difficulty];
        if (difficulty.Length == 1)
        {
            var fixedDifficulty = $"Rog{difficulty}";
            if (IsValidDifficulty(fixedDifficulty))
                return validDifficulties[fixedDifficulty];
        }

        return validDifficulties.Values.First();
    }

    private static int ensureGameMode(string gameMode)
    {
        if (IsValidGameMode(gameMode))
            return validGameModes[gameMode];
        return validGameModes.Values.First();
    }

    private static void LoadChallenges(List<string> modPaths)
    {
        Log.Info("Attempting to find custom challenges");

        foreach (var modPath in modPaths)
        {
            // Scan for *.challenges.json files in mods subfolders
            var challengeJsonFiles = Directory.GetFiles(modPath, "*.challenges.json", SearchOption.AllDirectories);
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
    }

    private static CustomChallengeDescription GetModSourceDescription(string challengeName, string modSource) =>
        new()
        {
            DescriptionType = CustomChallengeDescription.EDescriptionType.Negative,
            Key = $"{challengeName}_ModSource",
            localization = Localization.GetTranslations(new()
            {
                ["en"] = $"Mod: {modSource}"
            })
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

        var modSource = templateFile.ModSource ?? EasyChallenges.MOD_NAME;
        foreach (var template in templateFile.Challenges)
        {
            Log.Debug($"Attempting to add {template.Name}");
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
                localization = Localization.GetChallengeDescriptionTranslations(descriptionTemplate)
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
                var challengeSo = ChallengeAPI.AddCustomChallenge(
                    template.Name,
                    ensureGameMode(template.GameMode),
                    fixDifficulty(template.Difficulty),
                    template.SoulCoinModifier,
                    challengeModifier, template.IsHardMode,
                    Localization.GetNameTranslations(template),
                    template.Order,
                    challengeDescriptions
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
                        Log.Debug($"Banished cards by stat: {banishedCardsByStat.Count}");
                        CustomChallengeModifierHolder.AddBanishedCardsForChallenge(template.Name, banishedCardsByStat);
                    }
                }

                CustomChallengeModifierHolder.SetStartingCardsForChallenge(template.Name, template.ChallengeModifier.StartingCards);

                Log.Debug($"Added challenge {template.Name}");
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
