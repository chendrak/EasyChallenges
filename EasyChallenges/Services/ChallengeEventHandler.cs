namespace EasyChallenges.Services;

using Common.Logging;
using Helpers;
using RogueGenesia.Data;
using RogueGenesia.UI;

internal static class ChallengeEventHandler
{

    private static bool challengeCardsProcessed = false;

    public static void Initialize()
    {
        GameEvents.OnNewRunStartedEvent += OnNewRunStarted;
        GameEvents.OnStageStartEvent += OnStageEntered;
        GameEvents.OnStageOnSaveLoadedEvent += OnSaveLoaded;
    }

    private static void OnSaveLoaded()
    {
        // Set this here, so we don't re-add cards after a save is loaded
        challengeCardsProcessed = true;
    }

    private static void OnStageEntered()
    {
        if (challengeCardsProcessed) return;

        if (ChallengeData.InChallenge && ChallengeData.ActualChallenge)
        {
            var challengeName = ChallengeData.ActualChallenge.name;

            var startingCards = CustomChallengeModifierHolder.GetStartingCardsForChallenge(challengeName);
            foreach (var cardName in startingCards)
            {
                var cardSO = CardHelper.GetCardForName(cardName);
                if (cardSO)
                {
                    Log.Info($"Adding card {cardName}");
                    GameData.PlayerDatabase[0].AddSoulCardFromSO(cardSO);
                    if (PauseMenu.instance)
                    {
                        PauseMenu.instance.AddSoulCard(cardSO, 1);
                    }
                }
                else
                {
                    Log.Warn($"Couldn't find card {cardName}, so it won't be added");
                }
            }

            var banishedCards = CustomChallengeModifierHolder.GetBanishedCardsForChallenge(challengeName);
            foreach (var cardName in banishedCards)
            {
                var cardSO = CardHelper.GetCardForName(cardName);
                if (cardSO)
                {
                    GameData.BanishedCard.Add(cardSO);
                }
            }

            challengeCardsProcessed = true;
        }
    }

    private static void OnNewRunStarted() => challengeCardsProcessed = false;
}
