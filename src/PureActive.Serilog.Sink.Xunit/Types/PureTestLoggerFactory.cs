﻿using Microsoft.Extensions.Logging;
using PureActive.Logger.Provider.Serilog.Types;
using PureActive.Serilog.Sink.Xunit.Interfaces;

namespace PureActive.Serilog.Sink.Xunit.Types
{
    public class PureTestLoggerFactory : PureSeriLoggerFactory, IPureTestLoggerFactory
    {
        public PureTestLoggerFactory(ILoggerFactory loggerFactory) : base(loggerFactory)
        {

        }
    }
}