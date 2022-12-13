using Il2CppSystem.IO;

namespace EasyChallenges;

public static class Paths
{
    public static string EasyChallenges = Path.Combine(BepInEx.Paths.PluginPath, MyPluginInfo.PLUGIN_NAME);
    public static string Data = Path.Combine(EasyChallenges, "Data");
    public static string Plugins = BepInEx.Paths.PluginPath;
}
