// Helpers/Settings.cs
using System;
using System.IO;
using System.Xml.Serialization;
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace SettingsSample.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }
    #region Setting Objects
      /// <summary>
        /// Store object as serialized string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <example>
        ///   AppSettings.Set(new Person {Name="John", Birth=1952});
        ///  </example>
        public static void Set<T> (T value) where T : class, new()
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            AppSettings.AddOrUpdateValue(typeof(T).Name, Serialize(value));
        }

        public static T Get<T> () where T : class, new()
        {
            var key = typeof(T).Name;
            var value = AppSettings.GetValueOrDefault(key, string.Empty);
            return string.IsNullOrEmpty(value) 
                ? default(T) 
                : Deserialize<T>(value);
        }

        private static T Deserialize<T> (this string toDeserialize)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var textReader = new StringReader(toDeserialize);
            return (T)xmlSerializer.Deserialize(textReader);
        }

        private static string Serialize<T> (this T toSerialize)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
    #endregion
    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

    #endregion


    public static string GeneralSettings
    {
      get
      {
        return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
      }
      set
      {
        AppSettings.AddOrUpdateValue(SettingsKey, value);
      }
    }

  }
}
