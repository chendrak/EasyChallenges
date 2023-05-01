namespace EasyChallenges.Services;

using Common.Logging;
using Helpers;
using RogueGenesia.Data;
using RogueGenesia.GameManager;
using RogueGenesia.UI;

internal static class ChallengeEventHandler
{
    public static void Initialize()
    {
        GameEventManager.OnGameStart.AddListener(OnNewRunStarted);
    }

    private static void OnNewRunStarted()
    {
        Log.Debug("OnNewRunStarted");

        GameEventManager.OnStageStart.AddListener(OnStageEntered);
        GameEventManager.OnCardBanished.AddListener(OnCardBanished);
        GameEventManager.OnCardPickedUp.AddListener(OnCardPickedUp);
        GameEventManager.OnCardRemoved.AddListener(OnCardRemoved);

        if (ChallengeData.InChallenge && ChallengeData.ActualChallenge)
        {
            var challengeName = ChallengeData.ActualChallenge.name;

            var startingCards = CustomChallengeModifierHolder.GetStartingCardsForChallenge(challengeName);
            foreach (var cardName in startingCards)
            {
                var cardSO = CardHelper.GetCardForName(cardName);
                if (cardSO)
                {
                    Log.Debug($"Adding card {cardName}");

                    Log.Debug($"Number of held cards before adding: {GameData.PlayerDatabase[0]._soulCardSOList.Count}");
                    GameData.PlayerDatabase[0].AddSoulCardFromSO(cardSO);

                    Log.Debug($"Number of held cards AFTER adding: {GameData.PlayerDatabase[0]._soulCardSOList.Count}");
                    if (PauseMenu.instance)
                    {
                        Log.Debug("Adding card to pause menu");
                        PauseMenu.instance.AddSoulCard(cardSO, 1);
                    }
                    else
                    {
                        Log.Debug("No Pause Menu Instance");
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
                    GameData.BanishCard(cardSO);
                }
            }
        }
    }

    private static void OnCardRemoved(SoulCard card)
    {
        Log.Debug($"OnCardRemoved({card.GetName()})");
    }

    private static void OnCardPickedUp(SoulCard card)
    {
        Log.Debug($"OnCardPickedUp({card.GetName()})");
    }

    private static void OnCardBanished(SoulCardScriptableObject cardSo)
    {
        Log.Debug($"OnCardBanished({cardSo.name})");
    }

    private static void OnStageEntered(LevelObject level)
    {
        Log.Debug("OnStageEntered");
    }

    public static void Cleanup()
    {
        GameEventManager.OnGameStart.RemoveListener(OnNewRunStarted);
        GameEventManager.OnStageStart.RemoveListener(OnStageEntered);
        GameEventManager.OnCardBanished.RemoveListener(OnCardBanished);
        GameEventManager.OnCardPickedUp.RemoveListener(OnCardPickedUp);
        GameEventManager.OnCardRemoved.RemoveListener(OnCardRemoved);
    }
}
