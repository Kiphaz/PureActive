﻿// ***********************************************************************
// Assembly         : PureActive.Network.Services.ArpService
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ArpService.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PureActive.Core.Abstractions.System;
using PureActive.Hosting.Abstractions.Extensions;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.Hosting;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Extensions;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Abstractions.ArpService;
using PureActive.Network.Abstractions.PingService;

namespace PureActive.Network.Services.ArpService
{
    /// <summary>
    /// Class ArpService.
    /// Implements the <see cref="Hosting.Hosting.BackgroundServiceInternal{ArpService}" />
    /// Implements the <see cref="IArpService" />
    /// </summary>
    /// <seealso cref="Hosting.Hosting.BackgroundServiceInternal{ArpService}" />
    /// <seealso cref="IArpService" />
    /// <autogeneratedoc />
    public class ArpService : BackgroundServiceInternal<ArpService>, IArpService
    {
        /// <summary>
        /// The number of times we will attempt run arp command
        /// </summary>
        private const int ArpRetryAttempts = 3;

        /// <summary>
        /// The ip address2 arp item
        /// </summary>
        /// <autogeneratedoc />
        private readonly Dictionary<IPAddress, ArpItem> _ipAddress2ArpItem = new Dictionary<IPAddress, ArpItem>();

        /// <summary>
        /// The physical2 arp item
        /// </summary>
        /// <autogeneratedoc />
        private readonly Dictionary<PhysicalAddress, ArpItem> _physical2ArpItem =
            new Dictionary<PhysicalAddress, ArpItem>();

        // ILoggable Interface
        /// <summary>
        /// The ping service
        /// </summary>
        /// <autogeneratedoc />
        private readonly IPingService _pingService;

        /// <summary>
        /// The process exit delay
        /// </summary>
        /// <autogeneratedoc />
        private readonly int _processExitDelay = 100;
        /// <summary>
        /// The retry attempt delay
        /// </summary>
        /// <autogeneratedoc />
        private readonly TimeSpan _retryAttemptDelay = TimeSpan.FromSeconds(1);
        /// <summary>
        /// The update lock
        /// </summary>
        /// <autogeneratedoc />
        private readonly object _updateLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="ArpService"/> class.
        /// </summary>
        /// <param name="commonServices">The common services.</param>
        /// <param name="pingService">The ping service.</param>
        /// <param name="applicationLifetime">The application lifetime.</param>
        /// <autogeneratedoc />
        public ArpService(ICommonServices commonServices, IPingService pingService,
            IApplicationLifetime applicationLifetime = null) :
            base(commonServices, applicationLifetime, ServiceHost.ArpService)
        {
            _pingService = pingService;
        }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        /// <autogeneratedoc />
        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 3, 0); // 3 minutes

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        /// <autogeneratedoc />
        public int Count => _physical2ArpItem.Count;

        /// <summary>
        /// Gets the last arp refresh status.
        /// </summary>
        /// <value>The last arp refresh status.</value>
        /// <autogeneratedoc />
        public ArpRefreshStatus LastArpRefreshStatus { get; internal set; }
        /// <summary>
        /// Gets the last updated.
        /// </summary>
        /// <value>The last updated.</value>
        /// <autogeneratedoc />
        public DateTimeOffset LastUpdated { get; internal set; }

        /// <summary>
        /// Clears the arp cache.
        /// </summary>
        /// <autogeneratedoc />
        public void ClearArpCache()
        {
            lock (_updateLock)
            {
                _physical2ArpItem.Clear();
                _ipAddress2ArpItem.Clear();
            }
        }

        /// <summary>
        /// Refreshes the arp cache asynchronous.
        /// </summary>
        /// <param name="stoppingToken">The stopping token.</param>
        /// <param name="clearCache">if set to <c>true</c> [clear cache].</param>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        public Task RefreshArpCacheAsync(CancellationToken stoppingToken, bool clearCache)
        {
            if (clearCache)
                ClearArpCache();

            return RefreshInternalAsync(Timeout, stoppingToken);
        }

        /// <summary>
        /// Refreshes the arp cache asynchronous.
        /// </summary>
        /// <param name="clearCache">if set to <c>true</c> [clear cache].</param>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        public Task RefreshArpCacheAsync(bool clearCache)
        {
            return RefreshArpCacheAsync(CancellationToken.None, clearCache);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        /// <autogeneratedoc />
        public IEnumerator<ArpItem> GetEnumerator()
        {
            return _ipAddress2ArpItem.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.</returns>
        /// <autogeneratedoc />
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// get arp item as an asynchronous operation.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;ArpItem&gt;.</returns>
        /// <autogeneratedoc />
        public async Task<ArpItem> GetArpItemAsync(IPAddress ipAddress, CancellationToken cancellationToken)
        {
            ArpItem arpItem = null;

            // TODO: Rename to be Sync

            try
            {
                lock (_updateLock)
                {
                    if (_ipAddress2ArpItem.TryGetValue(ipAddress, out arpItem))
                        return arpItem;
                }

                cancellationToken.ThrowIfCancellationRequested();

                // OK if ping fails or times out at it will still populate the arp cache
                await _pingService.PingIpAddressAsync(ipAddress);

                cancellationToken.ThrowIfCancellationRequested();

                await RefreshInternalAsync(Timeout, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                lock (_updateLock)
                {
                    _ipAddress2ArpItem.TryGetValue(ipAddress, out arpItem);
                }
            }
            catch (OperationCanceledException ex)
            {
                Logger.LogDebug(ex, "GetArpItemAsync task cancelled");
            }

            return arpItem;
        }

        /// <summary>
        /// get arp item as an asynchronous operation.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>Task&lt;ArpItem&gt;.</returns>
        /// <autogeneratedoc />
        public async Task<ArpItem> GetArpItemAsync(IPAddress ipAddress) =>
            await GetArpItemAsync(ipAddress, CancellationToken.None);

        /// <summary>
        /// get physical address as an asynchronous operation.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;PhysicalAddress&gt;.</returns>
        /// <autogeneratedoc />
        public async Task<PhysicalAddress> GetPhysicalAddressAsync(IPAddress ipAddress,
            CancellationToken cancellationToken)
        {
            var arpItem = await GetArpItemAsync(ipAddress, cancellationToken);

            return arpItem?.PhysicalAddress ?? PhysicalAddress.None;
        }

        /// <summary>
        /// get physical address as an asynchronous operation.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>Task&lt;PhysicalAddress&gt;.</returns>
        /// <autogeneratedoc />
        public async Task<PhysicalAddress> GetPhysicalAddressAsync(IPAddress ipAddress) =>
            await GetPhysicalAddressAsync(ipAddress, CancellationToken.None);

        /// <summary>
        /// Gets the physical address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>PhysicalAddress.</returns>
        /// <autogeneratedoc />
        public PhysicalAddress GetPhysicalAddress(IPAddress ipAddress) => GetPhysicalAddressAsync(ipAddress).Result;

        /// <summary>
        /// Gets the ip address.
        /// </summary>
        /// <param name="physicalAddress">The physical address.</param>
        /// <param name="refreshCache">if set to <c>true</c> [refresh cache].</param>
        /// <returns>IPAddress.</returns>
        /// <autogeneratedoc />
        public IPAddress GetIPAddress(PhysicalAddress physicalAddress, bool refreshCache)
        {
            if (physicalAddress == null) throw new ArgumentNullException(nameof(physicalAddress));

            lock (_updateLock)
            {
                if (_physical2ArpItem.TryGetValue(physicalAddress, out var arpItem))
                    return arpItem.IPAddress;
            }

            if (!refreshCache) return IPAddress.None;

            RefreshInternalAsync(Timeout, CancellationToken.None).Wait();

            lock (_updateLock)
            {
                if (_physical2ArpItem.TryGetValue(physicalAddress, out var arpItem))
                    return arpItem.IPAddress;
            }
            
            return IPAddress.None;
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
            var logProperties = new List<IPureLogPropertyLevel>
            {
                new PureLogPropertyLevel("DiscoveredDevices", _physical2ArpItem.Count, LogLevel.Information),
                new PureLogPropertyLevel("ArpLastRefreshStatus", LastArpRefreshStatus, LogLevel.Information),
                new PureLogPropertyLevel("ArpLastUpdated", LastUpdated, LogLevel.Information)
            };

            if (logLevel <= LogLevel.Debug)
            {
                lock (_updateLock)
                {
                    var arpItems = this.ToList();

                    foreach (var arpItem in arpItems)
                    {
                        logProperties.Add(new PureLogPropertyLevel(arpItem.IPAddress.ToString(), arpItem.PhysicalAddress.ToDashString(), LogLevel.Information));
                    }
                }
            }

            return logProperties.Where(p => p.MinimumLogLevel.CompareTo(logLevel) >= 0);
        }

        [ExcludeFromCodeCoverage]
        private bool ShouldRetry(Exception ex) => true;

        /// <summary>
        /// run arp as an asynchronous operation.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task&lt;ProcessResult&gt;.</returns>
        /// <autogeneratedoc />
        private async Task<ProcessResult> RunArpAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            ProcessResult processResult;

            try
            {
                var arpCommandPath = CommonServices.FileSystem.ArpCommandPath();
                var args = new[] { "-a" };

                processResult = await CommonServices.OperationRunner.RetryOperationIfNeededAsync(
                    async () => await CommonServices.ProcessRunner.RunProcessAsync(arpCommandPath, args, timeout),
                    ShouldRetry,
                    ArpRetryAttempts,
                    _retryAttemptDelay,
                    false,
                    cancellationToken
                );

                cancellationToken.ThrowIfCancellationRequested();

                if (!processResult.Completed || string.IsNullOrWhiteSpace(processResult.Output))
                {
                    Logger.LogError("RunArpAsync failed to produce output");
                }
            }
            catch (OperationCanceledException ex)
            {
                Logger.LogDebug(ex, "ArpService: RunArpAsync task cancelled");
                processResult = new ProcessResult(false, string.Empty);
            }

            return processResult;
        }

        /// <summary>
        /// Updates the arp item.
        /// </summary>
        /// <param name="physicalAddress">The physical address.</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <returns>ArpItem.</returns>
        /// <autogeneratedoc />
        // ReSharper disable once UnusedMethodReturnValue.Local
        private ArpItem _UpdateArpItem(PhysicalAddress physicalAddress, IPAddress ipAddress, DateTimeOffset timestamp)
        {
            lock (_updateLock)
            {
                if (_physical2ArpItem.TryGetValue(physicalAddress, out var arpItem))
                {
                    // Update IP Address
                    if (!arpItem.IPAddress.Equals(ipAddress))
                    {
                        // Remove old entry and add new
                        _ipAddress2ArpItem.Remove(arpItem.IPAddress);

                        arpItem.IPAddress = ipAddress;
                        _ipAddress2ArpItem.Add(ipAddress, arpItem);
                    }

                    // Update cache value based on this session
                    arpItem.CreatedTimestamp = timestamp;
                }
                else
                {
                    // Create new entry
                    arpItem = new ArpItem(physicalAddress, ipAddress, timestamp);

                    // Add to both Dictionaries
                    _physical2ArpItem.Add(physicalAddress, arpItem);
                    _ipAddress2ArpItem.Add(ipAddress, arpItem);
                }

                return arpItem;
            }
        }

        /// <summary>
        /// Processes the arp output.
        /// </summary>
        /// <param name="arpResults">The arp results.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>ArpRefreshStatus.</returns>
        /// <autogeneratedoc />
        private ArpRefreshStatus _ProcessArpOutput(string arpResults, CancellationToken cancellationToken)
        {
            if (arpResults == null) throw new ArgumentNullException(nameof(arpResults));

            var timestamp = DateTimeOffset.Now;

            Regex regexIpAddress = new Regex(@"\b([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})\b",
                RegexOptions.IgnoreCase);
            Regex regexPhysicalAddress =
                new Regex(@"(?<![\w\-:])[0-9A-F]{1,2}([-:]?)(?:[0-9A-F]{1,2}\1){4}[0-9A-F]{1,2}(?![\w\-:])",
                    RegexOptions.IgnoreCase);

            using (var stringReader = new StringReader(arpResults))
            {
                string inputLine;

                while ((inputLine = stringReader.ReadLine()) != null)
                {
                    if (inputLine.Length == 0)
                        continue;

                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        var ipAddressString = regexIpAddress.Match(inputLine).Value;
                        var physicalAddressString = regexPhysicalAddress.Match(inputLine).Value;

                        if (!string.IsNullOrWhiteSpace(ipAddressString) &&
                            !string.IsNullOrWhiteSpace(physicalAddressString))
                        {
                            var physicalAddress = PhysicalAddressExtensions.NormalizedParse(physicalAddressString);
                            var ipAddress = IPAddress.Parse(ipAddressString);

                            _UpdateArpItem(physicalAddress, ipAddress, timestamp);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger?.LogError(ex, "ArpOutput parsing error with line {ArpOutputLine}", inputLine);
                    }
                }
            }

            return ArpRefreshStatus.Processed;
        }

        // ReSharper disable once UnusedMember.Local
        private ProcessResult RunArpSync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            return RunArpAsync(timeout, cancellationToken).Result;
        }

        /// <summary>
        /// refresh internal as an asynchronous operation.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        private async Task RefreshInternalAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            try
            {
                var arpRefreshStatus = ArpRefreshStatus.Running;

                var processResult = await RunArpAsync(timeout, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                await Task.Delay(_processExitDelay, cancellationToken);

                if (processResult.Completed)
                {
                    arpRefreshStatus = _ProcessArpOutput(processResult.Output, cancellationToken);
                }

                // if Processed succeeded then we're completed
                if (arpRefreshStatus == ArpRefreshStatus.Processed)
                {
                    arpRefreshStatus = ArpRefreshStatus.Completed;
                }

                LastUpdated = DateTimeOffset.Now;
                LastArpRefreshStatus = arpRefreshStatus;

                using (this.PushLogProperties(LogLevel.Debug))
                {
                    Logger?.LogDebug(
                        "{ServiceHost} Refresh Finished with Status: {ArpRefreshStatus}, Devices Discovered: {ArpDeviceCount}",
                        ServiceHost, arpRefreshStatus, Count);
                }
            }
            catch (OperationCanceledException ex)
            {
                Logger.LogDebug(ex, "ArpService: RefreshInternalAsync task cancelled");
            }
        }

        /// <summary>
        /// execute as an asynchronous operation.
        /// </summary>
        /// <param name="stoppingToken">Triggered when
        /// <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is
        /// called.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the long running operations.</returns>
        /// <autogeneratedoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ServiceHostStatus = ServiceHostStatus.StartPending;

            return Task.Run(async () =>
            {
                await RefreshInternalAsync(Timeout, stoppingToken);
                    
                ServiceHostStatus = ServiceHostStatus.Running;
                
                // TODO: Figure out ArpService periodic refresh
                stoppingToken.WaitHandle.WaitOne(-1);

            }, stoppingToken);
        }
    }
}