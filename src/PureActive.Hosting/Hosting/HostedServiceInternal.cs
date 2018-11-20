﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;

namespace PureActive.Hosting.Hosting
{
    public abstract class HostedServiceInternal<T> : PureLoggableBase<T>, IHostedServiceInternal
    {
        protected HostedServiceInternal(ICommonServices commonServices, IApplicationLifetime applicationLifetime,
            ServiceHost serviceHost) :
            base(commonServices?.LoggerFactory)
        {
            CommonServices = commonServices ?? throw new ArgumentNullException(nameof(commonServices));
            ApplicationLifetime = applicationLifetime;
            ServiceHost = serviceHost;

            if (applicationLifetime != null)
            {
                // Add ApplicationLifeTime delegates after logger creation
                _cancellationTokenOnStarted = applicationLifetime.ApplicationStarted.Register(OnStarted);
                _cancellationTokenOnStopping = applicationLifetime.ApplicationStopping.Register(OnStopping);
                _cancellationTokenOnStopped = applicationLifetime.ApplicationStopped.Register(OnStopped);
            }
        }

        private IApplicationLifetime ApplicationLifetime { get; }
        public ICommonServices CommonServices { get; }

        public ServiceHost ServiceHost { get; protected set; }

        public ServiceHostStatus ServiceHostStatus { get; protected set; } = ServiceHostStatus.Stopped;

        public override IEnumerable<IPureLogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel,
            LoggableFormat loggableFormat)
        {
            var logPropertyLevels = loggableFormat.IsWithParents()
                ? base.GetLogPropertyListLevel(logLevel, loggableFormat)?.ToList()
                : new List<IPureLogPropertyLevel>();

            if (logLevel <= LogLevel.Information)
            {
                logPropertyLevels?.Add(new PureLogPropertyLevel($"{ServiceHost}HostStatus", ServiceHostStatus,
                    LogLevel.Information));
            }

            return logPropertyLevels?.Where(p => p.MinimumLogLevel.CompareTo(logLevel) >= 0);
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.Running;

            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.Stopped;

            return Task.CompletedTask;
        }

        public virtual void RequestStopService()
        {
            ApplicationLifetime?.StopApplication();
        }

        protected virtual void OnStarted()
        {
            Logger?.LogInformation("{ServiceHost} OnStarted", ServiceHost);
        }

        protected virtual void OnStopping()
        {
            Logger?.LogInformation("{ServiceHost} OnStopping", ServiceHost);
        }

        protected virtual void OnStopped()
        {
            Logger?.LogInformation("{ServiceHost} OnStopped", ServiceHost);
        }

        // IApplicationLifetime support
        // ReSharper disable NotAccessedField.Local
        private readonly CancellationTokenRegistration _cancellationTokenOnStarted;
        private readonly CancellationTokenRegistration _cancellationTokenOnStopping;

        private readonly CancellationTokenRegistration _cancellationTokenOnStopped;
        // ReSharper restore NotAccessedField.Local
    }
}