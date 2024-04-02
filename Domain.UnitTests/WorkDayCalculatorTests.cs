using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkCalendar.Domain;

namespace Domain.UnitTests;

[TestClass]
public class WorkDayCalculatorTests
{
    private BusinessHours _businessHours;
    private WorkdayCalculator _calculator;
    private double _daysToAdd;

    private HashSet<Holiday> _holidays;
    private Day _startDay;
    private Day _targetResult;

    [TestInitialize]
    public void Init()
    {
        _holidays = new HashSet<Holiday>
        {
            new(new DateTime(2000, 5, 17), true),
            new(new DateTime(2004, 5, 27), false)
        };
        _businessHours = new BusinessHours(new TimeSpan(8, 0, 0), new TimeSpan(16, 0, 0));
    }

    /// <summary>
    ///     Test that covers the following scenario: Start date 24.05.2004 08:00 and add 2 days should result in 26.05.2004
    ///     08:00
    /// </summary>
    [TestMethod]
    public void Handle_VanillaCase()
    {
        // arrange
        _daysToAdd = 2;
        _startDay = new Day(new DateTimeOffset(2004, 5, 24, 08, 0, 0, new TimeSpan()));
        _calculator = new WorkdayCalculator(_businessHours, _holidays);
        _targetResult = new Day(new DateTimeOffset(2004, 5, 26, 8, 0, 0, new TimeSpan()));
        // act
        var resultDay = _calculator.CalculateWorkingDays(_startDay, _daysToAdd);

        // assert
        Assert.AreEqual(_targetResult.Date.Year, resultDay.Date.Year);
        Assert.AreEqual(_targetResult.Date.Month, resultDay.Date.Month);
        Assert.AreEqual(_targetResult.Date.Day, resultDay.Date.Day);
        Assert.AreEqual(_targetResult.Date.Hour, resultDay.Date.Hour);
        Assert.AreEqual(_targetResult.Date.Minute, resultDay.Date.Minute);
    }


    /// <summary>
    ///     Test that covers the following scenario: Start date 24.05.2004 18:05 and add -5.5 days should result in 14.05.2004
    ///     12:00
    /// </summary>
    [TestMethod]
    public void Handle_Minus5Point5()
    {
        // arrange
        _daysToAdd = -5.5;
        _startDay = new Day(new DateTimeOffset(2004, 5, 24, 18, 5, 0, new TimeSpan()));
        _calculator = new WorkdayCalculator(_businessHours, _holidays);
        _targetResult = new Day(new DateTimeOffset(2004, 5, 14, 12, 0, 0, new TimeSpan()));
        // act
        var resultDay = _calculator.CalculateWorkingDays(_startDay, _daysToAdd);

        // assert
        Assert.AreEqual(_targetResult.Date.Year, resultDay.Date.Year);
        Assert.AreEqual(_targetResult.Date.Month, resultDay.Date.Month);
        Assert.AreEqual(_targetResult.Date.Day, resultDay.Date.Day);
        Assert.AreEqual(_targetResult.Date.Hour, resultDay.Date.Hour);
        Assert.AreEqual(_targetResult.Date.Minute, resultDay.Date.Minute);
    }

    /// <summary>
    ///     Test that covers the following scenario: Start date 24-05-2004 19:03 and add 44.723656 days should result in
    ///     27-07-2004 13:47
    /// </summary>
    [TestMethod]
    public void Handle_Plus44_AndSomeChange()
    {
        // arrange
        _daysToAdd = 44.723656;
        _startDay = new Day(new DateTimeOffset(2004, 5, 24, 19, 3, 0, new TimeSpan()));
        _calculator = new WorkdayCalculator(_businessHours, _holidays);
        _targetResult = new Day(new DateTimeOffset(2004, 7, 27, 13, 47, 0, new TimeSpan()));
        // act
        var resultDay = _calculator.CalculateWorkingDays(_startDay, _daysToAdd);

        // assert
        Assert.AreEqual(_targetResult.Date.Year, resultDay.Date.Year);
        Assert.AreEqual(_targetResult.Date.Month, resultDay.Date.Month);
        Assert.AreEqual(_targetResult.Date.Day, resultDay.Date.Day);
        Assert.AreEqual(_targetResult.Date.Hour, resultDay.Date.Hour);
        Assert.AreEqual(_targetResult.Date.Minute, resultDay.Date.Minute);
    }

    /// <summary>
    ///     Test that covers the following scenario: Start date 24-05-2004 18:03 and add -6.7470217 days should result in
    ///     13-05-2004 10:02
    ///     TODO: This test fail, and I suspect it is rounding issues. The result become 10:01:25, while the case description states the result should be 10:02. Point of discussion..? :)
    ///     TODO: After further investigations it seems like there might be an error in the case description:
    ///     If we take the length of day * remainder: 8 *  -0.7470217 = -5.9761736 hrs.
    ///     If we then add that to the end of the day without converting to actual time:
    ///     16 - 5.9761736 = 10.0238264 (which i guess might be where the 10:02 comes from?).
    ///     While if we convert -0.9761736 into minutes 60 * -0.9761736 = -58.570416 minutes
    ///     So, if we subtract 5 hours 58 minutes and 34 seconds (.570416 * 60)  from 16:00 we get 10:01:26
    /// </summary>
    [TestMethod]
    public void Handle_MinusSix_AndSomeChange()
    {
        // arrange
        _daysToAdd = -6.7470217;
        _startDay = new Day(new DateTimeOffset(2004, 05, 24, 18, 3, 0, new TimeSpan()));
        _calculator = new WorkdayCalculator(_businessHours, _holidays);
        _targetResult = new Day(new DateTimeOffset(2004, 05, 13, 10, 2, 0, new TimeSpan()));

        // act
        var resultDay = _calculator.CalculateWorkingDays(_startDay, _daysToAdd);

        // assert
        Assert.AreEqual(_targetResult.Date.Year, resultDay.Date.Year);
        Assert.AreEqual(_targetResult.Date.Month, resultDay.Date.Month);
        Assert.AreEqual(_targetResult.Date.Day, resultDay.Date.Day);
        Assert.AreEqual(_targetResult.Date.Hour, resultDay.Date.Hour);
        Assert.AreEqual(_targetResult.Date.Minute, resultDay.Date.Minute);
    }

    /// <summary>
    ///     Test that covers the following scenario: Start date 24-05-2004 08:03 and add 12.782709 days should result in
    ///     10-06-2004 14:18
    /// </summary>
    [TestMethod]
    public void Handle_PlusTwelve_AndSomeChange()
    {
        // arrange
        _daysToAdd = 12.782709;
        _startDay = new Day(new DateTimeOffset(2004, 05, 24, 8, 3, 0, new TimeSpan()));
        _calculator = new WorkdayCalculator(_businessHours, _holidays);
        _targetResult = new Day(new DateTimeOffset(2004, 6, 10, 14, 18, 0, new TimeSpan()));
        // act
        var resultDay = _calculator.CalculateWorkingDays(_startDay, _daysToAdd);

        // assert
        Assert.AreEqual(_targetResult.Date.Year, resultDay.Date.Year);
        Assert.AreEqual(_targetResult.Date.Month, resultDay.Date.Month);
        Assert.AreEqual(_targetResult.Date.Day, resultDay.Date.Day);
        Assert.AreEqual(_targetResult.Date.Hour, resultDay.Date.Hour);
        Assert.AreEqual(_targetResult.Date.Minute, resultDay.Date.Minute);
    }

    /// <summary>
    ///     Test that covers the following scenario: Start date 24-05-2004 07:03 and add 8.276628 days should result in
    ///     04-06-2004 10:12
    /// </summary>
    [TestMethod]
    public void Handle_PlusEight_AndSomeChange()
    {
        // arrange
        _daysToAdd = 8.276628;
        _startDay = new Day(new DateTimeOffset(2004, 05, 24, 7, 3, 0, new TimeSpan()));
        _calculator = new WorkdayCalculator(_businessHours, _holidays);
        _targetResult = new Day(new DateTimeOffset(2004, 6, 4, 10, 12, 0, new TimeSpan()));
        // act
        var resultDay = _calculator.CalculateWorkingDays(_startDay, _daysToAdd);

        // assert
        Assert.AreEqual(_targetResult.Date.Year, resultDay.Date.Year);
        Assert.AreEqual(_targetResult.Date.Month, resultDay.Date.Month);
        Assert.AreEqual(_targetResult.Date.Day, resultDay.Date.Day);
        Assert.AreEqual(_targetResult.Date.Hour, resultDay.Date.Hour);
        Assert.AreEqual(_targetResult.Date.Minute, resultDay.Date.Minute);
    }

    /// <summary>
    ///     Test that covers the following scenario: Start date 1-03-2024 08:00 and add 5.5 days should result in 11-03-2024
    ///     12:00
    /// </summary>
    [TestMethod]
    public void Handle_Plus5Point5()
    {
        // arrange
        _daysToAdd = 5.5;
        _holidays = new HashSet<Holiday>
        {
            new(new DateTime(2024, 3, 8), false)
        };
        _startDay = new Day(new DateTimeOffset(2024, 03, 1, 8, 0, 0, new TimeSpan()));
        _calculator = new WorkdayCalculator(_businessHours, _holidays);
        _targetResult = new Day(new DateTimeOffset(2024, 3, 11, 12, 0, 0, new TimeSpan()));
        // act
        var resultDay = _calculator.CalculateWorkingDays(_startDay, _daysToAdd);

        // assert
        Assert.AreEqual(_targetResult.Date.Year, resultDay.Date.Year);
        Assert.AreEqual(_targetResult.Date.Month, resultDay.Date.Month);
        Assert.AreEqual(_targetResult.Date.Day, resultDay.Date.Day);
        Assert.AreEqual(_targetResult.Date.Hour, resultDay.Date.Hour);
        Assert.AreEqual(_targetResult.Date.Minute, resultDay.Date.Minute);
    }

    /// <summary>
    ///     Test that covers the following scenario: Start date 26-03-2024 15:07 and add 0.25 days should result in 27-03-2024
    ///     09:07
    /// </summary>
    [TestMethod]
    public void Handle_PointTwoFive()
    {
        // arrange
        _daysToAdd = 0.25;
        _holidays = new HashSet<Holiday>();
        _startDay = new Day(new DateTimeOffset(2024, 03, 26, 15, 7, 0, new TimeSpan()));
        _calculator = new WorkdayCalculator(_businessHours, _holidays);
        _targetResult = new Day(new DateTimeOffset(2024, 3, 27, 9, 7, 0, new TimeSpan()));
        // act
        var resultDay = _calculator.CalculateWorkingDays(_startDay, _daysToAdd);

        // assert
        Assert.AreEqual(_targetResult.Date.Year, resultDay.Date.Year);
        Assert.AreEqual(_targetResult.Date.Month, resultDay.Date.Month);
        Assert.AreEqual(_targetResult.Date.Day, resultDay.Date.Day);
        Assert.AreEqual(_targetResult.Date.Hour, resultDay.Date.Hour);
        Assert.AreEqual(_targetResult.Date.Minute, resultDay.Date.Minute);
    }

    /// <summary>
    ///     Test that covers the following scenario: Start date 26-03-2024 08:43 and add -0.25 days should result in 25-03-2024
    ///     14:43
    /// </summary>
    [TestMethod]
    public void Handle_Minus_PointTwoFive()
    {
        // arrange
        _daysToAdd = -0.25;
        _holidays = new HashSet<Holiday>();
        _startDay = new Day(new DateTimeOffset(2024, 03, 26, 8, 43, 0, new TimeSpan()));
        _calculator = new WorkdayCalculator(_businessHours, _holidays);
        _targetResult = new Day(new DateTimeOffset(2024, 3, 25, 14, 43, 0, new TimeSpan()));
        // act
        var resultDay = _calculator.CalculateWorkingDays(_startDay, _daysToAdd);

        // assert
        Assert.AreEqual(_targetResult.Date.Year, resultDay.Date.Year);
        Assert.AreEqual(_targetResult.Date.Month, resultDay.Date.Month);
        Assert.AreEqual(_targetResult.Date.Day, resultDay.Date.Day);
        Assert.AreEqual(_targetResult.Date.Hour, resultDay.Date.Hour);
        Assert.AreEqual(_targetResult.Date.Minute, resultDay.Date.Minute);
    }


    /// <summary>
    ///     Test that covers the following scenario: Start date 26-03-2024 04:00 and add 0.5 days should result in 26-03-2024
    ///     12:00
    /// </summary>
    [TestMethod]
    public void Handle_PointFive()
    {
        // arrange
        _daysToAdd = 0.5;
        _holidays = new HashSet<Holiday>();
        _startDay = new Day(new DateTimeOffset(2024, 03, 26, 4, 0, 0, new TimeSpan()));
        _calculator = new WorkdayCalculator(_businessHours, _holidays);
        _targetResult = new Day(new DateTimeOffset(2024, 3, 26, 12, 0, 0, new TimeSpan()));
        // act
        var resultDay = _calculator.CalculateWorkingDays(_startDay, _daysToAdd);

        // assert
        Assert.AreEqual(_targetResult.Date.Year, resultDay.Date.Year);
        Assert.AreEqual(_targetResult.Date.Month, resultDay.Date.Month);
        Assert.AreEqual(_targetResult.Date.Day, resultDay.Date.Day);
        Assert.AreEqual(_targetResult.Date.Hour, resultDay.Date.Hour);
        Assert.AreEqual(_targetResult.Date.Minute, resultDay.Date.Minute);
    }
}