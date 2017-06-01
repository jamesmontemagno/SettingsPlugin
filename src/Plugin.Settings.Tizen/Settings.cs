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

        /// <summary>
        /// Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply, Tizen = Prefix)</param>
        /// <returns>True if added or update and you need to save</returns>
        public bool AddOrUpdateValue<T>(string key, T value, string fileName = null)
        {
            Type typeOf = typeof(T);
            if (typeOf.GetTypeInfo().IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeOf = Nullable.GetUnderlyingType(typeOf);
            }
            var typeCode = Type.GetTypeCode(typeOf);
            return AddOrUpdateValue(key, value, typeCode, fileName);
        }
        private bool AddOrUpdateValue(string key, object value, TypeCode typeCode, string fileName)
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

        /// <summary>
        /// Clear all keys from settings
        /// </summary>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply, Tizen = Prefix)</param>
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
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply, Tizen = Prefix)</param>
        /// <returns>True if contains key, else false</returns>
        public bool Contains(string key, string fileName = null)
        {
            lock (locker)
            {
                return Preference.Contains(GetFullPrefKey(key, fileName));
            }
        }

        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply, Tizen = Prefix)</param>
        /// <returns>Value or default</returns>
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T), string fileName = null)
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

        /// <summary>
        /// Removes a desired key from the settings
        /// </summary>
        /// <param name="key">Key for setting</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply, Tizen = Prefix)</param>
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
        private string GetFullPrefKey(string key, string fileName = null)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                return key;
            }
            return $"{fileName}_{key}";
        }
    }
}
