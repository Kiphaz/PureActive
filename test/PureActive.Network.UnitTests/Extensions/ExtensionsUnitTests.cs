using System.Net.NetworkInformation;
using PureActive.Network.Abstractions.Extensions;
using Xunit;

namespace PureActive.Network.UnitTests.Extensions
{
    public class ExtensionsUnitTests
    {
        private const string PhysicalAddressZeros = "00-00-00-00-00-00";

        [Fact]
        public void ToDashStringTest()
        {
            PhysicalAddress physicalAddress = PhysicalAddress.Parse("00-1A-8C-46-27-D0");
            var physicalAddressDash = physicalAddress.ToDashString();
            Assert.Equal(physicalAddressDash, $"00-1A-8C-46-27-D0");
        }

        [Fact]
        public void ToColonStringTest()
        {
            PhysicalAddress physicalAddress = PhysicalAddress.Parse("00-1A-8C-46-27-D0");
            var physicalAddressColon = physicalAddress.ToColonString();
            Assert.Equal(physicalAddressColon, $"00:1A:8C:46:27:D0");
        }


        [Theory]
        [InlineData("0-0-0-0-0-0", PhysicalAddressZeros)]
        [InlineData("1:1A:8C:46:27:D0", "01-1A-8C-46-27-D0")]
        [InlineData("00:1A:8C:46:27:D0", "00-1A-8C-46-27-D0")]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("3:4:5", "03-04-05")]
        public void NormalizedParseTest(string physicalAddressString, string physicalAddressStringExpectedValue)
        { 
            var physicalAddressExpectedValue = string.IsNullOrEmpty(physicalAddressStringExpectedValue) ? PhysicalAddress.None : PhysicalAddress.Parse(physicalAddressStringExpectedValue);
            var physicalAddressNormalized = PhysicalAddressExtensions.NormalizedParse(physicalAddressString);

            Assert.Equal(physicalAddressExpectedValue, physicalAddressNormalized);
        }
    }
}