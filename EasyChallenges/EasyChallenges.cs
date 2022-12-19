using BepInEx;
using EasyChallenges.Common.Helpers;
using EasyChallenges.Helpers;
using EasyChallenges.Services;
using DLog = EasyChallenges.Common.Logging.Log;
using BepInEx.Unity.IL2CPP;
using SemanticVersioning;

namespace EasyChallenges
{
    [BepInDependency(DependencyGUID: "ModManager", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class EasyChallenges : BasePlugin
    {
        private Version MinimumRequiredGameVersion = new(0, 8, 0);

        public override void Load()
        {
            if (!VersionHelper.IsGameVersionAtLeast(this.MinimumRequiredGameVersion))
            {
                DLog.Error($"Wrong game version! Minimum required game version is {this.MinimumRequiredGameVersion}, you have {VersionHelper.GameVersion}");
                this.Unload();
                return;
            }

            HarmonyPatchHelper.ApplyPatches("EasyChallenges");
            ChallengeLoader.LoadChallenges();
        }

        // public override string ModDescription() => $"Loaded challenges: 999";
    }
}
