namespace EnumComboBox.Test;

using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using NUnit.Framework;
using System;
using System.Threading;
using TestTools;

[TestFixture]
public class UnitTest
{
    private const string DemoAppName = "UpdateCheck.Demo";

    [Test]
    public void TestDefault1()
    {
        DemoApp? DemoApp = DemoApplication.Launch(DemoAppName);
        Assert.That(DemoApp, Is.Not.Null);

        Window MainWindow = DemoApp.MainWindow;
        Assert.That(MainWindow, Is.Not.Null);

        AutomationElement TextElement = MainWindow.FindFirstDescendant(cf => cf.ByText("Version:"));
        Assert.That(TextElement, Is.Not.Null);

        DemoApplication.Stop(DemoApp);
    }
}
