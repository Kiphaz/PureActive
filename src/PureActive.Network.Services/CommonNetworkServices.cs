﻿// ***********************************************************************
// Assembly         : PureActive.Network.Services
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="CommonNetworkServices.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.Async;
using PureActive.Core.Abstractions.System;
using PureActive.Hosting.Abstractions.Extensions;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Abstractions.ArpService;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Networking;
using PureActive.Network.Abstractions.PingService;
using PureActive.Network.Services.Networking;

namespace PureActive.Network.Services
{
    /// <summary>
    /// Class CommonNetworkServices.
    /// Implements the <see cref="Logging.Extensions.Types.PureLoggableBase{CommonNetworkServices}" />
    /// Implements the <see cref="ICommonNetworkServices" />
    /// </summary>
    /// <seealso cref="Logging.Extensions.Types.PureLoggableBase{CommonNetworkServices}" />
    /// <seealso cref="ICommonNetworkServices" />
    /// <autogeneratedoc />
    public class CommonNetworkServices : PureLoggableBase<CommonNetworkServices>, ICommonNetworkServices
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonNetworkServices"/> class.
        /// </summary>
        /// <param name="commonServices">The common services.</param>
        /// <param name="pingService">The ping service.</param>
        /// <param name="arpService">The arp service.</param>
        /// <param name="networkingService">the networking service interface</param>
        /// <exception cref="ArgumentNullException">
        /// commonServices
        /// or
        /// pingService
        /// or
        /// arpService
        /// </exception>
        /// <autogeneratedoc />
        public CommonNetworkServices(ICommonServices commonServices, INetworkingService networkingService, IPingService pingService, IArpService arpService) :
            base(commonServices?.LoggerFactory)
        {
            CommonServices = commonServices ?? throw new ArgumentNullException(nameof(commonServices));
            NetworkingService = networkingService ?? throw new ArgumentNullException(nameof(networkingService));
            PingService = pingService ?? throw new ArgumentNullException(nameof(pingService));
            ArpService = arpService ?? throw new ArgumentNullException(nameof(arpService));
        }

        // Implementation of ICommonServices
        /// <summary>
        /// Gets the process runner.
        /// </summary>
        /// <value>The process runner.</value>
        /// <autogeneratedoc />
        public IProcessRunner ProcessRunner => CommonServices?.ProcessRunner;
        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>The file system.</value>
        /// <autogeneratedoc />
        public IFileSystem FileSystem => CommonServices?.FileSystem;
        /// <summary>
        /// Gets the operation runner.
        /// </summary>
        /// <value>The operation runner.</value>
        /// <autogeneratedoc />
        public IOperationRunner OperationRunner => CommonServices?.OperationRunner;
        /// <summary>
        /// Gets the operating system.
        /// </summary>
        /// <value>The operating system.</value>
        /// <autogeneratedoc />
        public IOperatingSystem OperatingSystem => CommonServices?.OperatingSystem;

        /// <summary>
        /// Gets the networking system.
        /// </summary>
        /// <value>The networking system.</value>
        /// <autogeneratedoc />
        public INetworkingService NetworkingService { get; }
    
        // Implementation of ICommonNetworkServices specific items
        /// <summary>
        /// Gets the common services.
        /// </summary>
        /// <value>The common services.</value>
        /// <autogeneratedoc />
        public ICommonServices CommonServices { get; }
        /// <summary>
        /// Gets the ping service.
        /// </summary>
        /// <value>The ping service.</value>
        /// <autogeneratedoc />
        public IPingService PingService { get; }
        /// <summary>
        /// Gets the arp service.
        /// </summary>
        /// <value>The arp service.</value>
        /// <autogeneratedoc />
        public IArpService ArpService { get; }

        /// <summary>
        /// Gets the service host status.
        /// </summary>
        /// <value>The service host status.</value>
        /// <autogeneratedoc />
        public ServiceHostStatus ServiceHostStatus { get; set; } = ServiceHostStatus.Stopped;

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        public Task StartAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.StartPending;
            var tasks = new List<Task>
            {
                CommonServices.StartAsync(cancellationToken),
                PingService.StartAsync(cancellationToken),
                ArpService.StartAsync(cancellationToken)
            };

            return tasks.WaitForTasksAction(cancellationToken,
                (t) =>
                {
                    if (t.IsCompleted && t.Status == TaskStatus.RanToCompletion)
                    {
                        ServiceHostStatus = ServiceHostStatus.Running;
                    }
                },
                Logger);
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>
            {
                ArpService.StopAsync(cancellationToken),
                PingService.StopAsync(cancellationToken),
                CommonServices.StopAsync(cancellationToken)
            };

            ServiceHostStatus = ServiceHostStatus.StopPending;

            var logger = CommonServices?.LoggerFactory?.CreatePureLogger<CommonNetworkServices>();

            return tasks.WaitForTasksAction(cancellationToken, (t) =>
                {
                    if (t.IsCompleted)
                    {
                        ServiceHostStatus = ServiceHostStatus.Stopped;
                    }
                }
            , logger);
        }


        /// <summary>
        /// Gets the log property list level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="loggableFormat">The loggable format.</param>
        /// <returns>IEnumerable&lt;IPureLogPropertyLevel&gt;.</returns>
        /// <autogeneratedoc />
        public override IEnumerable<IPureLogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel,
            LoggableFormat loggableFormat)
        {
            var logPropertyLevels = loggableFormat.IsWithParents()
                ? base.GetLogPropertyListLevel(logLevel, loggableFormat)?.ToList()
                : new List<IPureLogPropertyLevel>();

            if (logLevel <= LogLevel.Information)
            {
                logPropertyLevels?.Add(new PureLogPropertyLevel("CommonNetworkServicesHostStatus", ServiceHostStatus,
                    LogLevel.Information));
            }

            return logPropertyLevels;
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="commonServices">The common services.</param>
        /// <returns>ICommonNetworkServices.</returns>
        /// <exception cref="ArgumentNullException">
        /// loggerFactory
        /// or
        /// commonServices
        /// </exception>
        /// <autogeneratedoc />
        public static ICommonNetworkServices CreateInstance(IPureLoggerFactory loggerFactory,
            ICommonServices commonServices)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            if (commonServices == null) throw new ArgumentNullException(nameof(commonServices));

            // Common Network Services
            var networkingService = new NetworkingService(loggerFactory.CreatePureLogger<NetworkingService>());
            var pingService = new PingService.PingService(commonServices, networkingService);
            var arpService = new ArpService.ArpService(commonServices, pingService);

            return new CommonNetworkServices(commonServices, networkingService, pingService, arpService);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="appName">Name of the application.</param>
        /// <returns>ICommonNetworkServices.</returns>
        /// <autogeneratedoc />
        public static ICommonNetworkServices CreateInstance(IPureLoggerFactory loggerFactory, string appName)
        {
            return CreateInstance(loggerFactory,
                Hosting.CommonServices.CommonServices.CreateInstance(loggerFactory, appName));
        }
    }
}