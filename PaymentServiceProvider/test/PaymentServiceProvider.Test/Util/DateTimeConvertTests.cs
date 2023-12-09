using PaymentServiceProvider.Util.Utils;

namespace PaymentServiceProvider.Test.Util
{
    public class DateTimeExtensionsTest
    {
        [Fact(DisplayName = "DateFormatter: Should return formatted value")]
        public void DateFormatter_Should_Return_Formatted_Value()
        {
            // Arrange
            var dateToFormat = new DateTime(2023, 04, 12, 0, 0, 0);

            // Act
            var valueFormatted = dateToFormat.DateFormatter();

            // Assert
            valueFormatted.Should().Be("12/04/2023");
        }
        
        [Fact(DisplayName = "DateFormatter: Should return datetime")]
        public void DateFormatter_Should_Return_datetime()
        {
            // Arrange
            var expectedResponse = new DateTime(2023, 04, 12, 0, 0, 0);
            var dateToParse = "12/04/2023";

            // Act
            var valueFormatted = dateToParse.ParseToDate();

            // Assert
            valueFormatted.Should().Be(expectedResponse);
        }
    }
}
