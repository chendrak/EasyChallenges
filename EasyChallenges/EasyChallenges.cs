using EasyChallenges.Services;
using DLog = EasyChallenges.Common.Logging.Log;

namespace EasyChallenges
{
    using System.Linq;
    using ModGenesia;

    public class EasyChallenges : RogueGenesiaMod
    {
        public const string MOD_NAME = "EasyChallenges";

        public EasyChallenges()
        {
            DLog.Initialize(MOD_NAME);
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
    }
}
