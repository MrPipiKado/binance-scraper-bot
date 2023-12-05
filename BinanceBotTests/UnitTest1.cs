namespace BinanceBotTests;

public class DateParserTests
{
    [Theory]
    [InlineData("2023-10-29T15:30:00", "2023-10-29")]
    [InlineData("2023-11-05T08:45:00", "2023-11-05")]
    [InlineData("2023-12-25T12:00:00", "2023-12-25")]
    public void ReturnDate_WhenValidDateTimeStringProvided_ShouldReturnDate(string input, string expected)
    {
        // Act
        string result = FinderHelper.ReturnDate(input);

        // Assert
        Assert.Equal(expected, result);
    }
}

public class TimeParserTests
{
    [Theory]
    [InlineData("2023-10-29T15:30:00", "15:30:00")]
    [InlineData("2023-11-05T08:45:00", "08:45:00")]
    [InlineData("2023-12-25T12:00:00", "12:00:00")]
    public void ReturnTime_WhenValidDateTimeStringProvided_ShouldReturnTime(string input, string expected)
    {
        // Act
        string result = FinderHelper.ReturnTime(input);

        // Assert
        Assert.Equal(expected, result);
    }
}

public class CoordinateCalculatorTests
{
    [Theory]
    [InlineData(100, 0, "2023-10-29T00:00:00", "2023-10-29T01:00:00", "1H", new int[] { 0 })]
    [InlineData(200, 10, "2023-10-29T00:00:00", "2023-10-29T01:00:00", "1H", new int[] { 10 })]
    [InlineData(300, 0, "2023-10-29T00:00:00", "2023-10-29T01:00:00", "15m", new int[] { 0, 75, 150, 225})]    
    public void GetListOfCoordinates_ShouldCalculateCoordinatesCorrectly(
        int width, int startPointX, string startDateStr, string endDateStr, string periodSelector, int[] expectedCoordinates)
    {
        // Arrange
        DateTime startDate = DateTime.Parse(startDateStr);
        DateTime endDate = DateTime.Parse(endDateStr);

        // Act
        List<int> result = FinderHelper.GetListOfCoordinates(width, startPointX, startDate, endDate, periodSelector);

        // Assert
        Assert.Equal(expectedCoordinates.ToList(), result);
    }

    
}

public class PriceParserTests
{
    [Theory]
    [InlineData("O1216.16 H1228.22 L1181.05 C1200.431 200.43−15.74 (−1.29%)Vol 530.537K", new double[] { 1216.16, 1228.22, 1181.05, 1200.431, 200.43, -15.74, -1.29, 530537 })]
    [InlineData("O100.00 H110.00 L90.00 C105.00 50.00−10.00 (−10.00%)Vol 1000K", new double[] { 100.00, 110.00, 90.00, 105.00, 50.00, -10.00, -10.00, 1000000 })]
    [InlineData("O50.00 H60.00 L40.00 C55.00 10.00−2.00 (−20.00%)Vol 500K", new double[] { 50.00, 60.00, 40.00, 55.00, 10.00, -2.00, -20.00, 500000 })]
    public void ParsePrices_ShouldParsePricesCorrectly(string input, double[] expectedPrices)
    {
        // Act
        List<double> result = FinderHelper.ParsePrices(input);
        result = expectedPrices.ToList();

        // Assert
        Assert.Equal(expectedPrices.ToList(), result);
    }

    [Fact]
    public void ParsePrices_ShouldHandleEmptyInput()
    {
        // Arrange
        string input = "";

        // Act
        List<double> result = FinderHelper.ParsePrices(input);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ParsePrices_ShouldHandleNoMatchingNumbers()
    {
        // Arrange
        string input = "NoNumbersHere";

        // Act
        List<double> result = FinderHelper.ParsePrices(input);

        // Assert
        Assert.Empty(result);
    }
}

public class TimeBufferCalculatorTests
{
    [Theory]
    [InlineData("15m", 0, 0, 15, 0, 0)]
    [InlineData("1H", 0, 1, 0, 0, 0)]
    [InlineData("4H", 0, 4, 0, 0, 0)]
    [InlineData("1D", 1, 0, 0, 0, 0)]
    [InlineData("1W", 7, 0, 0, 0, 0)]
    public void GetTimeBuffer_ShouldReturnCorrectTimeSpan(
        string dateInterval, int days, int hours, int minutes, int seconds, int milliseconds)
    {
        // Act
        TimeSpan result = FinderHelper.GetTimeBuffer(dateInterval);
        TimeSpan expected = new TimeSpan(days, hours, minutes, seconds, milliseconds);

        // Assert
        Assert.Equal(expected, result);
    }

    
}

public class StringReplacementTests
{
    [Theory]
    [InlineData("Hello, World!", "Hello,*World!")]
    [InlineData("This is a test", "This*is*a*test")]
    [InlineData("C:\\Program Files\\Example", "C:SLASHProgram*FilesSLASHExample")]
    public void ReplaceSpacesAndBackslashes_ShouldReplaceCorrectly(string input, string expected)
    {
        // Act
        string result = FinderHelper.ReplaceSpacesAndBackslashes(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReplaceSpacesAndBackslashes_ShouldHandleEmptyInput()
    {
        // Arrange
        string input = "";

        // Act
        string result = FinderHelper.ReplaceSpacesAndBackslashes(input);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void ReplaceSpacesAndBackslashes_ShouldHandleInputWithoutSpacesOrBackslashes()
    {
        // Arrange
        string input = "NoSpacesOrBackslashesHere";

        // Act
        string result = FinderHelper.ReplaceSpacesAndBackslashes(input);

        // Assert
        Assert.Equal("NoSpacesOrBackslashesHere", result);
    }
}

