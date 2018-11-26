﻿// ***********************************************************************
// Assembly         : PureActive.Logging.UnitTests
// Author           : SteveBu
// Created          : 11-24-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-24-2018
// ***********************************************************************
// <copyright file="NullLoggerUnitTests.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logging.UnitTests.Logger
{
    /// <summary>
    /// Class NullLoggerUnitTests.
    /// Implements the <see cref="PureActive.Serilog.Sink.Xunit.TestBase.TestBaseLoggable{NullLoggerUnitTests}" />
    /// </summary>
    /// <seealso cref="PureActive.Serilog.Sink.Xunit.TestBase.TestBaseLoggable{NullLoggerUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class NullLoggerUnitTests : TestBaseLoggable<NullLoggerUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullLoggerUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public NullLoggerUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        /// <summary>
        /// Defines the test method NullPureLogger constructor
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_Constructor()
        {
            NullPureLogger.Instance.Should().NotBeNull();
        }

        /// <summary>Defines the test method NullPureLogger_Properties.</summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_Properties()
        {
            NullPureLogger.Instance.Should().NotBeNull();
            NullPureLogger.Instance.IsEnabled(LogLevel.Trace).Should().BeFalse();
        }

        /// <summary>Defines the test method NullPureLogger_ILogger_Methods.</summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_ILogger_Methods()
        {
            using (NullPureLogger.Instance.BeginScope("Test"))
            {
                NullPureLogger.Instance.Log(LogLevel.Debug, new EventId(1), (Exception) null, null);
            }
        }

        /// <summary>Defines the test method to test IPureLogProperty Null</summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_PushLogProperties_IPureLogProperty_Null()
        {
            using (NullPureLogger.Instance.PushLogProperties(null))
            {
                NullPureLogger.Instance.Log(LogLevel.Debug, new EventId(1), (Exception)null, null);
            }

        }

        /// <summary>Defines the test method to test IPureLogProperty Empty</summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_PushLogProperties_IPureLogProperty_Empty()
        {
            using (NullPureLogger.Instance.PushLogProperties(new IPureLogPropertyLevel[0]))
            {
                NullPureLogger.Instance.Log(LogLevel.Debug, new EventId(1), (Exception)null, null);
            }
        }

        /// <summary>Defines the test method NullPureLogger_PushLogProperties_IPureLogProperty_LogLevel.</summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_PushLogProperties_IPureLogProperty_LogLevel()
        {
            using (NullPureLogger.Instance.PushLogProperties(new IPureLogPropertyLevel[0], LogLevel.Debug))
            {
                NullPureLogger.Instance.Log(LogLevel.Debug, new EventId(1), (Exception)null, null);
            }
        }

        [ExcludeFromCodeCoverage]
        private bool IncludeLogProperty(IPureLogPropertyLevel pureLogPropertyLevel) => true;

        /// <summary>Defines the test method NullPureLogger_PushLogProperties_IPureLogProperty_IncludeLogProperty.</summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_PushLogProperties_IPureLogProperty_IncludeLogProperty()
        {
            using (NullPureLogger.Instance.PushLogProperties(new IPureLogPropertyLevel[0], IncludeLogProperty))
            {
                NullPureLogger.Instance.Log(LogLevel.Debug, new EventId(1), (Exception)null, null);
            }
        }

        /// <summary>Defines the test method NullPureLogger_PushLogProperties_KeyValuePair.</summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_PushLogProperties_KeyValuePair()
        {
            using (NullPureLogger.Instance.PushLogProperties(new KeyValuePair<string, object>[0]))
            {
                NullPureLogger.Instance.Log(LogLevel.Debug, new EventId(1), (Exception)null, null);
            }
        }

        /// <summary>Defines the test method to test PushProperty with int</summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_PushProperty_Int()
        {
            using (NullPureLogger.Instance.PushProperty("propertyName", 2))
            {
                NullPureLogger.Instance.Log(LogLevel.Debug, new EventId(1), (Exception)null, null);
            }
        }

        /// <summary>Defines the test method to test PushProperty with Object</summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_PushProperty_Object()
        {
            using (NullPureLogger.Instance.PushProperty("propertyName", new object()))
            {
                NullPureLogger.Instance.Log(LogLevel.Debug, new EventId(1), (Exception)null, null);
            }
        }

        /// <summary>Defines the test method to test PushProperty with string type</summary>
        /// <autogeneratedoc />
        [Fact]
        public void NullPureLogger_PushProperty_Type()
        {
            using (NullPureLogger.Instance.PushProperty("propertyName", "value"))
            {
                NullPureLogger.Instance.Log(LogLevel.Debug, new EventId(1), (Exception)null, null);
            }
        }
    }
}