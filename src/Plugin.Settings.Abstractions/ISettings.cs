
using System;

namespace Plugin.Settings.Abstractions
{
    /// <summary>
    /// Main interface for settings
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        T GetValueOrDefault<T>(string key, T defaultValue = default(T), string fileName = null);


        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        bool AddOrUpdateValue<T>(string key, T value, string fileName = null);

        /// <summary>
        /// Removes a desired key from the settings
        /// </summary>
        /// <param name="key">Key for setting</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        void Remove(string key, string fileName = null);

        /// <summary>
        /// Clear all keys from settings
        /// </summary>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        void Clear(string fileName = null);

        /// <summary>
        /// Checks to see if the key has been added.
        /// </summary>
        /// <param name="key">Key to check</param> 
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True if contains key, else false</returns>
        bool Contains(string key, string fileName = null);
    }
}
