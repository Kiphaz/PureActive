﻿using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Interfaces;
using Serilog.Core;
using Serilog.Events;

namespace PureActive.Logger.Provider.Serilog.Types
{
    public class SerilogLogLevel : ISerilogLogLevel
    {
        public SerilogLogLevel(LogEventLevel minimumLevelSerilog)
        {
            InitialLogEventLevel = minimumLevelSerilog;
            LoggingLevelSwitch = new LoggingLevelSwitch(minimumLevelSerilog);
        }

        public SerilogLogLevel(LogLevel minimumLevelMsft) :
            this(MsftToSerilogLogLevel(minimumLevelMsft))
        {
        }

        public LoggingLevelSwitch LoggingLevelSwitch { get; }

        public LogEventLevel MinimumLogEventLevel
        {
            get => LoggingLevelSwitch.MinimumLevel;
            set => LoggingLevelSwitch.MinimumLevel = value;
        }

        public LogEventLevel InitialLogEventLevel { get; set; }

        public LogLevel MinimumLogLevel
        {
            get => SerilogToMsftLogLevel(LoggingLevelSwitch.MinimumLevel);
            set => LoggingLevelSwitch.MinimumLevel = MsftToSerilogLogLevel(value);
        }

        public LogLevel InitialLogLevel
        {
            get => SerilogToMsftLogLevel(InitialLogEventLevel);
            set => InitialLogEventLevel = MsftToSerilogLogLevel(value);
        }

        public static LogEventLevel MsftToSerilogLogLevel(LogLevel logLevelMsft)
        {
            switch (logLevelMsft)
            {
                case LogLevel.Critical:
                    return LogEventLevel.Fatal;

                case LogLevel.Error:
                    return LogEventLevel.Error;

                case LogLevel.Warning:
                    return LogEventLevel.Warning;

                case LogLevel.Information:
                    return LogEventLevel.Information;

                case LogLevel.Debug:
                    return LogEventLevel.Debug;

                // ReSharper disable once RedundantCaseLabel
                case LogLevel.Trace:
                default:
                    return LogEventLevel.Verbose;
            }
        }

        public static LogLevel SerilogToMsftLogLevel(LogEventLevel logEventLevelSerilog)
        {
            switch (logEventLevelSerilog)
            {
                case LogEventLevel.Debug:
                    return LogLevel.Debug;

                case LogEventLevel.Information:
                    return LogLevel.Information;

                case LogEventLevel.Warning:
                    return LogLevel.Warning;

                case LogEventLevel.Error:
                    return LogLevel.Error;

                case LogEventLevel.Fatal:
                    return LogLevel.Critical;

                // ReSharper disable once RedundantCaseLabel
                case LogEventLevel.Verbose:
                default:
                    return LogLevel.Trace;
            }
        }
    }
}