﻿// ***********************************************************************
// Assembly         : PureActive.Hosting
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="CommonServices.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.Async;
using PureActive.Core.Abstractions.System;
using PureActive.Core.Async;
using PureActive.Core.System;
using PureActive.Hosting.Abstractions.Networking;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.Networking;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;

namespace PureActive.Hosting.CommonServices
{
    /// <summary>
    /// Class CommonServices.
    /// Implements the <see cref="PureActive.Logging.Extensions.Types.PureLoggableBase{CommonServices}" />
    /// Implements the <see cref="ICommonServices" />
    /// </summary>
    /// <seealso cref="PureActive.Logging.Extensions.Types.PureLoggableBase{CommonServices}" />
    /// <seealso cref="ICommonServices" />
    /// <autogeneratedoc />
    public class CommonServices : PureLoggableBase<CommonServices>, ICommonServices
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonServices"/> class.
        /// </summary>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="networkingSystem">The networking system.</param>
        /// <param name="operatingSystem">The operating system.</param>
        /// <param name="operationRunner">The operation runner.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <exception cref="ArgumentNullException">
        /// processRunner
        /// or
        /// fileSystem
        /// or
        /// operatingSystem
        /// or
        /// operationRunner
        /// </exception>
        /// <autogeneratedoc />
        public CommonServices(IProcessRunner processRunner, IFileSystem fileSystem, INetworkingSystem networkingSystem, 
            IOperatingSystem operatingSystem, IOperationRunner operationRunner, IPureLoggerFactory loggerFactory) :
            base(loggerFactory)
        {
            NetworkingSystem = networkingSystem ?? throw new ArgumentNullException(nameof(networkingSystem));
            ProcessRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            OperatingSystem = operatingSystem ?? throw new ArgumentNullException(nameof(operatingSystem));
            OperationRunner = operationRunner ?? throw new ArgumentNullException(nameof(operationRunner));
        }

        /// <summary>
        /// Gets the process runner.
        /// </summary>
        /// <value>The process runner.</value>
        /// <autogeneratedoc />
        public IProcessRunner ProcessRunner { get; }
        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>The file system.</value>
        /// <autogeneratedoc />
        public IFileSystem FileSystem { get; }
        /// <summary>
        /// Gets the operation runner.
        /// </summary>
        /// <value>The operation runner.</value>
        /// <autogeneratedoc />
        public IOperationRunner OperationRunner { get; }
        /// <summary>
        /// Gets the operating system.
        /// </summary>
        /// <value>The operating system.</value>
        /// <autogeneratedoc />
        public IOperatingSystem OperatingSystem { get; }

        /// <summary>
        /// Gets the networking system.
        /// </summary>
        /// <value>The networking system.</value>
        /// <autogeneratedoc />
        public INetworkingSystem NetworkingSystem { get; }


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
            ServiceHostStatus = ServiceHostStatus.Running;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        public Task StopAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.Stopped;

            return Task.CompletedTask;
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
                logPropertyLevels?.Add(new PureLogPropertyLevel("CommonServicesHostStatus", ServiceHostStatus,
                    LogLevel.Information));
            }

            return logPropertyLevels;
        }

        /// <summary>
        /// Creates the instance core.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <returns>ICommonServices.</returns>
        /// <exception cref="ArgumentNullException">loggerFactory</exception>
        /// <autogeneratedoc />
        private static ICommonServices CreateInstanceCore(IPureLoggerFactory loggerFactory, IFileSystem fileSystem)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            var processRunner = new ProcessRunner(loggerFactory.CreatePureLogger<ProcessRunner>());
            var operationRunner = new OperationRunner(loggerFactory.CreatePureLogger<OperationRunner>());
            var networkingSystem = new NetworkingSystem(loggerFactory.CreatePureLogger<NetworkingSystem>());

            return new CommonServices(processRunner, fileSystem, networkingSystem, fileSystem.OperatingSystem, operationRunner, loggerFactory);
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="appFolderName">Name of the application folder.</param>
        /// <returns>ICommonServices.</returns>
        /// <autogeneratedoc />
        public static ICommonServices CreateInstance(IPureLoggerFactory loggerFactory, string appFolderName)
        {
            return CreateInstanceCore(loggerFactory, new FileSystem(appFolderName));
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="type">The type.</param>
        /// <returns>ICommonServices.</returns>
        /// <autogeneratedoc />
        public static ICommonServices CreateInstance(IPureLoggerFactory loggerFactory, Type type)
        {
            return CreateInstanceCore(loggerFactory, new FileSystem(type));
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>ICommonServices.</returns>
        /// <autogeneratedoc />
        public static ICommonServices CreateInstance(IPureLoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            return CreateInstanceCore(loggerFactory, new FileSystem(configuration));
        }
    }
}