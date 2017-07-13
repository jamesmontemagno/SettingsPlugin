## Shared Settings

Often you may need to share settings across your app installed on multiple device such as a watch. This is where shared settings and the `fileName` come into play.

## FileName Parameter

Each method takes an additional parameter called `fileName`, which enables the setting to be stored a bit different on each platform.

* iOS: Specifies the [SuiteName](https://developer.xamarin.com/guides/ios/watch/working-with/settings/)
* Android: Specifies the [SharedPreferences Name](http://bit.ly/2tpFrTG)
* UWP: Specifies the [Container Name](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdatacontainer)
* .NET 4.5: Does not apply

All you need to do is pass in a file name for any of the settings:

```csharp
private static ISettings AppSettings =>
    CrossSettings.Current;

private const string WatchFile = "watch";

public static string UserName
{
  get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty, WatchFile); 
  set => AppSettings.AddOrUpdateValue(nameof(UserName), value, WatchFile); 
}
```

<= Back to [Table of Contents](README.md)