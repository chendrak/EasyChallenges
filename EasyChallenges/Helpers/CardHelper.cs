namespace EasyChallenges.Helpers;

using System.Collections.Generic;
using System.Linq;
using RogueGenesia.Data;

public static class CardHelper
{
    private static Dictionary<string, SoulCardScriptableObject> allCards = new();

    static CardHelper()
    {
        allCards = GameData.GetAllCards().ToDictionary(card => card.name, card => card);
    }

    public static SoulCardScriptableObject? GetCardForName(string name) =>
        allCards.ContainsKey(name) ? allCards[name] : null;
}
