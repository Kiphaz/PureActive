// ***********************************************************************
// Assembly         : PureActive.Queue.Hangfire.UnitTests
// Author           : SteveBu
// Created          : 11-17-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="QueueHangfireUnitTests.cs" company="BushChang Corporation">
//     � 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using Hangfire.Logging;
using Hangfire.States;
using Hangfire.Storage.Monitoring;
using PureActive.Core.Abstractions.Queue;
using PureActive.Core.Extensions;
using PureActive.Queue.Hangfire.Queue;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Serilog.Sinks.TestCorrelator;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Queue.Hangfire.UnitTests
{
    /// <summary>
    /// Class QueueHangfireUnitTests.
    /// Implements the <see cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{QueueHangfireUnitTests}" />
    /// </summary>
    /// <seealso cref="Serilog.Sink.Xunit.TestBase.TestBaseLoggable{QueueHangfireUnitTests}" />
    /// <autogeneratedoc />
    [Trait("Category", "Unit")]
    public class QueueHangfireUnitTests : TestBaseLoggable<QueueHangfireUnitTests>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueHangfireUnitTests"/> class.
        /// </summary>
        /// <param name="testOutputHelper">The test output helper.</param>
        /// <autogeneratedoc />
        public QueueHangfireUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Defines the test method QueueHangfire_Logging.
        /// </summary>
        /// <autogeneratedoc />
        [Fact]
        public void QueueHangfire_Logging()
        {
            var hangFireLogger = new HangfireLogProvider(TestLoggerFactory);
            var sourceContext = "QueueHangfireUnitTests";
            var logger = hangFireLogger.GetLogger(sourceContext);
            var testString = "Test: QueueHangfire_Logging";

            using (TestCorrelator.CreateContext())
            {
                logger.Log(LogLevel.Debug, () => testString);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.Properties["State"].ToString()
                    .Should().Be(testString.ToDoubleQuoted());

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.Properties["SourceContext"].ToString()
                    .Should().Be(sourceContext.ToDoubleQuoted());
            }
        }

        /// <summary>
        /// Defines the test method QueueHangfire_Logging LogLevels.
        /// </summary>
        /// <autogeneratedoc />
        [Theory]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Info)]
        [InlineData(LogLevel.Warn)]
        [InlineData(LogLevel.Error)]
        [InlineData(LogLevel.Fatal)]
        public void QueueHangfire_Logging_LogLevels(LogLevel logLevel)
        {
            var hangFireLogger = new HangfireLogProvider(TestLoggerFactory);
            var sourceContext = "QueueHangfireUnitTests";
            var logger = hangFireLogger.GetLogger(sourceContext);
            var testString = "Test: QueueHangfire_Logging";

            logger.Log(logLevel, () => testString);
        }

        [ExcludeFromCodeCoverage]
        private string TestString()
        {
            return string.Empty;
        }

        [Fact]
        public void QueueHangfire_Logging_LogLevels_Bogus()
        {
            var hangFireLogger = new HangfireLogProvider(TestLoggerFactory);
            var sourceContext = "QueueHangfireUnitTests";
            var logger = hangFireLogger.GetLogger(sourceContext);
  
            Func<bool> fx = () => logger.Log((LogLevel)100, TestString);
            fx.Should().Throw<ArgumentOutOfRangeException>();
        }


        private readonly MethodInfo _methodInfoGetJobState = typeof(JobQueueClient).GetMethod("GetJobState", BindingFlags.NonPublic | BindingFlags.Static);

        private JobState InvokeGetJobState(StateHistoryDto jobDetails)
        {
            object[] parameters = { jobDetails };

            return (JobState)_methodInfoGetJobState.Invoke(null, parameters);
        }

        public static TheoryData<StateHistoryDto, JobState> StateHistoryDtoData => new TheoryData<StateHistoryDto, JobState>
        {
            {new StateHistoryDto(){StateName = EnqueuedState.StateName}, JobState.NotStarted },
            {new StateHistoryDto(){StateName = AwaitingState.StateName}, JobState.Awaiting },
            {new StateHistoryDto(){StateName = SucceededState.StateName}, JobState.Succeeded },
            {new StateHistoryDto(){StateName = FailedState.StateName}, JobState.Failed },
            {new StateHistoryDto(){StateName = ScheduledState.StateName}, JobState.Scheduled },
            {new StateHistoryDto(){StateName = DeletedState.StateName}, JobState.Deleted },
            {new StateHistoryDto(){StateName = ProcessingState.StateName}, JobState.InProgress },
            {new StateHistoryDto(){StateName = "Foobar"}, JobState.Unknown },
        };

        [Theory]
        [MemberData(nameof(StateHistoryDtoData))]
        public void QueueHangfire_GetJobState(StateHistoryDto jobDetails, JobState jobStateExpected)
        {
            InvokeGetJobState(jobDetails).Should().Be(jobStateExpected);
        }
    }
}