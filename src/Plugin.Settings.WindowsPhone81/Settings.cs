using Windows.Storage;
using Plugin.Settings.Abstractions;
using System;
using System.Diagnostics;

namespace Plugin.Settings
{
    /// <summary>
    /// Main ISettings Implementation
    /// </summary>
    public class SettingsImplementation : ISettings
    {
        ApplicationDataContainer GetAppSettings(string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return ApplicationData.Current.LocalSettings;

            if (!ApplicationData.Current.LocalSettings.Containers.ContainsKey(fileName))
                ApplicationData.Current.LocalSettings.CreateContainer(fileName, ApplicationDataCreateDisposition.Always);

            return ApplicationData.Current.LocalSettings.Containers[fileName];
        }

        readonly object locker = new object();

        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T), string fileName = null)
        {
            

            object value;
            lock (locker)
            {
                var settings = GetAppSettings(fileName);

                if (typeof(T) == typeof(decimal))
                {
                    string savedDecimal;
                    // If the key exists, retrieve the value.
                    if (settings.Values.ContainsKey(key))
                    {
                        savedDecimal = Convert.ToString(settings.Values[key]);
                    }
                    // Otherwise, use the default value.
                    else
                    {
                        savedDecimal = defaultValue == null ? default(decimal).ToString() : defaultValue.ToString();
                    }

                    value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);

                    return null != value ? (T)value : defaultValue;
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    string savedTime = null;
                    // If the key exists, retrieve the value.
                    if (settings.Values.ContainsKey(key))
                    {
                        savedTime = Convert.ToString(settings.Values[key]);
                    }

                    if (string.IsNullOrWhiteSpace(savedTime))
                    {
                        value = defaultValue;
                    }
                    else
                    {
                        var ticks = Convert.ToInt64(savedTime, System.Globalization.CultureInfo.InvariantCulture);
                        if (ticks >= 0)
                        {
                            //Old value, stored before update to UTC values
                            value = new DateTime(ticks);
                        }
                        else
                        {
                            //New value, UTC
                            value = new DateTime(-ticks, DateTimeKind.Utc);
                        }
                    }

                    return (T)value;
                }

                // If the key exists, retrieve the value.
                if (settings.Values.ContainsKey(key))
                {
                    var tempValue = settings.Values[key];
                    if (tempValue != null)
                        value = (T)tempValue;
                    else
                        value = defaultValue;
                }
                // Otherwise, use the default value.
                else
                {
                    value = defaultValue;
                }
            }

            return null != value ? (T)value : defaultValue;
        }

        /// <summary>
        /// Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValueInternal<T>(string key, T value, string fileName = null)
        {
            if (value == null)
            {
                Remove(key, fileName);
                return true;
            }

            return AddOrUpdateValueCore(key, value, fileName);
        }
        

        bool AddOrUpdateValueCore(string key, object value, string fileName)
        {
            bool valueChanged = false;
            lock (locker)
            {
                var settings = GetAppSettings(fileName);
                if (value is decimal)
                {
                    return AddOrUpdateValueInternal(key, Convert.ToString(Convert.ToDecimal(value), System.Globalization.CultureInfo.InvariantCulture), fileName);
                }
                else if (value is DateTime)
                {
                    return AddOrUpdateValueInternal(key, Convert.ToString(-(Convert.ToDateTime(value)).ToUniversalTime().Ticks, System.Globalization.CultureInfo.InvariantCulture), fileName);
                }


                // If the key exists
                if (settings.Values.ContainsKey(key))
                {

                    // If the value has changed
                    if (settings.Values[key] != value)
                    {
                        // Store key new value
                        settings.Values[key] = value;
                        valueChanged = true;
                    }
                }
                // Otherwise create the key.
                else
                {
                    //settings.CreateContainer(key, ApplicationDataCreateDisposition.Always);
                    settings.Values[key] = value;
                    valueChanged = true;
                }
            }

            return valueChanged;
        }

        /// <summary>
        /// Removes a desired key from the settings
        /// </summary>
        /// <param name="key">Key for setting</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        public void Remove(string key, string fileName = null)
        {
            lock (locker)
            {
                var settings = GetAppSettings(fileName);
                // If the key exists remove
                if (settings.Values.ContainsKey(key))
                {
                    settings.Values.Remove(key);
                }
            }
        }

        /// <summary>
        /// Clear all keys from settings
        /// </summary>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        public void Clear(string fileName = null)
        {
            lock (locker)
            {
                try
                {
                    var settings = GetAppSettings(fileName);
                    settings.Values.Clear();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to clear all defaults. Message: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Checks to see if the key has been added.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True if contains key, else false</returns>
        public bool Contains(string key, string fileName = null)
        {
            lock (locker)
            {
                try
                {
                    var settings = GetAppSettings(fileName);
                    return settings.Values.ContainsKey(key);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to check " + key + " Message: " + ex.Message);
                }

                return false;
            }
        }


        #region GetValueOrDefault
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        public decimal GetValueOrDefault(string key, decimal defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        public bool GetValueOrDefault(string key, bool defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        public long GetValueOrDefault(string key, long defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        public string GetValueOrDefault(string key, string defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        public int GetValueOrDefault(string key, int defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        public float GetValueOrDefault(string key, float defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        public DateTime GetValueOrDefault(string key, DateTime defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        public Guid GetValueOrDefault(string key, Guid defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        public double GetValueOrDefault(string key, double defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        #endregion

        #region AddOrUpdateValue
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, decimal value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, bool value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, long value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, string value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, int value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, float value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, DateTime value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, Guid value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, double value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);

        #endregion
    }
}
