using Plugin.Settings.Abstractions;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;

namespace Plugin.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public class SettingsImplementation : ISettings
    {
        static IsolatedStorageFile Store => IsolatedStorageFile.GetUserStoreForDomain();
        

        private readonly object locker = new object();

        /// <summary>
        /// Add or Upate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns></returns>
        bool AddOrUpdateValueInternal<T>(string key, T value, string fileName = null)
        {
            if (value == null)
            {
                Remove(key);

                return true;
            }

            var type = value.GetType();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GenericTypeArguments.FirstOrDefault();
            }


            if ((type == typeof(string)) ||
                (type == typeof(decimal)) ||
                (type == typeof(double)) ||
                (type == typeof(Single)) ||
                (type == typeof(DateTime)) ||
                (type == typeof(Guid)) ||
                (type == typeof(bool)) ||
                (type == typeof(Int32)) ||
                (type == typeof(Int64)) ||
                (type == typeof(byte)))
            {
                lock (locker)
                {
                    string str;

                    if (value is decimal)
                    {
                        return AddOrUpdateValue(key, Convert.ToString(Convert.ToDecimal(value), System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else if (value is DateTime)
                    {
                        return AddOrUpdateValue(key, Convert.ToString(-(Convert.ToDateTime(value)).ToUniversalTime().Ticks, System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else
                        str = Convert.ToString(value);

                    string oldValue = null;

                    if (Store.FileExists(key))
                    {
                        using (var stream = Store.OpenFile(key, FileMode.Open))
                        {
                            using (var sr = new StreamReader(stream))
                            {
                                oldValue = sr.ReadToEnd();
                            }
                        }
                    }

                    using (var stream = Store.OpenFile(key, FileMode.Create, FileAccess.Write))
                    {
                        using (var sw = new StreamWriter(stream))
                        {
                            sw.Write(str);
                        }
                    }

                    return oldValue != str;
                }
            }

            throw new ArgumentException(string.Format("Value of type {0} is not supported.", type.Name));
        }

        /// <summary>
        /// Get Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns></returns>
        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T), string fileName = null)
        {
            object value = null;
            lock (locker)
            {
                string str = null;

                // If the key exists, retrieve the value.
                if (Store.FileExists(key))
                {
                    using (var stream = Store.OpenFile(key, FileMode.Open))
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            str = sr.ReadToEnd();
                        }
                    }
                }

                if (str == null)
                    return defaultValue;

                var type = typeof(T);

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GenericTypeArguments.FirstOrDefault();
                }

                if (type == typeof(string))
                    value = str;

                else if (type == typeof(decimal))
                {
                    
                    string savedDecimal = Convert.ToString(str);
                    

                    value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);

                    return null != value ? (T)value : defaultValue;
                    
                }

                else if (type == typeof(double))
                {
                    value = Convert.ToDouble(str, System.Globalization.CultureInfo.InvariantCulture);
                }

                else if (type == typeof(Single))
                {
                    value = Convert.ToSingle(str, System.Globalization.CultureInfo.InvariantCulture);
                }

                else if (type == typeof(DateTime))
                {
                    
                    var ticks = Convert.ToInt64(str, System.Globalization.CultureInfo.InvariantCulture);
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
                    

                    return (T)value;
                }

                else if (type == typeof(Guid))
                {
                    if (Guid.TryParse(str, out Guid guid))
                        value = guid;
                }

                else if (type == typeof(bool))
                {
                    value = Convert.ToBoolean(str, System.Globalization.CultureInfo.InvariantCulture);
                }

                else if (type == typeof(Int32))
                {
                    value = Convert.ToInt32(str, System.Globalization.CultureInfo.InvariantCulture);
                }

                else if (type == typeof(Int64))
                {
                    value = Convert.ToInt64(str, System.Globalization.CultureInfo.InvariantCulture);
                }

                else if (type == typeof(byte))
                {
                    value = Convert.ToByte(str, System.Globalization.CultureInfo.InvariantCulture);
                }

                else
                {
                    throw new ArgumentException($"Value of type {type} is not supported.");
                }
            }

            return null != value ? (T)value : defaultValue;
        }

        /// <summary>
        /// Remove key
        /// </summary>
        /// <param name="key">Key to remove</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        public void Remove(string key, string fileName = null)
        {
            if (Store.FileExists(key))
                Store.DeleteFile(key);
        }

        /// <summary>
        /// Clear all keys from settings
        /// </summary>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        public void Clear(string fileName = null)
        {
            try
            {
                foreach(var file in Store.GetFileNames())
                {
                    Store.DeleteFile(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to clear all defaults. Message: " + ex.Message);
            }
        }

        /// <summary>
        /// Checks to see if the key has been added.
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True if contains key, else false</returns>
        public bool Contains(string key, string fileName = null) => Store.FileExists(key);





        #region GetValueOrDefault
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public decimal GetValueOrDefault(string key, decimal defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public bool GetValueOrDefault(string key, bool defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public long GetValueOrDefault(string key, long defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public string GetValueOrDefault(string key, string defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public int GetValueOrDefault(string key, int defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public float GetValueOrDefault(string key, float defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public DateTime GetValueOrDefault(string key, DateTime defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>Value or default</returns>
        public Guid GetValueOrDefault(string key, Guid defaultValue, string fileName = null) =>
            GetValueOrDefaultInternal(key, defaultValue, fileName);
        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
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
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, decimal value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, bool value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, long value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, string value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, int value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, float value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, DateTime value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, Guid value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);
        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <param name="fileName">Name of file for settings to be stored and retrieved </param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, double value, string fileName = null) =>
            AddOrUpdateValueInternal(key, value, fileName);

        #endregion
    }
}
