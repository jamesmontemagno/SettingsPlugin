## DataBinding to a Setting
I got a really great question over at StackOverflow as to how to data bind this puppy up for views and there are two approaches. I prefer the first one as it keeps everything all static, but you can of course do it via a singleton too:

Given this Settings Class:
```csharp
public static class Settings 
{

    private static ISettings AppSettings => 
      CrossSettings.Current;

    public static int Count
    {
        get => AppSettings.GetValueOrDefault(nameof(Count), CountDefault); 
        set => AppSettings.AddOrUpdateValue(nameof(Count), value);
    }
}
```

Approach 1: Essentially you need to create a view model with a public property that you wish to data bind to and then call into settings from there and raise a property changed notification if the value changed. Your Settings.cs can stay the same but you will need to create the viewmodel such as:
```csharp
public class MyViewModel : INotifyPropertyChanged
{

    public int Count
    {
        get => Settings.Count; 
        set
        {
            if (Settings.Count == value)
                return;

            Settings.Count = value;
            OnPropertyChanged();
        }
            
    }

    private Command increase;
    public Command IncreaseCommand =>
      increase ?? (increase = new Command(() =>Count++));


    #region INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string name = "") =>
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    #endregion


}
```
Then you XAML will look like this inside your Content page:
```xml
<StackLayout Padding="25">
 <Button Text="Increase" Command="{Binding IncreaseCommand}"/>
 <Label Text="{Binding Count, StringFormat='The count is {0:F0}'}"/>
</StackLayout>
```

Make sure you set the BindingContext in the xaml.cs of the page:
```csharp
public partial class MyPage : ContentPage
{
    public MyPage()
    {
        InitializeComponent();
        BindingContext = new MyViewModel();
    }
}
```

This actually isn't too much code to actually implement as your ViewModel would have a BaseViewModel that implements INotifyProprety changed, so really you are just adding in 

```csharp
public int Count
{
    get => Settings.Count;
    set
    {
        if (Settings.Count == value)
            return;

        Settings.Count = value;
        OnPropertyChanged();
    }
}
```


### Approach 2: More magical way

However, using the powers of C# and knowing how Databinding works you could first create a BaseViewModel that everything will use:

```csharp
public class BaseViewModel : INotifyPropertyChanged
{
    public Settings Settings => Settings.Current;
  

    #region INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string name = "") =>
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    

    #endregion
}
```

Notice my reference to **Settings.Current**, we will need to implement that now as a singleton, but we will use our BaseViewModel so we don't have to re-implement INotifyPropertyChanged:

```csharp
public class Settings : BaseViewModel
{
    static ISettings AppSettings =>
      CrossSettings.Current;

    static Settings settings;
    public static Settings Current =>
      settings ?? (settings = new Settings());

    public int Count
    {
        get => AppSettings.GetValueOrDefault(nameof(Count), CountDefault);
        set
        { 
            if (AppSettings.AddOrUpdateValue(nameof(Count), value))
                OnPropertyChanged();

        }
    }
}
```

Now of course we will still want to create a unique ViewModel that our XAML view will bind to:
```csharp
public class MyViewModel : BaseViewModel
{
    private Command increase;
    public Command IncreaseCommand =>
      increase ?? (increase = new Command(() =>Settings.Count++));
}
```

Notice that we are now inheriting from BaseViewModel, which means our command can actually just increment Settings.Count! But now we must adjust our Xaml just a bit as to what we are actually data binding to for our label:

```xml
<StackLayout Padding="25">
 <Button Text="Increase" Command="{Binding IncreaseCommand}"/>
 <Label BindingContext="{Binding Settings}" Text="{Binding Count, StringFormat='The count is {0:F0}'}"/>
</StackLayout>
```
Notice I am setting the BindingContext to our Settings, which is in our BaseViewModel for the Label, this must be done because that is where it is located now. And there you have it.
<= Back to [Table of Contents](README.md)