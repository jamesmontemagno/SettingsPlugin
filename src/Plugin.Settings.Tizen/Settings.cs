using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tizen.Applications;

namespace Plugin.Settings
{
    /// <summary>
    /// Main Implementation for ISettings
    /// </summary>
    public class SettingsImplementation : ISettings
    {
        private readonly object locker = new object();
        #region Internal

        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T), string fileName = null)
        {
            lock (locker)
            {
                var prefkey = GetFullPrefKey(key, fileName);
                if (!Preference.Contains(prefkey))
                    return defaultValue;

                Type typeOf = typeof(T);
                if (typeOf.GetTypeInfo().IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    typeOf = Nullable.GetUnderlyingType(typeOf);
                }
                object value = null;
                var typeCode = Type.GetTypeCode(typeOf);
                switch (typeCode)
                {
                    case TypeCode.Decimal:
                        var savedDecimal = Preference.Get<string>(prefkey);
                        value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Boolean:
                        value = Preference.Get<bool>(prefkey);
                        break;
                    case TypeCode.Int64:
                        var savedInt64 = Preference.Get<string>(prefkey);
                        value = Convert.ToInt64(savedInt64, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Double:
                        value = Preference.Get<double>(prefkey);
                        break;
                    case TypeCode.String:
                        value = Preference.Get<string>(prefkey);
                        break;
                    case TypeCode.Int32:
                        value = Preference.Get<int>(prefkey);
                        break;
                    case TypeCode.Single:
                        var savedSingle = Preference.Get<string>(prefkey);
                        value = Convert.ToSingle(savedSingle, System.Globalization.CultureInfo.InvariantCulture);
                        break;

                    case TypeCode.DateTime:
                        var savedTime = Preference.Get<string>(prefkey);
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
                        break;
                    default:

                        if (defaultValue is Guid)
                        {
                            var outGuid = Guid.Empty;
                            var savedGuid = Preference.Get<string>(prefkey);
                            if (string.IsNullOrWhiteSpace(savedGuid))
                            {
                                value = outGuid;
                            }
                            else
                            {
                                Guid.TryParse(savedGuid, out outGuid);
                                value = outGuid;
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"Value of type {typeCode} is not supported.");
                        }

                        break;
                }


                return null != value ? (T)value : defaultValue;
            }
        }
      
        bool AddOrUpdateValueInternal<T>(string key, T value, string fileName = null)
        {
            Type typeOf = typeof(T);
            if (typeOf.GetTypeInfo().IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeOf = Nullable.GetUnderlyingType(typeOf);
            }
            var typeCode = Type.GetTypeCode(typeOf);
            return AddOrUpdateValueCore(key, value, typeCode, fileName);
        }
        bool AddOrUpdateValueCore(string key, object value, TypeCode typeCode, string fileName)
        {
            lock (locker)
            {
                var prefKey = GetFullPrefKey(key, fileName);
                switch (typeCode)
                {
                    case TypeCode.Decimal:
                        Preference.Set(prefKey, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.Boolean:
                        Preference.Set(prefKey, Convert.ToBoolean(value));
                        break;
                    case TypeCode.Int64:
                        Preference.Set(prefKey, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.Double:
                        Preference.Set(prefKey, Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.String:
                        Preference.Set(prefKey, Convert.ToString(value));
                        break;
                    case TypeCode.Int32:
                        Preference.Set(prefKey, Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.Single:
                        Preference.Set(prefKey, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
                        break;
                    case TypeCode.DateTime:
                        Preference.Set(Convert.ToString(-(Convert.ToDateTime(value)).ToUniversalTime().Ticks), prefKey);
                        break;
                    default:
                        if (value is Guid)
                        {
                            if (value == null)
                                value = Guid.Empty;

                            Preference.Set(((Guid)value).ToString(), prefKey);
                        }
                        else
                        {
                            throw new ArgumentException($"Value of type {typeCode} is not supported.");
                        }
                        break;
                }
            }
            return true;
        }

        string GetFullPrefKey(string key, string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return key;
            }
            return $"{fileName}_{key}";
        }
        #endregion


        /// <summary>
        /// Clear all keys from settings
        /// </summary>
        /// <param name="fileName">Prefix for the key</param>
        public void Clear(string fileName = null)
        {
            lock (locker)
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    Preference.RemoveAll();
                }
                else
                {
                    var keys = Preference.Keys.Where(key => key.StartsWith($"{fileName}_")).ToList();
                    keys.ForEach(Preference.Remove);
                }
            }
        }

        /// <summary>
        /// Checks to see if the key has been added.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>True if contains key, else false</returns>
        public bool Contains(string key, string fileName = null)
        {
            lock (locker)
            {
                return Preference.Contains(GetFullPrefKey(key, fileName));
            }
        }



        /// <summary>
        /// Removes a desired key from the settings
        /// </summary>
        /// <param name="key">Key for setting</param>
        /// <param name="fileName">Prefix for the key</param>
        public void Remove(string key, string fileName = null)
        {
            lock (locker)
            {
                var prefkey = GetFullPrefKey(key, fileName);
                if (Preference.Contains(prefkey))
                {
                    Preference.Remove(prefkey);
                }
            }
        }

        #region GetValueOrDefault
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>Value or default</returns>
        public decimal GetValueOrDefault(string key, decimal defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>Value or default</returns>
        public bool GetValueOrDefault(string key, bool defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>Value or default</returns>
        public long GetValueOrDefault(string key, long defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>Value or default</returns>
        public string GetValueOrDefault(string key, string defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>Value or default</returns>
        public int GetValueOrDefault(string key, int defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>Value or default</returns>
        public float GetValueOrDefault(string key, float defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>Value or default</returns>
        public DateTime GetValueOrDefault(string key, DateTime defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>Value or default</returns>
        public Guid GetValueOrDefault(string key, Guid defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Prefix for the key</param>
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
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, decimal value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, bool value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, long value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, string value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, int value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, float value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, DateTime value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, Guid value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Prefix for the key</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, double value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);

        #endregion
    }
}
