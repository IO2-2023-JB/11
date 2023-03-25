using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YouTubeV2.Application.Utils;

namespace YouTubeV2.Application.Tests.UtilitesTests
{
    [TestClass]
    public class RangeExtensionsTests
    {
        [TestMethod]
        public void FromStringValidInputReturnsEquivalentRange()
        {
            // ARRANGE
            string input = "1-20";
            int start = 1;
            int end = 20;

            // ACT
            Range range = RangeExtensions.FromString(input);

            // ARRANGE
            range.Start.Value.Should().Be(start);
            range.End.Value.Should().Be(end);
        }

        [TestMethod]
        public void FromStringWithInputWithWrongNumberOfDashesShouldThrowAnException()
        {
            // ARRANGE
            string input = "1-2-0";

            // ACT
            Action action = () => RangeExtensions.FromString(input);

            // ARRANGE
            action.Should().Throw<ArgumentException>().WithMessage($"Expected input argument format is int1-int2. Recieved input argument: {input}");
        }

        [TestMethod]
        public void FromStringWithFirstNumberNotBeingANumberShouldThrowAnException()
        {
            // ARRANGE
            string input = "test-2";

            // ACT
            Action action = () => RangeExtensions.FromString(input);

            // ARRANGE
            action.Should().Throw<ArgumentException>().WithMessage($"Expected input argument format is int1-int2. In recieved input ({input}) int1 can not be converted to an integer");
        }

        [TestMethod]
        public void FromStringWithSecondNumberNotBeingANumberShouldThrowAnException()
        {
            // ARRANGE
            string input = "1-test";

            // ACT
            Action action = () => RangeExtensions.FromString(input);

            // ARRANGE
            action.Should().Throw<ArgumentException>().WithMessage($"Expected input argument format is int1-int2. In recieved input ({input}) int2 can not be converted to an integer");
        }
    }
}
