namespace EasyChallenges;

using System.IO;

public static class Paths
{
    public static string EasyChallenges { get; private set; }
    public static string Data { get; private set; }
    public static string BaseModDirectory { get; private set; }

    public static void Initialize(DirectoryInfo modDirectory)
    {
        EasyChallenges = modDirectory.FullName;
        Data = Path.Combine(EasyChallenges, "Data");
        BaseModDirectory = modDirectory.Parent.FullName;
    }
}
