using System;
using System.IO;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Extensions;
using PureActive.Core.System;
using PureActive.Logger.Provider.Serilog.Configuration;
using PureActive.Logger.Provider.Serilog.Settings;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Serilog.Events;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Logger.Provider.Serilog.IntegrationTests
{
    [Trait("Category", "Integration")]
    public class SerilogProviderIntegrationTests : TestBaseLoggable<SerilogProviderIntegrationTests>
    {
        private readonly IFileSystem _fileSystem;

        public SerilogProviderIntegrationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _fileSystem = new FileSystem(typeof(SerilogProviderIntegrationTests));
        }

        private IPureLoggerFactory CreatePureLoggerFactory(LogEventLevel logEventLevel, LoggingOutputFlags loggingOutputFlags, string logFileName)
        {
            var loggerSettings = new SerilogLoggerSettings(_fileSystem, logEventLevel, loggingOutputFlags);
            var loggerConfiguration = LoggerConfigurationFactory.CreateLoggerConfiguration((string)null, logFileName, loggerSettings, b => true);

            return LoggerConfigurationFactory.CreatePureSeriLoggerFactory(loggerSettings, loggerConfiguration);
        }

        private void AssertLogFileEntry(IPureLoggerSettings loggerSettings, LogLevel logLevel, string logFileName, Action<string, LogLevel> testAction)
        {
            string partialName = _fileSystem.GetFileNameWithoutExtension(logFileName);
            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(loggerSettings.TestLogFolderPath);
            FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + partialName + "*.*");

            foreach (FileInfo foundFile in filesInDir)
            {
                if (foundFile.Name.StartsWith(partialName))
                {
                    if (!_fileSystem.FileExists(foundFile.FullName)) continue;

                    using (var sr = new StreamReader(foundFile.FullName))
                    {
                        var logContents = sr.ReadToEnd();

                        testAction(logContents, logLevel);
                    }
                }
            }
        }


        [Fact]
        public void SerilogProvider_CreateLoggers_AppConsoleFile()
        {
            var logFileName = FileExtensions.GetRandomFileName("", ".log");
            var loggerFactory = CreatePureLoggerFactory(LogEventLevel.Debug, LoggingOutputFlags.TestingAppConsoleFile, logFileName);
            
            // Validate IPureLoggerFactory Interface
            loggerFactory.Should().NotBeNull();
            loggerFactory.PureLoggerSettings.Should().NotBeNull();
            loggerFactory.WrappedLoggerFactory.Should().NotBeNull();

            // Validate creation of loggers
            var pureLogger = loggerFactory.CreatePureLogger<SerilogProviderIntegrationTests>();
            pureLogger.Should().NotBeNull();

            var logger = loggerFactory.CreatePureLogger<SerilogProviderIntegrationTests>();
            logger.Should().NotBeNull();
        }

        [Fact]
        public void SerilogProvider_CreateLogger_AppConsoleFile_LogLevel()
        {
            var logFileName = FileExtensions.GetRandomFileName("", ".log");
            var loggerFactory = CreatePureLoggerFactory(LogEventLevel.Debug, LoggingOutputFlags.TestingAppConsoleFile, logFileName);
            var logger = loggerFactory.CreatePureLogger<SerilogProviderIntegrationTests>();

            const string msg = "Test";
            logger.LogDebug(msg);

            // Dispose Logger Factory so we can access log file
            loggerFactory.Dispose();

            AssertLogFileEntry(loggerFactory.PureLoggerSettings, LogLevel.Debug, logFileName, 
                (logContents, logLevel) =>
                {
                    logContents.Should().EndWith($"{msg}{Environment.NewLine}");
                    logContents.Should().Contain($"[{logLevel}]");
                }
            );
        }


        [Fact]
        public void SerilogProvider_CreateLogger_AppConsoleFile_BadLogLevel()
        {
            var logFileName = FileExtensions.GetRandomFileName("", ".log");
            var loggerFactory = CreatePureLoggerFactory(LogEventLevel.Debug, LoggingOutputFlags.TestingAppConsoleFile, logFileName);
            var logger = loggerFactory.CreatePureLogger<SerilogProviderIntegrationTests>();

            const string msg = "Test";
            logger.LogDebug(msg);

            // Dispose Logger Factory so we can access log file
            loggerFactory.Dispose();

            AssertLogFileEntry(loggerFactory.PureLoggerSettings, LogLevel.Critical, logFileName,
                (logContents, logLevel) =>
                {
                    logContents.Should().EndWith($"{msg}{Environment.NewLine}");
                    logContents.Should().NotContain($"[{logLevel}]");
                }
            );
        }
    }
}