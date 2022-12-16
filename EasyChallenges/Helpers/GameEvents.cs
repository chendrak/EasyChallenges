namespace EasyChallenges.Helpers;

using HarmonyLib;
using RogueGenesia.Data;
using RogueGenesia.GameManager;
using RogueGenesia.Save;

[HarmonyPatch]
public static class GameEvents
{
    public delegate void OnNewRunStartedHandler();
    public delegate void OnGameStartHandler();
    public delegate void OnStageStartHandler();
    public delegate void OnSaveLoadedHandler();

    public static event OnNewRunStartedHandler OnNewRunStartedEvent;
    public static event OnGameStartHandler OnGameStartEvent;
    public static event OnStageStartHandler OnStageStartEvent;
    public static event OnSaveLoadedHandler OnStageOnSaveLoadedEvent;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), nameof(GameData.OnGameStart))]
    private static void GameData_OnGameStart() => OnGameStartEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameData), nameof(GameData.OnStartNewGame))]
    private static void GameData_OnStartNewGame() => OnNewRunStartedEvent?.Invoke();

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GameManagerRogue), nameof(GameManagerRogue.Start))]
    private static void GameManagerRogue_Start_Prefix() => OnStageStartEvent?.Invoke();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.LoadCurrentGame))]
    private static void SaveManager_LoadCurrentGame() => OnStageOnSaveLoadedEvent?.Invoke();
}
