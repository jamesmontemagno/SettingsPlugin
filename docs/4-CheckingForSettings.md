## Checking for Settings

Perhaps you want to ensure and not return a default if the user has never set the value. This means you should check to see if it was ever set before.


```csharp
/// <summary>
/// Checks to see if the key has been added.
/// </summary>
/// <param name="key">Key to check</param> 
/// <param name="fileName">Name of file for settings to be stored and retrieved </param>
/// <returns>True if contains key, else false</returns>
bool Contains(string key, string fileName = null);
```

Example:
```csharp
private static ISettings AppSettings =>
    CrossSettings.Current;

static bool IsUserSet => AppSettings.Contains(nameof(UserName));

static string UserName
{
  get => AppSettings.GetValueOrDefault(nameof(UserName),string.Empty); 
  set => AppSettings.AddOrUpdateValue(nameof(UserName), value); 
}
```

<= Back to [Table of Contents](README.md)