using EasyChallenges.Services;

namespace EasyChallenges
{
    using System.Linq;
    using Common.Logging;
    using Helpers;
    using ModGenesia;

    public class EasyChallenges : RogueGenesiaMod
    {
        public const string MOD_NAME = "EasyChallenges";

        public EasyChallenges()
        {
            ModOptionHelper.RegisterModOptions();
            var logLevel = ModOptionHelper.AreDebugLogsEnabled() ? Log.LogLevel.DEBUG : Log.LogLevel.INFO;
            Log.Initialize(MOD_NAME);
            Log.SetMinimumLogLevel(logLevel);
        }

        public override void OnModLoaded(ModData modData)
        {
            // This needs to be the first line, because a bunch of stuff relies on the paths being initialized
            Paths.Initialize(modData.ModDirectory);
        }

        public override void OnRegisterModdedContent()
        {
            var modPaths = ModLoader.EnabledMods.Select(mod => mod.ModDirectory.FullName).ToList();
            ChallengeEventHandler.Initialize();
            ChallengeLoader.Initialize(modPaths);
        }

        public override void OnModUnloaded()
        {
            Log.Debug("OnModUnloaded");
            ChallengeEventHandler.Cleanup();
            ChallengeLoader.Cleanup();
        }
    }
}
