
using System;
using System.Runtime.CompilerServices;

namespace Plugin.Settings.Abstractions.Extensions
{
    /// <summary>
    /// Auto keys extensions for ISettings
    /// </summary>
    public static class SettingsAutoKeyExtensions
    {
        /// <summary>
        /// Gets the current value or the default.
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="defaultValue">Default value if not set. By default it's initial value of type</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>Value or default</returns>
        public static Decimal GetValueOrDefault(this ISettings settings, Decimal defaultValue = default(Decimal), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        /// <summary>
        /// Gets the current value or the default.
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="defaultValue">Default value if not set. By default it's initial value of type</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>Value or default</returns>
        public static Boolean GetValueOrDefault(this ISettings settings, Boolean defaultValue = default(Boolean), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        /// <summary>
        /// Gets the current value or the default.
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="defaultValue">Default value if not set. By default it's initial value of type</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>Value or default</returns>
        public static Int64 GetValueOrDefault(this ISettings settings, Int64 defaultValue = default(Int64), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        /// <summary>
        /// Gets the current value or the default.
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="defaultValue">Default value if not set. By default it's initial value of type</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>Value or default</returns>
        public static String GetValueOrDefault(this ISettings settings, String defaultValue = default(String), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        /// <summary>
        /// Gets the current value or the default.
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="defaultValue">Default value if not set. By default it's initial value of type</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>Value or default</returns>
        public static Int32 GetValueOrDefault(this ISettings settings, Int32 defaultValue = default(Int32), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        /// <summary>
        /// Gets the current value or the default.
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="defaultValue">Default value if not set. By default it's initial value of type</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>Value or default</returns>
        public static Single GetValueOrDefault(this ISettings settings, Single defaultValue = default(Single), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        /// <summary>
        /// Gets the current value or the default.
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="defaultValue">Default value if not set. By default it's initial value of type</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>Value or default</returns>
        public static DateTime GetValueOrDefault(this ISettings settings, DateTime defaultValue = default(DateTime), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        /// <summary>
        /// Gets the current value or the default.
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="defaultValue">Default value if not set. By default it's initial value of type</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>Value or default</returns>
        public static Guid GetValueOrDefault(this ISettings settings, Guid defaultValue = default(Guid), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);

        /// <summary>
        /// Gets the current value or the default.
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="defaultValue">Default value if not set. By default it's initial value of type</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>Value or default</returns>
        public static Double GetValueOrDefault(this ISettings settings, Double defaultValue = default(Double), string fileName = null, [CallerMemberName] string key = null)
        => settings.GetValueOrDefault(key, defaultValue, fileName);


        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public static bool AddOrUpdateValue(this ISettings settings, Decimal value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public static bool AddOrUpdateValue(this ISettings settings, Boolean value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public static bool AddOrUpdateValue(this ISettings settings, Int64 value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public static bool AddOrUpdateValue(this ISettings settings, String value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public static bool AddOrUpdateValue(this ISettings settings, Int32 value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public static bool AddOrUpdateValue(this ISettings settings, Single value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public static bool AddOrUpdateValue(this ISettings settings, DateTime value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public static bool AddOrUpdateValue(this ISettings settings, Guid value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public static bool AddOrUpdateValue(this ISettings settings, Double value, string fileName = null, [CallerMemberName] string key = null)
        => settings.AddOrUpdateValue(key, value, fileName);

        /// <summary>
        /// Removes a record from settings by key
        /// </summary>
        /// <param name="settings">Settings instance</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key for removing. By default it's caller's name (method or property)</param>
        public static void Remove(this ISettings settings, string fileName = null, [CallerMemberName] string key = null)
        => settings.Remove(key, fileName);

        /// <summary>
        /// Checks to see if the key has been added.
        /// </summary> 
        /// <param name="settings">Settings instance</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="key">Key to check. By default it's caller's name (method or property)</param>
        /// <returns>True if contains key, else false</returns>
        public static void Contains(this ISettings settings, string fileName, [CallerMemberName] string key = null)
        => settings.Contains(key, fileName);
    }
}
