namespace TestUpdateCheck
{
    using System;
    using System.Windows;
    using UpdateCheck;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Checker = new Checker("dlebansais", "UpdateCheck");
            Checker.UpdateStatusChanged += OnUpdateStatusChanged;
            Checker.CheckUpdate();
        }

        private Checker Checker;

        private void OnUpdateStatusChanged(object? sender, EventArgs e)
        {
            bool? IsUpdateAvailable = Checker.IsUpdateAvailable;
            string ReleasePageAddress = Checker.ReleasePageAddress;
        }
    }
}
