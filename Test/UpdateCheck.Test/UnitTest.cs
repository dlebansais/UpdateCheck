namespace UpdateCheck.Test;

using System;
using System.Threading;
using FlaUI.Core.AutomationElements;
using NUnit.Framework;
using TestTools;

[TestFixture]
[Category("UseOpenCover")]
internal class UnitTest
{
    private const string DemoAppName = "UpdateCheck.Demo";

    [Test]
    public void TestDefault1()
    {
        DemoApp? DemoApp = DemoApplication.Launch(DemoAppName);
        Assert.That(DemoApp, Is.Not.Null);

        Window MainWindow = DemoApp.MainWindow;
        Assert.That(MainWindow, Is.Not.Null);

        AutomationElement? TextElement = MainWindow.FindFirstDescendant(cf => cf.ByText("Version:"));
        Assert.That(TextElement, Is.Not.Null);

        Thread.Sleep(TimeSpan.FromSeconds(10));

        DemoApplication.Stop(DemoApp);
    }
}
