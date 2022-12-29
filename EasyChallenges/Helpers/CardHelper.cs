namespace EasyChallenges.Helpers;

using System.Collections.Generic;
using System.Linq;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using RogueGenesia.Data;

public static class CardHelper
{
    private static Il2CppReferenceArray<SoulCardScriptableObject> allCards => GameDataGetter.GetAllCards();

    public static SoulCardScriptableObject? GetCardForName(string name) =>
        allCards.FirstOrDefault(card => card.name == name);

    public static List<SoulCardScriptableObject> GetCardsForStat(StatsType cardStat) =>
        allCards.Where(card => card.StatsModifier.ContainsKey(cardStat)).ToList();
}
