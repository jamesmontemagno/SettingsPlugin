# Settings Plugin for Xamarin And Windows

Create and access settings from shared code across all of your apps!

## Documentation
Get started by reading through the [Settings Plugin documentation](https://jamesmontemagno.github.io/SettingsPlugin/).

Looking to store credentials and sensitive information? Use Xamarin.Essential's [Secure Storage ](https://docs.microsoft.com/xamarin/essentials/secure-storage)

## NuGet
* [Xam.Plugins.Settings](http://www.nuget.org/packages/Xam.Plugins.Settings) [![NuGet](https://img.shields.io/nuget/v/Xam.Plugins.Settings.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugins.Settings)

### The Future: [Xamarin.Essentials](https://docs.microsoft.com/xamarin/essentials/index?WT.mc_id=docs-github-jamont)

I have been working on Plugins for Xamarin for a long time now. Through the years I have always wanted to create a single, optimized, and official package from the Xamarin team at Microsoft that could easily be consumed by any application. The time is now with [Xamarin.Essentials](https://docs.microsoft.com/xamarin/essentials/index?WT.mc_id=docs-github-jamont), which offers over 50 cross-platform native APIs in a single optimized package. I worked on this new library with an amazing team of developers and I highly highly highly recommend you check it out.

I will continue to work and maintain my Plugins, but I do recommend you checkout Xamarin.Essentials to see if it is a great fit your app as it has been for all of mine!

## Build: 
* ![Build status](https://jamesmontemagno.visualstudio.com/_apis/public/build/definitions/6b79a378-ddd6-4e31-98ac-a12fcd68644c/14/badge)
* CI NuGet Feed: http://myget.org/F/xamarin-plugins

**Platform Support**

|Platform|Version|
| ------------------- | :-----------: |
|Xamarin.iOS|iOS 7+|
|Xamarin.Android|API 15+|
|Windows 10 UWP|10+|
|Xamarin.Mac|All|
|Xamarin.tvOS|All|
|Xamarin.watchOS|All|
|.NET|4.5+|
|.NET Core|2.0+|
|Tizen|4.0+|


#### Settings Plugin or Xamarin.Forms App.Properties
I get this question a lot, so here it is from a recent issue opened up. This plugin saves specific properties directly to each platforms native settings APIs (NSUserDefaults, SharedPreferences, etc). This ensures the fastest, most secure, and reliable creation and editing settings per application. Additionally, it works with **any Xamarin application**, not just Xamarin.Forms.

App.Current.Properties actually serializes and deserializes items to disk as you can see in the [implementation](https://github.com/xamarin/Xamarin.Forms/blob/e6d5186c8acbf37b877c7ca3c77a378352a3743d/Xamarin.Forms.Platform.iOS/Deserializer.cs).

To me that isn't as reliable as saving direct to the native platforms settings.

# Contribution

Thanks you for your interest in contributing to Settings plugin! In this section we'll outline what you need to know about contributing and how to get started.

### Bug Fixes
Please browse open issues, if you're looking to fix something, it's possible that someone already reported it. Additionally you select any `up-for-grabs` items

### Pull requests
Please fill out the pull request template when you send one.
Run tests to make sure your changes don't break any unit tests. Follow these instructions to run tests - 

**iOS**
- Navigate to _tests/Plugin.Settings.NUnitTest.iOS_
- Execute `make run-simulator-tests`

**Android**

Execute `./build.sh --target RunDroidTests` from the project root

## License
The MIT License (MIT) see [License file](LICENSE)

### Want To Support This Project?
All I have ever asked is to be active by submitting bugs, features, and sending those pull requests down! Want to go further? Make sure to subscribe to my weekly development podcast [Merge Conflict](http://mergeconflict.fm), where I talk all about awesome Xamarin goodies and you can optionally support the show by becoming a [supporter on Patreon](https://www.patreon.com/mergeconflictfm).
