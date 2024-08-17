namespace CodingTracker.Tests;

[TestClass()]
public class CodingSessionTests
{
    private CodingSession _session = new();

    [TestInitialize]
    public void Setup()
    {
        _session = new CodingSession() { StartTime = "2024-08-16 20:00", EndTime = "2024-08-16 22:10" };
    }

    [TestMethod()]
    public void GetStartDateTimeTest()
    {
        DateTime startDateTime = new(2024, 8, 16, 20, 0, 0);
        Assert.AreEqual(startDateTime, _session.GetStartDateTime());
    }

    [TestMethod()]
    public void GetEndDateTimeTest()
    {
        DateTime endDateTime = new(2024, 8, 16, 22, 10, 0);
        Assert.AreEqual(endDateTime, _session.GetEndDateTime());
    }

    [TestMethod()]
    public void CalculateDurationTimeSpanTest()
    {
        TimeSpan duration = new(2, 10, 0);
        Assert.AreEqual(duration, _session.CalculateDurationTimeSpan());
    }

    [TestMethod()]
    public void CalculateDurationMinutesTest()
    {
        long duration = 130;
        Assert.AreEqual(duration, _session.CalculateDurationMinutes());
    }

    [TestMethod()]
    public void SaveDurationTest()
    {
        Assert.AreEqual(0,  _session.Duration);
        _session.SaveDuration();
        Assert.AreEqual(130, _session.Duration);
    }
}