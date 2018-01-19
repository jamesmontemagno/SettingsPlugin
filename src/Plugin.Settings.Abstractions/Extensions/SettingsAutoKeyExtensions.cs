
using System;
using System.Runtime.CompilerServices;

namespace Plugin.Settings.Abstractions.Extensions
{
    /// <summary>
    /// Auto keys extensions for ISettings
    /// </summary>
    public static class SettingsAutoKeyExtensions
    {
        public static Decimal GetValueOrDefault(this ISettings settings, Decimal defaultValue = default(Decimal), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        public static Boolean GetValueOrDefault(this ISettings settings, Boolean defaultValue = default(Boolean), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        public static Int64 GetValueOrDefault(this ISettings settings, Int64 defaultValue = default(Int64), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        public static String GetValueOrDefault(this ISettings settings, String defaultValue = default(String), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        public static Int32 GetValueOrDefault(this ISettings settings, Int32 defaultValue = default(Int32), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        public static Single GetValueOrDefault(this ISettings settings, Single defaultValue = default(Single), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        public static DateTime GetValueOrDefault(this ISettings settings, DateTime defaultValue = default(DateTime), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        public static Guid GetValueOrDefault(this ISettings settings, Guid defaultValue = default(Guid), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        public static Double GetValueOrDefault(this ISettings settings, Double defaultValue = default(Double), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        public static bool AddOrUpdateValue(this ISettings settings, Decimal value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        public static bool AddOrUpdateValue(this ISettings settings, Boolean value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        public static bool AddOrUpdateValue(this ISettings settings, Int64 value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        public static bool AddOrUpdateValue(this ISettings settings, String value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        public static bool AddOrUpdateValue(this ISettings settings, Int32 value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        public static bool AddOrUpdateValue(this ISettings settings, Single value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        public static bool AddOrUpdateValue(this ISettings settings, DateTime value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        public static bool AddOrUpdateValue(this ISettings settings, Guid value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        public static bool AddOrUpdateValue(this ISettings settings, Double value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        public static void Remove(this ISettings settings, string fileName = null, [CallerMemberName] string key = null)
        => settings.Remove(key, fileName);

        public static void Contains(this ISettings settings, string fileName, [CallerMemberName] string key = null)
        => settings.Contains(key, fileName);
    }
}
