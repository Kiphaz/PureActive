// ***********************************************************************
// Assembly         : PureActive.Core.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-29-2018
// ***********************************************************************
// <copyright file="NetworkingSystemUnitTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using FluentAssertions;
using PureActive.Core.Abstractions.System;
using PureActive.Core.System;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Core.UnitTests.System
{
    /// <summary>
    /// Class NetworkingSystemUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{NetworkingSystemUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{NetworkingSystemUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class NetworkingSystemUnitTests : TestBaseLoggable<NetworkingSystemUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkingSystemUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public NetworkingSystemUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _networkingSystem = new NetworkingSystem(TestLoggerFactory.CreatePureLogger<NetworkingSystem>());
        }

        private readonly INetworkingSystem _networkingSystem;

        [Fact]
        public void NetworkingSystem_Constructor()
        {
            _networkingSystem.Should().NotBeNull().And.Subject.Should().BeAssignableTo<INetworkingSystem>();
        }
   }
}