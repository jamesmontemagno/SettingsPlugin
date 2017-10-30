## Settings Plugin for Xamarin And Windows

Create and access settings from shared code across all of your apps!

## Documentation
Get started by reading through the [Settings Plugin documentation](https://jamesmontemagno.github.io/SettingsPlugin/).

## NuGet
* [Xam.Plugins.Settings](http://www.nuget.org/packages/Xam.Plugins.Settings) [![NuGet](https://img.shields.io/nuget/v/Xam.Plugins.Settings.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugins.Settings)

## Build: 
* [![Build status](https://ci.appveyor.com/api/projects/status/24dn7jw4it6jbd39?svg=true)](https://ci.appveyor.com/project/JamesMontemagno/settingsplugin)
* CI NuGet Feed: https://ci.appveyor.com/nuget/settingsplugin

**Platform Support**

|Platform|Version|
| ------------------- | :-----------: |
|Xamarin.iOS|iOS 7+|
|Xamarin.Android|API 15+|
|Windows 10 UWP|10+|
|Xamarin.Mac|All|
|Xamarin.tvOS|All|
|Xamarin.watchOS|All|
|Tizen.NET|3.0+|
|.NET 4.5|All|


#### Settings Plugin or Xamarin.Forms App.Properties
I get this question a lot, so here it is from a recent issue opened up. This plugin saves specific properties directly to each platforms native settings APIs (NSUserDefaults, SharedPreferences, etc). This ensures the fastest, most secure, and reliable creation and editing settings per application. Additionally, it works with **any Xamarin application**, not just Xamarin.Forms.

App.Current.Properties actually serializes and deserializes items to disk as you can see in the [implementation](https://github.com/xamarin/Xamarin.Forms/blob/e6d5186c8acbf37b877c7ca3c77a378352a3743d/Xamarin.Forms.Platform.iOS/Deserializer.cs).

To me that isn't as reliable as saving direct to the native platforms settings.

## License
The MIT License (MIT) see [License file](LICENSE)
