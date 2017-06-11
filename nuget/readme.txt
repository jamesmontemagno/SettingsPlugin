Settings Plugin Readme

See more at: https://github.com/jamesmontemagno/SettingsPlugin/blob/master/CHANGELOG.md

### New in 3.0
* Breaking changes, no longer generic and only takes in specific data types
* Now a .NET Standard Library
* Ability to specify the "file name" of the settings such as SuiteName on iOS
* Support for Mac, tvOS, watchOS, and Tizen!
* Drop support for Windows Phone/Store 8/8.1

### Important
Ensure that you install NuGet into ALL projects.

Create a new file called Settings.cs or whatever you want and copy this code in to get started:


// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace $rootnamespace$.Helpers
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
