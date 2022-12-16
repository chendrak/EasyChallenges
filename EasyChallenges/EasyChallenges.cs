using BepInEx;
using EasyChallenges.Common.Helpers;
using EasyChallenges.Services;

namespace EasyChallenges
{
    using BepInEx.Unity.IL2CPP;

    [BepInDependency(DependencyGUID: "ModManager", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class EasyChallenges : BasePlugin
    {
        public override void Load()
        {
            HarmonyPatchHelper.ApplyPatches("EasyChallenges");
            ChallengeEventHandler.Initialize();
            ChallengeLoader.Initialize();
        }

        // public override string ModDescription() => $"Loaded challenges: 999";
    }
}
