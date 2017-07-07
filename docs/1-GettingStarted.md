# Getting Started

## Setup
* NuGet: [Xam.Plugins.Settings](http://www.nuget.org/packages/Xam.Plugins.Settings) [![NuGet](https://img.shields.io/nuget/v/Xam.Plugins.Settings.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugins.Settings/)
* `PM> Install-Package Xam.Plugins.Settings`
* Install into ALL of your projects, include client projects.

## Where Do Settings Get Saved?
This library uses the native settings management, which means all settings are persisted across app updates, saved natively, and can be integrated into native settings.

* Android: SharedPreferences
* Apple: NSUserDefaults
* UWP: ApplicationDataContainer
* .NET: UserStore -> IsolcatedStorageFile


## What Kind of Data Can I Store?
Since data is stored natively on each platform only certain data types are supported:

* Boolean
* Int32
* Int64
* String
* Single(float)
* Double
* Decimal
* DateTime (Stored and retrieved in UTC)

## Using Settings APIs
It is drop dead simple to gain access to the Settings APIs in any project. All you need to do is get a reference to the current instance of ISettings via `CrossSettings.Current`:

```csharp
private static ISettings AppSettings =>
    CrossSettings.Current;

public static string UserName
{
  get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty); 
  set => AppSettings.AddOrUpdateValue(nameof(UserName), value); 
}
```

There may be instances where you install a plugin into a platform that it isn't supported yet. This means you will have access to the interface, but no implementation exists. You can make a simple check before calling any API to see if it is supported on the platform where the code is running. This if nifty when unit testing:

```csharp
private static ISettings AppSettings
{
    get 
    {
        if(CrossSettings.IsSupported)
            return CrossSettings.Current;
        
        return null; // or your custom implementation 
    }
}
```

## Architecture

### What's with this .Current Global Variable? Why can't I use $FAVORITE_IOC_LIBARY
You totally can! Every plugin I create is based on an interface. The static singleton just gives you a super simple way of gaining access to the platform implementation. Realize that the implementation of the plugin lives in your iOS, Android, Windows, etc. Thies means you will need to register it there by instantiating a `CrossSettingsImplementation` from the platform specific projects.

If you are using a ViewModel/IOC approach your code may look like:

```csharp
public MyViewModel()
{
    readonly ISettings settings;
    public MyViewModel(ISettings settings)
    {
        this.settings = settings;
    }

    public string UserName
    {
    get => settings.GetValueOrDefault(nameof(UserName), string.Empty); 
    set => settings.AddOrUpdateValue(nameof(UserName), value); 
    }
}
```

### What About Unit Testing?
To learn about unit testing strategies be sure to read my blog: [Unit Testing Plugins for Xamarin](http://motzcod.es/post/159267241302/unit-testing-plugins-for-xamarin)


<= Back to [Table of Contents](README.md)