## Access Settings
Setting and retrieving settings is as simple as a getter and setter in your code. There are specific methods for each data type that can be passed in:

```csharp
/// <summary>
/// Gets the current value or the default that you specify.
/// </summary>
/// <param name="key">Key for settings</param>
/// <param name="defaultValue">default value if not set</param>
/// <param name="fileName">Name of file for settings to be stored and retrieved </param>
/// <returns>Value or default</returns>
Decimal GetValueOrDefault(string key, Decimal defaultValue, string fileName = null);

/// <summary>
/// Adds or updates the value 
/// </summary>
/// <param name="key">Key for settting</param>
/// <param name="value">Value to set</param>
/// <param name="fileName">Name of file for settings to be stored and retrieved </param>
/// <returns>True of was added or updated and you need to save it.</returns>
bool AddOrUpdateValue(string key, Decimal value, string fileName = null);
```


Example:
```csharp
private static ISettings AppSettings =>
    CrossSettings.Current;

public static string UserName
{
  get => AppSettings.GetValueOrDefault(nameof(UserName),string.Empty); 
  set => AppSettings.AddOrUpdateValue(nameof(UserName), value); 
}
```

When using C# 6 and 7 you can use the nice `nameof` and `=>` accessors to simplify your code.

## Dynamic Settings
You are also able to set the string to anything you want dynamically if you desire:

```csharp
public static bool DidPurchase(string key) =>
    AppSettings.GetValueOrDefault("iap_" + key, false);

public static void SetDidPurchase(string key) =>
    AppSettings.AddOrUpdateValue("iap_" + key, true);
```

## Key autodetection
You can skip passing key directly and determine it from caller context (it can be name of method or property). Also you can skip passing default value, in this case it will be initial value of type. This feature is available as the *SettingsAutoKeyExtensions* class with extension methods in the *Plugin.Settings.Abstractions.Extensions* namespace. There are specific methods for each data type that can be passed in:

```csharp

/// <summary>
/// Gets the current value or the default.
/// </summary>
/// <param name="settings">Settings instance</param>
/// <param name="defaultValue">Default value if not set. By default it's initial value of type</param>
/// <param name="fileName">Name of file for settings to be stored and retrieved </param>
/// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
/// <returns>Value or default</returns>
public static Int32 GetValueOrDefault(this ISettings settings, Int32 defaultValue = default(Int32), string fileName = null, [CallerMemberName] string key = null)
=> settings.GetValueOrDefault(key, defaultValue, fileName);

/// <summary>
/// Adds or updates the value 
/// </summary>
/// <param name="settings">Settings instance</param>
/// <param name="value">Value to set</param>
/// <param name="fileName">Name of file for settings to be stored and retrieved </param>
/// <param name="key">Key for settings. By default it's caller's name (method or property)</param>
/// <returns>True of was added or updated and you need to save it.</returns>
public static bool AddOrUpdateValue(this ISettings settings, Int32 value, string fileName = null, [CallerMemberName] string key = null)
=> settings.AddOrUpdateValue(key, value, fileName);
```

Example:
```csharp
using Plugin.Settings.Abstractions.Extensions;

private static ISettings AppSettings => CrossSettings.Current;

public static bool IsAppInitialized
{
  get => AppSettings.GetValueOrDefault(); 
  set => AppSettings.AddOrUpdateValue(value); 
}

<= Back to [Table of Contents](README.md)

