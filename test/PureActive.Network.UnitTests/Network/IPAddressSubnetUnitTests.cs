﻿// ***********************************************************************
// Assembly         : PureActive.Network.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-24-2018
// ***********************************************************************
// <copyright file="IPAddressSubnetUnitTests.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Net;
using FluentAssertions;
using PureActive.Hosting.Abstractions.Extensions;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.UnitTests.Network
{
    /// <summary>
    /// Class IPAddressSubnetUnitTests.
    /// Implements the <see cref="PureActive.Serilog.Sink.Xunit.TestBase.TestBaseLoggable{IPAddressSubnetUnitTests}" />
    /// </summary>
    /// <seealso cref="PureActive.Serilog.Sink.Xunit.TestBase.TestBaseLoggable{IPAddressSubnetUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class IPAddressSubnetUnitTests : TestBaseLoggable<IPAddressSubnetUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IPAddressSubnetUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public IPAddressSubnetUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        /// <summary>
        /// Defines the test method IPAddressSubnet_Constructor.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void IPAddressSubnet_Constructor()
        {
            var ipAddressSubnet = new IPAddressSubnet(TestAddressClassA);
            ipAddressSubnet.Should().NotBeNull();
        }
        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// Gets the network address data.
        /// </summary>
        /// <value>The network address data.</value>
        /// <autogeneratedoc />
        public static TheoryData<IPAddress, IPAddress, IPAddress> NetworkAddressData => new TheoryData<IPAddress, IPAddress, IPAddress>
        {
            {TestAddressClassA, IPAddressExtensions.SubnetClassA, NetworkAddressClassA},
            {TestAddressClassB, IPAddressExtensions.SubnetClassB, NetworkAddressClassB},
            {TestAddressClassC, IPAddressExtensions.SubnetClassC, NetworkAddressClassC},

            {TestAddressClass1A, IPAddressExtensions.SubnetClassA, NetworkAddressClassA},
            {TestAddressClass1B, IPAddressExtensions.SubnetClassB, NetworkAddressClassB},

            {TestAddressClass1A, IPAddressExtensions.SubnetClassC, NetworkAddressClass1C}
        };

        /// <summary>
        /// Defines the test method IPAddressSubnet_NetworkAddress.
        /// </summary>
        /// <param name="ipAddressNetwork">The ip address network.</param>
        /// <param name="subnetAddress">The subnet address.</param>
        /// <param name="ipAddressExpected">The ip address expected.</param>
        /// <autogeneratedoc />
        [Theory]
        [MemberData(nameof(NetworkAddressData))]
        public void IPAddressSubnet_NetworkAddress(IPAddress ipAddressNetwork, IPAddress subnetAddress, IPAddress ipAddressExpected)
        {
            var ipAddressSubnet = new IPAddressSubnet(ipAddressNetwork, subnetAddress);
            ipAddressSubnet.Should().NotBeNull();
            ipAddressSubnet.NetworkAddress.Should().Be(ipAddressExpected);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        /// Gets the broadcast address data.
        /// </summary>
        /// <value>The broadcast address data.</value>
        /// <autogeneratedoc />
        public static TheoryData<IPAddress, IPAddress, IPAddress> BroadcastAddressData => new TheoryData<IPAddress, IPAddress, IPAddress>
        {
            {TestAddressClassA, IPAddressExtensions.SubnetClassA, BroadcastAddressClassA},
            {TestAddressClassB, IPAddressExtensions.SubnetClassB, BroadcastAddressClassB},
            {TestAddressClassC, IPAddressExtensions.SubnetClassC, BroadcastAddressClassC},

            {TestAddressClass1A, IPAddressExtensions.SubnetClassA, BroadcastAddressClassA},
            {TestAddressClass1B, IPAddressExtensions.SubnetClassB, BroadcastAddressClassB},

            {TestAddressClass1A, IPAddressExtensions.SubnetClassC, BroadcastAddressClass1C}
        };

        /// <summary>
        /// Defines the test method IPAddressSubnet_BroadcastAddress.
        /// </summary>
        /// <param name="ipAddressBroadcast">The ip address broadcast.</param>
        /// <param name="subnetAddress">The subnet address.</param>
        /// <param name="ipAddressExpected">The ip address expected.</param>
        /// <autogeneratedoc />
        [Theory]
        [MemberData(nameof(BroadcastAddressData))]
        public void IPAddressSubnet_BroadcastAddress(IPAddress ipAddressBroadcast, IPAddress subnetAddress, IPAddress ipAddressExpected)
        {
            var ipAddressSubnet = new IPAddressSubnet(ipAddressBroadcast, subnetAddress);
            ipAddressSubnet.Should().NotBeNull();
            ipAddressSubnet.BroadcastAddress.Should().Be(ipAddressExpected);
        }

        /// <summary>
        /// Defines the test method IPAddressSubnet_AddressFamily_IPv6.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void IPAddressSubnet_AddressFamily_IPv6()
        {
            Func<IPAddressSubnet> fx = () => new IPAddressSubnet(IPAddress.IPv6None, IPAddressExtensions.SubnetClassC);

            fx.Should().Throw<ArgumentException>().And.ParamName.Should().Be("ipAddress");
        }


        /// <summary>
        /// Defines the test method IPAddressSubnet_Equal.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void IPAddressSubnet_CompareTo_Equal()
        {
            var ipAddressSubnet = new IPAddressSubnet(TestAddressClassC, IPAddressExtensions.SubnetClassC);

            ipAddressSubnet.CompareTo(ipAddressSubnet).Should().Be(0);
        }

        /// <summary>
        /// Defines the test method IPAddressSubnet_Equal_Object.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void IPAddressSubnet_CompareTo_Equal_Object()
        {
            var ipAddressSubnet = new IPAddressSubnet(TestAddressClassC, IPAddressExtensions.SubnetClassC);

            ipAddressSubnet.CompareTo((object)ipAddressSubnet).Should().Be(0);
        }

        /// <summary>
        /// Defines the test method IPAddressSubnet_Equals_Object.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void IPAddressSubnet_CompareTo_Equals_Object()
        {
            var ipAddressSubnet = new IPAddressSubnet(TestAddressClassC, IPAddressExtensions.SubnetClassC);

            ipAddressSubnet.Equals((object) ipAddressSubnet).Should().BeTrue();
        }

        /// <summary>
        /// Defines the test method IPAddressSubnet_CompareTo_Equal_DifferentSubnet.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void IPAddressSubnet_CompareTo_Equal_DifferentSubnet()
        {
            var ipAddressSubnet1 = new IPAddressSubnet(TestAddressClassA, IPAddressExtensions.SubnetClassA);
            var ipAddressSubnet2 = new IPAddressSubnet(TestAddressClassA, IPAddressExtensions.SubnetClassC);

            // Subnet is ignored
            ipAddressSubnet1.CompareTo(ipAddressSubnet2).Should().Be(0);
        }


        /// <summary>
        /// Defines the test method IPAddressSubnet_IsAddressOnSameSubnet False.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void IPAddressSubnet_IsAddressOnSameSubnet_False()
        {
            var ipAddressSubnet1 = new IPAddressSubnet(TestAddressClassA, IPAddressExtensions.SubnetClassA);
            var ipAddressSubnet2 = new IPAddressSubnet(TestAddressClassC, IPAddressExtensions.SubnetClassC);

            // Subnet is ignored
            ipAddressSubnet1.IsAddressOnSameSubnet(ipAddressSubnet2.IPAddress).Should().BeFalse();
        }


        /// <summary>
        /// Defines the test method IPAddressSubnet_NotIPAddressSubnet
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void IPAddressSubnet_CompareTo_NotIPAddressSubnet()
        {
            Func<int> fx = () =>
                new IPAddressSubnet(TestAddressClassC, IPAddressExtensions.SubnetClassC).CompareTo(new object());

            fx.Should().Throw<ArgumentException>().And.ParamName.Should().Be("obj");
        }

        /// <summary>
        /// Defines the test method IPAddressSubnet_Null.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void IPAddressSubnet_CompareTo_Null()
        {
            new IPAddressSubnet(TestAddressClassC, IPAddressExtensions.SubnetClassC).CompareTo(null).Should().Be(1);
        }



        /// <summary>
        /// The network address class a
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress NetworkAddressClassA = IPAddress.Parse("10.0.0.0");
        /// <summary>
        /// The network address class b
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress NetworkAddressClassB = IPAddress.Parse("172.16.0.0");
        /// <summary>
        /// The network address class c
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress NetworkAddressClassC = IPAddress.Parse("192.168.1.0");
        /// <summary>
        /// The network address class1 c
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress NetworkAddressClass1C = IPAddress.Parse("10.0.1.0");
        /// <summary>
        /// The test address class a
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress TestAddressClassA = IPAddress.Parse("10.0.0.5");
        /// <summary>
        /// The test address class b
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress TestAddressClassB = IPAddress.Parse("172.16.0.5");
        /// <summary>
        /// The test address class c
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress TestAddressClassC = IPAddress.Parse("192.168.1.5");
        /// <summary>
        /// The test address class1 a
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress TestAddressClass1A = IPAddress.Parse("10.0.1.5");
        /// <summary>
        /// The test address class1 b
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress TestAddressClass1B = IPAddress.Parse("172.16.1.5");
        /// <summary>
        /// The broadcast address class a
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress BroadcastAddressClassA = IPAddress.Parse("10.255.255.255");
        /// <summary>
        /// The broadcast address class b
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress BroadcastAddressClassB = IPAddress.Parse("172.16.255.255");
        /// <summary>
        /// The broadcast address class c
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress BroadcastAddressClassC = IPAddress.Parse("192.168.1.255");
        /// <summary>
        /// The broadcast address class1 c
        /// </summary>
        /// <autogeneratedoc />
        private static readonly IPAddress BroadcastAddressClass1C = IPAddress.Parse("10.0.1.255");

    }
}