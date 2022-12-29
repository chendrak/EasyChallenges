namespace EasyChallenges.Helpers;

using System.Collections.Generic;
using System.Linq;
using RogueGenesia.Data;

public static class CardHelper
{
    private static Dictionary<string, SoulCardScriptableObject> allCards = new();

    static CardHelper()
    {
        allCards = GameDataGetter.GetAllCards().ToDictionary(card => card.name, card => card);
    }

    public static SoulCardScriptableObject? GetCardForName(string name) =>
        allCards.ContainsKey(name) ? allCards[name] : null;

    public static List<SoulCardScriptableObject> GetCardsForStat(StatsType cardStat) =>
        allCards.Values.Where(card => card.StatsModifier.ContainsKey(cardStat)).ToList();
}
