using System;
using System.Linq;

namespace EasyChallenges.Extensions;

public static class EnumToStringHelper
{
    public static string NamesToString(Type enumToCheck) => string.Join(", ", Enum.GetNames(enumToCheck));
}
