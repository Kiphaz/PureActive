﻿// ***********************************************************************
// Assembly         : PureActive.Network.Services.PingService
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="PingService.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.Hosting;
using PureActive.Logging.Abstractions.Types;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Abstractions.PingService;
using PureActive.Network.Abstractions.PingService.Events;
using PureActive.Network.Abstractions.PingService.Extensions;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Services.PingService
{
    /// <summary>
    /// Class PingService.
    /// Implements the <see cref="Hosting.Hosting.BackgroundServiceInternal{PingService}" />
    /// Implements the <see cref="IPingService" />
    /// </summary>
    /// <seealso cref="Hosting.Hosting.BackgroundServiceInternal{PingService}" />
    /// <seealso cref="IPingService" />
    /// <autogeneratedoc />
    public partial class PingService : BackgroundServiceInternal<PingService>, IPingService
    {
        /// <summary>
        /// The default TTL
        /// </summary>
        /// <autogeneratedoc />
        private static readonly int DefaultTtl = 30;
        /// <summary>
        /// The default network timeout
        /// </summary>
        /// <autogeneratedoc />
        private static readonly int DefaultNetworkTimeout = 250;

        /// <summary>
        /// The ping task
        /// </summary>
        /// <autogeneratedoc />
        private readonly IPingTask _pingTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="PingService"/> class.
        /// </summary>
        /// <param name="commonServices">The common services.</param>
        /// <param name="applicationLifetime">The application lifetime.</param>
        /// <exception cref="ArgumentNullException">commonServices</exception>
        /// <autogeneratedoc />
        public PingService(ICommonServices commonServices, IApplicationLifetime applicationLifetime = null) :
            base(commonServices, applicationLifetime, ServiceHost.PingService)
        {
            // base throws if (commonServices == null) throw new ArgumentNullException(nameof(commonServices));

            OnPingReplyService += PingReplyLoggingEventHandler;

            _pingTask = new PingTaskImpl(commonServices);
            _pingTask.OnPingReplyTask += PingTaskReplyEventHandler;
        }

        /// <summary>
        /// Occurs when [on ping reply].
        /// </summary>
        /// <autogeneratedoc />
        public event PingReplyEventHandler OnPingReplyService;
        /// <summary>
        /// Gets or sets a value indicating whether [enable logging].
        /// </summary>
        /// <value><c>true</c> if [enable logging]; otherwise, <c>false</c>.</value>
        /// <autogeneratedoc />
        public bool EnableLogging { get; set; } = true;


        /// <summary>
        /// Pings the network asynchronous.
        /// </summary>
        /// <param name="ipAddressSubnet">The ip address subnet.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="pingCallLimit">The ping call limit.</param>
        /// <param name="waitBetweenPings">delay in milliseconds between pings</param>
        /// <param name="shuffle">if set to <c>true</c> [shuffle].</param>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        public Task PingNetworkAsync(IPAddressSubnet ipAddressSubnet, CancellationToken cancellationToken, int timeout,
            int pingCallLimit, int waitBetweenPings, bool shuffle)
        {
            var iPAddressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);

            return _pingTask.PingNetworkAsync(iPAddressSubnet, cancellationToken, timeout, pingCallLimit, waitBetweenPings, shuffle);
        }

        private void PingTaskReplyEventHandler(object sender, PingReplyEventArgs pingReplyEventArgs)
        {
            OnPingReplyService?.Invoke(this, pingReplyEventArgs);
        }

        /// <summary>
        /// Pings the ip address asynchronous.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Task&lt;PingReply&gt;.</returns>
        /// <autogeneratedoc />
        public Task<PingReply> PingIpAddressAsync(IPAddress ipAddress, int timeout)
        {
            return _pingTask.PingIpAddressAsync(ipAddress, timeout);
        }

        /// <summary>
        /// Pings the ip address asynchronous.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>Task&lt;PingReply&gt;.</returns>
        /// <autogeneratedoc />
        public Task<PingReply> PingIpAddressAsync(IPAddress ipAddress) => _pingTask.PingIpAddressAsync(ipAddress);

        /// <summary>
        /// Pings the reply logging event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PingReplyEventArgs"/> instance containing the event data.</param>
        /// <autogeneratedoc />
        [ExcludeFromCodeCoverage]
        private void PingReplyLoggingEventHandler(object sender, PingReplyEventArgs args)
        {
            if (args == null || !EnableLogging) return;

            using (Logger?.BeginScope("[{Timestamp}:{JobId}:{TaskId}]", args.PingJob.Timestamp,
                args.PingJob.JobGuid, args.PingJob.TaskId))
            {
                var pingReply = args.PingReply;

                if (pingReply.Status == IPStatus.Success)
                {
                    using (Logger?.PushLogProperties(
                        pingReply.GetLogPropertyListLevel(LogLevel.Information, LoggableFormat.ToLog)))
                    {
                        Logger?.LogInformation("Ping {Status} to {IPAddressSubnet}", args.PingReply.Status,
                            args.PingJob.IPAddressSubnet);
                    }
                }
                else if (pingReply.Status == IPStatus.TimedOut)
                {
                    using (Logger?.PushLogProperties(
                        pingReply.GetLogPropertyListLevel(LogLevel.Trace, LoggableFormat.ToLog)))
                    {
                        Logger?.LogTrace("Ping {Status} for {IPAddressSubnet}", args.PingReply.Status,
                            args.PingJob.IPAddressSubnet);
                    }
                }
                else
                {
                    using (Logger?.PushLogProperties(
                        pingReply.GetLogPropertyListLevel(LogLevel.Debug, LoggableFormat.ToLog)))
                    {
                        Logger?.LogDebug("Ping {Status} for {IPAddressSubnet}", args.PingReply.Status,
                            args.PingJob.IPAddressSubnet);
                    }
                }
            }
        }

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The
        /// implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when
        /// <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is
        /// called.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.</returns>
        /// <autogeneratedoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var iPAddressSubnet = IPAddressExtensions.GetDefaultGatewayAddressSubnet(Logger);

                    await _pingTask.PingNetworkAsync(iPAddressSubnet, stoppingToken, DefaultNetworkTimeout,
                        new PingOptions(DefaultTtl, true), int.MaxValue, 0, true);
                }
            }, stoppingToken);
        }
    }
}