using BepInEx;
using BepInEx.Unity.IL2CPP;
using EasyChallenges.Common.Helpers;
using EasyChallenges.Helpers;
using EasyChallenges.Services;
using SemanticVersioning;
using DLog = EasyChallenges.Common.Logging.Log;

namespace EasyChallenges
{
    [BepInDependency(DependencyGUID: "ModManager", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class EasyChallenges : BasePlugin
    {
        private readonly Version MinimumRequiredGameVersion = new(0, 7, 2, preRelease: ".0b-beta");

        public override void Load()
        {
            if (!VersionHelper.IsGameVersionAtLeast(this.MinimumRequiredGameVersion))
            {
                DLog.Error($"Wrong game version! Minimum required game version is {this.MinimumRequiredGameVersion}, you have {VersionHelper.GameVersion}");
                this.Unload();
                return;
            }

            HarmonyPatchHelper.ApplyPatches("EasyChallenges");

            ChallengeEventHandler.Initialize();
            ChallengeLoader.Initialize();
        }

        // public override string ModDescription() => $"Loaded challenges: 999";
    }
}
