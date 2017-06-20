using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace SettingsSample.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings =>
            CrossSettings.Current;


        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get => AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);

            set => AppSettings.AddOrUpdateValue(SettingsKey, value);

        }

    }
}
