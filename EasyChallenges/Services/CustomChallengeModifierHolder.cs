namespace EasyChallenges.Services;

using System.Collections.Generic;
using Common.Extensions;
using Helpers;
using RogueGenesia.Data;

public static class CustomChallengeModifierHolder
{
    private static Dictionary<string, List<string>> StartingCardsForChallenge = new();
    private static Dictionary<string, List<string>> BanishedCardsForChallenge = new();

    public static void SetStartingCardsForChallenge(string challengeName, List<string> startingCardNames) =>
        StartingCardsForChallenge[challengeName] = startingCardNames;

    public static void SetBanishedCardsForChallenge(string challengeName, List<string> banishedCardNames) =>
        BanishedCardsForChallenge[challengeName] = banishedCardNames;

    public static List<string> GetStartingCardsForChallenge(string challengeName) =>
        StartingCardsForChallenge.TryGetValue(challengeName, out var startingCards)
            ? startingCards
            : new List<string>();

    public static List<string> GetBanishedCardsForChallenge(string challengeName) =>
        BanishedCardsForChallenge.TryGetValue(challengeName, out var banishedCards)
            ? banishedCards
            : new List<string>();

    public static CustomChallengeDescription? GetStartingCardsDescription(string challengeName, List<string> cardNames)
    {
        if (cardNames.Count == 0) return null;

        var localizedCardNames = GetLocalizedCardNames(cardNames);
        if (localizedCardNames.Count == 0) return null;

        var localizedCardNameString = string.Join(", ", localizedCardNames);

        var description = ModGenesia.ChallengeAPI.BuildCustomChallengeDescription(
            Key: $"{challengeName}_StartingCards",
            descriptionType: CustomChallengeDescription.EDescriptionType.Neutral,
            localisedText: Localization.GetTranslations(new Dictionary<string, string>
            {
                ["en"] = $"Starting cards: {RGRichText.NextLevel(localizedCardNameString)}"
            }).ToIl2CppList()
        );

        return description;
    }

    public static CustomChallengeDescription? GetBanishedCardsDescription(string challengeName, List<string> cardNames)
    {
        if (cardNames.Count == 0) return null;

        var localizedCardNames = GetLocalizedCardNames(cardNames);
        if (localizedCardNames.Count == 0) return null;

        var localizedCardNameString = string.Join(", ", localizedCardNames);

        var description = ModGenesia.ChallengeAPI.BuildCustomChallengeDescription(
            Key: $"{challengeName}_BanishedCards",
            descriptionType: CustomChallengeDescription.EDescriptionType.Neutral,
            localisedText: Localization.GetTranslations(new Dictionary<string, string>
            {
                ["en"] =
                    $"Banished cards: {RGRichText.DebuffLevel(localizedCardNameString)}"
            }).ToIl2CppList()
        );

        return description;
    }

    private static List<string> GetLocalizedCardNames(List<string> cardNames)
    {
        var startingCardSOList = new List<SoulCardScriptableObject>();

        foreach (var cardName in cardNames)
        {
            var cardSo = CardHelper.GetCardForName(cardName);
            if (cardSo) startingCardSOList.Add(cardSo);
        }

        if (startingCardSOList.Count == 0) return new List<string>();

        return startingCardSOList.ConvertAll(card => card.GetLocalizedName());
    }
}
