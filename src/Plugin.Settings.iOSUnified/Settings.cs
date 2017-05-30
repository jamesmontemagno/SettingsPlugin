
using System;
using Foundation;
using Plugin.Settings.Abstractions;

namespace Plugin.Settings
{
    /// <summary>
    /// Main implementation for ISettings
    /// </summary>
    [Preserve(AllMembers =true)]
    public class SettingsImplementation : ISettings
    {

        private readonly object locker = new object();

        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>Value or default</returns>
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T), string fileName = null)
        {
            lock (locker)
            {
                var defaults = GetUserDefaults(fileName);

                if (defaults[key] == null)
                    return defaultValue;

                Type typeOf = typeof(T);
                if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    typeOf = Nullable.GetUnderlyingType(typeOf);
                }
                object value = null;
                var typeCode = Type.GetTypeCode(typeOf);
                switch (typeCode)
                {
                    case TypeCode.Decimal:
                        var savedDecimal = defaults.StringForKey(key);
                        value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Boolean:
                        value = defaults.BoolForKey(key);
                        break;
                    case TypeCode.Int64:
                        var savedInt64 = defaults.StringForKey(key);
                        value = Convert.ToInt64(savedInt64, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case TypeCode.Double:
                        value = defaults.DoubleForKey(key);
                        break;
                    case TypeCode.String:
                        value = defaults.StringForKey(key);
                        break;
                    case TypeCode.Int32:
                        value = (Int32)defaults.IntForKey(key);
                        break;
                    case TypeCode.Single:
                        value = (float)defaults.FloatForKey(key);
                        break;

                    case TypeCode.DateTime:
                        var savedTime = defaults.StringForKey(key);
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
                            var savedGuid = defaults.StringForKey(key);
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
        /// Adds or updates a value
        /// </summary>
        /// <param name="key">key to update</param>
        /// <param name="value">value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
        /// <returns>True if added or update and you need to save</returns>
        public bool AddOrUpdateValue<T>(string key, T value, string fileName = null)
        {

            Type typeOf = typeof(T);
            if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
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
                var defaults = GetUserDefaults(fileName);
                switch (typeCode)
                {
                    case TypeCode.Decimal:
                        defaults.SetString(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.Boolean:
                        defaults.SetBool(Convert.ToBoolean(value), key);
                        break;
                    case TypeCode.Int64:
                        defaults.SetString(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.Double:
                        defaults.SetDouble(Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.String:
                        defaults.SetString(Convert.ToString(value), key);
                        break;
                    case TypeCode.Int32:
                        defaults.SetInt(Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.Single:
                        defaults.SetFloat(Convert.ToSingle(value, System.Globalization.CultureInfo.InvariantCulture), key);
                        break;
                    case TypeCode.DateTime:
                        defaults.SetString(Convert.ToString(-(Convert.ToDateTime(value)).ToUniversalTime().Ticks), key);
                        break;
                    default:
                        if (value is Guid)
                        {
                            if (value == null)
                                value = Guid.Empty;

                            defaults.SetString(((Guid)value).ToString(), key);
                        }
                        else
                        {
                            throw new ArgumentException($"Value of type {typeCode} is not supported.");
                        }
                        break;
                }
                try
                {
                    defaults.Synchronize();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
                }
            }


            return true;
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
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
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
                        var nsString = item as NSString;
                        if(nsString != null)
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
        /// <param name="fileName">Name of</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved (iOS = SuiteName, Android = Name, Windows Store/RT8.1/UWP = Container name, WinPhone 8 SL = Doesn't Apply)</param>
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

    }

}
