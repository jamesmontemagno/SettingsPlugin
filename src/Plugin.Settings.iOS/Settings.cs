
using System;
using Foundation;
#if __IOS__
using UIKit;
#endif
using Plugin.Settings.Abstractions;

namespace Plugin.Settings
{
    /// <summary>
    /// Main implementation for ISettings
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SettingsImplementation : ISettings
    {

        readonly object locker = new object();

        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="getter">Retrieve value strategy</param>
        /// <returns>Value or default</returns>
        T GetValueOrDefaultInternal<T>(string key, T defaultValue, string fileName, Func<string, NSUserDefaults, T> getter)
        {
            lock (locker)
            {
                var defaults = GetUserDefaults(fileName);

                if (defaults[key] == null)
                    return defaultValue;

                return getter.Invoke(key, defaults);
            }
        }

        decimal OnDecimalRetrieved(string key, NSUserDefaults defaults)
        => decimal.Parse(defaults.StringForKey(key), System.Globalization.CultureInfo.InvariantCulture);

        bool OnBoolRetrieved(string key, NSUserDefaults defaults)
        => defaults.BoolForKey(key);

        int OnIntRetrieved(string key, NSUserDefaults defaults)
        => (int)defaults.IntForKey(key);

        long OnLongRetrieved(string key, NSUserDefaults defaults)
        => long.Parse(defaults.StringForKey(key), System.Globalization.CultureInfo.InvariantCulture);

        double OnDoubleRetrieved(string key, NSUserDefaults defaults)
        => defaults.DoubleForKey(key);

        float OnFloatRetrieved(string key, NSUserDefaults defaults)
        => defaults.FloatForKey(key);

        string OnStringRetrieved(string key, NSUserDefaults defaults)
        => defaults.StringForKey(key);

        Guid OnGuidRetrieved(string key, NSUserDefaults defaults)
        {
            var savedGuid = defaults.StringForKey(key);
            if (string.IsNullOrWhiteSpace(savedGuid))
            {
                return Guid.Empty;
            }
            Guid.TryParse(savedGuid, out Guid outGuid);
            return outGuid;
        }

        DateTime OnDateTimeRetrieved(string key, NSUserDefaults defaults)
        {
            var savedTime = defaults.StringForKey(key);
            if (string.IsNullOrWhiteSpace(savedTime))
            {
                return DateTime.MinValue;
            }

            var ticks = Convert.ToInt64(savedTime, System.Globalization.CultureInfo.InvariantCulture);
            if (ticks >= 0)
            {
                return new DateTime(ticks); //Old value, stored before update to UTC values
            }
            return new DateTime(-ticks, DateTimeKind.Utc); //New value, UTC
        }

        /// <summary>
        /// Adds or updates a reference value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="handler">Store value strategy</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateRefValueInternal<T>(string key, T value, string fileName, Action<string, T, NSUserDefaults> handler) where T : class
        {
            if (value == null)
            {
                Remove(key, fileName);
                return true;
            }

            return AddOrUpdateValueInternal(key, value, fileName, handler);
        }

        /// <summary>
        /// Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <param name="handler">Store value strategy</param>
        /// <returns>True if added or update and you need to save</returns>
        bool AddOrUpdateValueInternal<T>(string key, T value, string fileName, Action<string, T, NSUserDefaults> handler)
        {
            lock (locker)
            {
                try
                {
                    var defaults = GetUserDefaults(fileName);
                    handler.Invoke(key, value, defaults);
                    defaults.Synchronize();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
                }
            }

            return true;
        }

        void OnDecimalSaved(string key, decimal value, NSUserDefaults defaults)
        => defaults.SetString(value.ToString(System.Globalization.CultureInfo.InvariantCulture), key);

        void OnBoolSaved(string key, bool value, NSUserDefaults defaults)
        => defaults.SetBool(value, key);

        void OnIntSaved(string key, int value, NSUserDefaults defaults)
        => defaults.SetInt(value, key);

        void OnLongSaved(string key, long value, NSUserDefaults defaults)
        => defaults.SetString(value.ToString(System.Globalization.CultureInfo.InvariantCulture), key);

        void OnDoubleSaved(string key, double value, NSUserDefaults defaults)
        => defaults.SetDouble(value, key);

        void OnFloatSaved(string key, float value, NSUserDefaults defaults)
        => defaults.SetFloat(value, key);

        void OnStringSaved(string key, string value, NSUserDefaults defaults)
        => defaults.SetString(value, key);

        void OnGuidSaved(string key, Guid value, NSUserDefaults defaults)
        => defaults.SetString(value.ToString(), key);

        void OnDateTimeSaved(string key, DateTime value, NSUserDefaults defaults)
        => defaults.SetString((-value.ToUniversalTime().Ticks).ToString(), key);

        /// <summary>
        /// Removes a desired key from the settings
        /// </summary>
        /// <param name="key">Key for setting</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        public void Remove(string key, string fileName = null)
        {
            lock (locker)
            {
                var defaults = GetUserDefaults(fileName);
                try
                {
                    if (defaults[key] != null)
                    {
                        defaults.RemoveObject(key);
                        defaults.Synchronize();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Clear all keys from settings
        /// </summary>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        public void Clear(string fileName = null)
        {
            lock (locker)
            {
                var defaults = GetUserDefaults(fileName);
                try
                {
                    var items = defaults.ToDictionary();

                    foreach (var item in items.Keys)
                    {
                        if (item is NSString nsString)
                            defaults.RemoveObject(nsString);
                    }
                    defaults.Synchronize();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to clear all defaults. Message: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Checks to see if the key has been added.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True if contains key, else false</returns>
        public bool Contains(string key, string fileName = null)
        {
            lock (locker)
            {
                var defaults = GetUserDefaults(fileName);
                try
                {
                    var setting = defaults[key];
                    return setting != null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to clear all defaults. Message: " + ex.Message);
                }

                return false;
            }
        }

        NSUserDefaults GetUserDefaults(string fileName = null) =>
            string.IsNullOrWhiteSpace(fileName) ?
            NSUserDefaults.StandardUserDefaults :
            new NSUserDefaults(fileName, NSUserDefaultsType.SuiteName);



#region GetValueOrDefault
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public decimal GetValueOrDefault(string key, decimal defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName, OnDecimalRetrieved);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public bool GetValueOrDefault(string key, bool defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName, OnBoolRetrieved);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public long GetValueOrDefault(string key, long defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName, OnLongRetrieved);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public string GetValueOrDefault(string key, string defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName, OnStringRetrieved);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public int GetValueOrDefault(string key, int defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName, OnIntRetrieved);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public float GetValueOrDefault(string key, float defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName, OnFloatRetrieved);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public DateTime GetValueOrDefault(string key, DateTime defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName, OnDateTimeRetrieved);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public Guid GetValueOrDefault(string key, Guid defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName, OnGuidRetrieved);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public double GetValueOrDefault(string key, double defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName, OnDoubleRetrieved);
#endregion

#region AddOrUpdateValue
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, decimal value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName, OnDecimalSaved);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, bool value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName, OnBoolSaved);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, long value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName, OnLongSaved);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, string value, string fileName = null) =>
            AddOrUpdateRefValueInternal(key, value, fileName, OnStringSaved);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, int value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName, OnIntSaved);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, float value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName, OnFloatSaved);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, DateTime value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName, OnDateTimeSaved);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, Guid value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName, OnGuidSaved);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, double value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName, OnDoubleSaved);

        #endregion


        /// <summary>
        /// Attempts to open the app settings page.
        /// </summary>
        /// <returns>true if success, else false and not supported</returns>
        public bool OpenAppSettings()
        {
#if __IOS__
            //Opening settings only open in iOS 8+
            if (!UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                return false;

            try
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                return true;
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }

    }

}
