using System;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Extensions.Extensions;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logging.UnitTests.Extensions
{

    public class LoggerExtensionsUnitTests : LoggingUnitTestBase<LoggerExtensionsUnitTests>
    {
        public LoggerExtensionsUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {

        }

        [Fact]
        public void LoggerExtensions_BeginPropertyScope()
        {
            using (Logger.BeginPropertyScope("PropertyInt", 14))
            {
                Logger.LogDebug("Log Int Property Scope");
            }
        }
    }
}
