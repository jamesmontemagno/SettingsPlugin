## Removing Settings
When you want to clear out a single setting or all of your settings there are a few options to get you back to factory default.

#### Remove Single Setting
To remove a single setting simply call the `Remove` method:

```csharp
/// <summary>
/// Removes a desired key from the settings
/// </summary>
/// <param name="key">Key for setting</param>
/// <param name="fileName">Name of file for settings to be stored and retrieved </param>
void Remove(string key, string fileName = null);
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

publc static void RemoveUserName() => AppSettings.Remove(nameof(UserName));
```

### Clear All Settings
This should be used with caution as it will remove all of your specific app's settings that were ever created.

```csharp
/// <summary>
/// Clear all keys from settings
/// </summary>
/// <param name="fileName">Name of file for settings to be stored and retrieved </param>
void Clear(string fileName = null);
```

Example:
```csharp
private static ISettings AppSettings =>
    CrossSettings.Current;

public static void ClearEverything()
{
    AppSettings.Clear();
}
```

<= Back to [Table of Contents](README.md)