namespace UpdateCheckDemo;

using System;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using UpdateCheck;

[INotifyPropertyChanged]
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        Checker = new Checker("dlebansais", "UpdateCheck");
        Checker.UpdateStatusChanged += OnUpdateStatusChanged;

        _ = Dispatcher.BeginInvoke(async () => await Checker.CheckUpdateAsync());
    }

    private readonly Checker Checker;

    private void OnUpdateStatusChanged(object? sender, EventArgs e)
    {
        IsUpdateAvailable = Checker.IsUpdateAvailable;
        ReleasePageAddress = Checker.ReleasePageAddress;
    }

    [ObservableProperty]
    private bool? _IsUpdateAvailable;

    [ObservableProperty]
    private string _ReleasePageAddress = string.Empty;
}
