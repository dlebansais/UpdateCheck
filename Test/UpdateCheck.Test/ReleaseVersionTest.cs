namespace UpdateCheck.Test;

using NUnit.Framework;

[TestFixture]
public class ReleaseVersionTest
{
    [Test]
    public void CreateEmpty()
    {
        ReleaseVersion TestObject = new();

        Assert.That(TestObject.Major, Is.EqualTo(0));
        Assert.That(TestObject.Minor, Is.EqualTo(0));
        Assert.That(TestObject.Revision, Is.EqualTo(0));
        Assert.That(TestObject.Build, Is.EqualTo(0));
    }

    [Test]
    public void CreateInit()
    {
        ReleaseVersion TestObject = new() { Major = 1, Minor = 2, Revision = 3, Build = 4 };

        Assert.That(TestObject.Major, Is.EqualTo(1));
        Assert.That(TestObject.Minor, Is.EqualTo(2));
        Assert.That(TestObject.Revision, Is.EqualTo(3));
        Assert.That(TestObject.Build, Is.EqualTo(4));
    }

    [Test]
    public void CreateParsing()
    {
        ReleaseVersion TestObject = new("1.2.3.4", out bool parsedSuccessfully);

        Assert.That(parsedSuccessfully, Is.True);
        Assert.That(TestObject.Major, Is.EqualTo(1));
        Assert.That(TestObject.Minor, Is.EqualTo(2));
        Assert.That(TestObject.Revision, Is.EqualTo(3));
        Assert.That(TestObject.Build, Is.EqualTo(4));
    }

    [Test]
    public void CreateParsingFailed()
    {
        ReleaseVersion TestObject = new("1.2.3.x", out bool parsedSuccessfully);

        Assert.That(parsedSuccessfully, Is.False);
        Assert.That(TestObject.Major, Is.EqualTo(0));
        Assert.That(TestObject.Minor, Is.EqualTo(0));
        Assert.That(TestObject.Revision, Is.EqualTo(0));
        Assert.That(TestObject.Build, Is.EqualTo(0));
    }

    [Test]
    public void CompareToSame()
    {
        ReleaseVersion TestObject1 = new("1.2.3.4", out bool parsedSuccessfully1);
        ReleaseVersion TestObject2 = new("1.2.3.4", out bool parsedSuccessfully2);

        Assert.That(parsedSuccessfully1, Is.True);
        Assert.That(parsedSuccessfully2, Is.True);

        Assert.That(TestObject1 == TestObject2, Is.True);
        Assert.That(TestObject1 != TestObject2, Is.False);
        Assert.That(TestObject1 > TestObject2, Is.False);
        Assert.That(TestObject1 < TestObject2, Is.False);
        Assert.That(TestObject1.CompareTo(TestObject2), Is.EqualTo(0));
    }

    [Test]
    public void CompareToOtherMajor()
    {
        ReleaseVersion TestObject1 = new("1.2.3.4", out bool parsedSuccessfully1);
        ReleaseVersion TestObject2 = new("2.0.0.0", out bool parsedSuccessfully2);

        Assert.That(parsedSuccessfully1, Is.True);
        Assert.That(parsedSuccessfully2, Is.True);

        Assert.That(TestObject1 == TestObject2, Is.False);
        Assert.That(TestObject1 != TestObject2, Is.True);
        Assert.That(TestObject1 > TestObject2, Is.False);
        Assert.That(TestObject1 < TestObject2, Is.True);
        Assert.That(TestObject1.CompareTo(TestObject2), Is.EqualTo(1.CompareTo(2)));
        Assert.That(TestObject2.CompareTo(TestObject1), Is.EqualTo(2.CompareTo(1)));
    }

    [Test]
    public void CompareToOtherMinor()
    {
        ReleaseVersion TestObject1 = new("1.2.3.4", out bool parsedSuccessfully1);
        ReleaseVersion TestObject2 = new("1.3.0.0", out bool parsedSuccessfully2);

        Assert.That(parsedSuccessfully1, Is.True);
        Assert.That(parsedSuccessfully2, Is.True);

        Assert.That(TestObject1 == TestObject2, Is.False);
        Assert.That(TestObject1 != TestObject2, Is.True);
        Assert.That(TestObject1 > TestObject2, Is.False);
        Assert.That(TestObject1 < TestObject2, Is.True);
        Assert.That(TestObject1.CompareTo(TestObject2), Is.EqualTo(2.CompareTo(3)));
        Assert.That(TestObject2.CompareTo(TestObject1), Is.EqualTo(3.CompareTo(2)));
    }

    [Test]
    public void CompareToOtherRevision()
    {
        ReleaseVersion TestObject1 = new("1.2.3.4", out bool parsedSuccessfully1);
        ReleaseVersion TestObject2 = new("1.2.4.0", out bool parsedSuccessfully2);

        Assert.That(parsedSuccessfully1, Is.True);
        Assert.That(parsedSuccessfully2, Is.True);

        Assert.That(TestObject1 == TestObject2, Is.False);
        Assert.That(TestObject1 != TestObject2, Is.True);
        Assert.That(TestObject1 > TestObject2, Is.False);
        Assert.That(TestObject1 < TestObject2, Is.True);
        Assert.That(TestObject1.CompareTo(TestObject2), Is.EqualTo(3.CompareTo(4)));
        Assert.That(TestObject2.CompareTo(TestObject1), Is.EqualTo(4.CompareTo(3)));
    }

    [Test]
    public void CompareToOtherBuild()
    {
        ReleaseVersion TestObject1 = new("1.2.3.4", out bool parsedSuccessfully1);
        ReleaseVersion TestObject2 = new("1.2.3.5", out bool parsedSuccessfully2);

        Assert.That(parsedSuccessfully1, Is.True);
        Assert.That(parsedSuccessfully2, Is.True);

        Assert.That(TestObject1 == TestObject2, Is.False);
        Assert.That(TestObject1 != TestObject2, Is.True);
        Assert.That(TestObject1 > TestObject2, Is.False);
        Assert.That(TestObject1 < TestObject2, Is.True);
        Assert.That(TestObject1.CompareTo(TestObject2), Is.EqualTo(4.CompareTo(5)));
        Assert.That(TestObject2.CompareTo(TestObject1), Is.EqualTo(5.CompareTo(4)));
    }
}
