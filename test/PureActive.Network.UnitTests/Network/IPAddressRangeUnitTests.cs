﻿// ***********************************************************************
// Assembly         : PureActive.Network.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="IPAddressRangeUnitTests.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Net;
using PureActive.Hosting.Abstractions.Extensions;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Network.Extensions.Network;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.UnitTests.Network
{
    /// <summary>
    /// Class IPAddressRangeUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{IPAddressRangeUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{IPAddressRangeUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class IPAddressRangeUnitTests : TestBaseLoggable<IPAddressRangeUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IPAddressRangeUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public IPAddressRangeUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Defines the test method IPAddressRange_IPAddress_Count.
        /// </summary>
        /// <param name="ipAddressLowerInclusiveString">The ip address lower inclusive string.</param>
        /// <param name="ipAddressUpperInclusiveString">The ip address upper inclusive string.</param>
        /// <param name="total">The total.</param>
        /// <autogeneratedoc />
        [Theory]
        [InlineData("10.1.10.255", "10.1.11.2", 4)]
        [InlineData("10.1.10.0", "10.1.11.255", 512)]
        public void IPAddressRange_IPAddress_Count(string ipAddressLowerInclusiveString,
            string ipAddressUpperInclusiveString, int total)
        {
            var ipAddressLowerInclusive = IPAddress.Parse(ipAddressLowerInclusiveString);
            var ipAddressUpperInclusive = IPAddress.Parse(ipAddressUpperInclusiveString);

            var ipAddressRange = new IPAddressRange(ipAddressLowerInclusive, ipAddressUpperInclusive);

            int count = 0;
            IPAddress ipAddressPrev = IPAddress.None;

            foreach (var ipAddress in ipAddressRange)
            {
                if (count++ == 0)
                    Assert.Equal(ipAddressLowerInclusive, ipAddress);
                else
                    Assert.Equal(ipAddress.Decrement(), ipAddressPrev);

                ipAddressPrev = ipAddress;
            }

            Assert.Equal(total, count);
            Assert.Equal(ipAddressUpperInclusive, ipAddressPrev);
        }

        /// <summary>
        /// Defines the test method IPAddressRange_IPAddressSubnet_Count.
        /// </summary>
        /// <param name="ipAddressLowerInclusiveString">The ip address lower inclusive string.</param>
        /// <param name="ipAddressSubnetString">The ip address subnet string.</param>
        /// <param name="total">The total.</param>
        /// <autogeneratedoc />
        [Theory]
        [InlineData("10.1.10.5", IPAddressExtensions.StringSubnetClassC, 256)]
        [InlineData("10.1.10.5", IPAddressExtensions.StringSubnetClassB, 65_536)]
        //       [InlineData("10.1.10.5", StringSubnetClassA, 16_777_216)]
        public void IPAddressRange_IPAddressSubnet_Count(string ipAddressLowerInclusiveString,
            string ipAddressSubnetString, int total)
        {
            var ipAddressLowerInclusive = IPAddress.Parse(ipAddressLowerInclusiveString);
            var ipAddressSubnetInclusive = IPAddress.Parse(ipAddressSubnetString);

            var ipAddressSubnet = new IPAddressSubnet(ipAddressLowerInclusive, ipAddressSubnetInclusive);

            var ipAddressRange = new IPAddressRange(ipAddressSubnet);

            int count = 0;
            IPAddress ipAddressPrev = IPAddress.None;

            foreach (var ipAddress in ipAddressRange)
            {
                if (count++ == 0)
                    Assert.Equal(ipAddressSubnet.NetworkAddress, ipAddress);
                else
                    Assert.Equal(ipAddress.Decrement(), ipAddressPrev);

                ipAddressPrev = ipAddress;
            }

            Assert.Equal(total, count);
            Assert.Equal(ipAddressSubnet.BroadcastAddress, ipAddressPrev);
        }
    }
}