﻿// ***********************************************************************
// Assembly         : PureActive.Network.Services.PingService
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="PingTask.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PureActive.Core.Extensions;
using PureActive.Hosting.Abstractions.System;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Network.Abstractions.PingService;
using PureActive.Network.Abstractions.PingService.Events;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Extensions.Network;

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
    public partial class PingService
    {
        /// <summary>
        /// Class PingTaskImpl.
        /// Implements the <see cref="IPingTask" />
        /// </summary>
        /// <seealso cref="IPingTask" />
        /// <autogeneratedoc />
        public class PingTaskImpl : IPingTask
        {
            /// <summary>
            /// The default data buffer
            /// </summary>
            /// <autogeneratedoc />
            private const string DefaultDataBuffer = "abcdefghijklmnopqrstuvwxyz012345";

            /// <summary>
            /// The windows default timeout
            /// </summary>
            /// <autogeneratedoc />
            private const int
                WindowsDefaultTimeout = 5000; // Wait a default of 5 seconds for a reply (same as Windows Ping)

            /// <summary>
            /// The logger
            /// </summary>
            /// <autogeneratedoc />
            private readonly IPureLogger _logger;

            /// <summary>
            /// The ping
            /// </summary>
            /// <autogeneratedoc />
            private readonly Ping _ping;

            /// <summary>
            /// The ping options
            /// </summary>
            /// <autogeneratedoc />
            private readonly PingOptions _pingOptions;

            /// <summary>
            /// The semaphore slim
            /// </summary>
            /// <autogeneratedoc />
            private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

            /// <summary>
            /// Initializes a new instance of the <see cref="PingTaskImpl"/> class.
            /// </summary>
            /// <param name="commonServices">The common services.</param>
            /// <param name="logger">The logger.</param>
            /// <exception cref="ArgumentNullException">commonServices</exception>
            /// <autogeneratedoc />
            public PingTaskImpl(ICommonServices commonServices, IPureLogger<PingTaskImpl> logger = null)
            {
                if (commonServices == null) throw new ArgumentNullException(nameof(commonServices));
                _ping = new Ping();
                _pingOptions = new PingOptions(64, true);

                _logger = logger ?? commonServices.LoggerFactory.CreatePureLogger<PingTaskImpl>();
            }

            /// <summary>
            /// Gets the default timeout.
            /// </summary>
            /// <value>The default timeout.</value>
            /// <autogeneratedoc />
            public int DefaultTimeout => WindowsDefaultTimeout;

            /// <summary>
            /// Occurs when [on ping reply].
            /// </summary>
            /// <autogeneratedoc />
            public event PingReplyEventHandler OnPingReply;

            /// <summary>
            /// Gets or sets the timeout.
            /// </summary>
            /// <value>The timeout.</value>
            /// <autogeneratedoc />
            public int Timeout { get; set; } = WindowsDefaultTimeout;
            /// <summary>
            /// Gets or sets the wait between pings.
            /// </summary>
            /// <value>The wait between pings.</value>
            /// <autogeneratedoc />
            public int WaitBetweenPings { get; set; } = 0; // No wait between pings

            /// <summary>
            /// Gets or sets the TTL.
            /// </summary>
            /// <value>The TTL.</value>
            /// <autogeneratedoc />
            public int Ttl
            {
                get => _pingOptions.Ttl;
                set => _pingOptions.Ttl = value;
            }

            /// <summary>
            /// Gets or sets a value indicating whether [do not fragment].
            /// </summary>
            /// <value><c>true</c> if [do not fragment]; otherwise, <c>false</c>.</value>
            /// <autogeneratedoc />
            public bool DoNotFragment
            {
                get => _pingOptions.DontFragment;
                set => _pingOptions.DontFragment = value;
            }


            /// <summary>
            /// ping ip address as an asynchronous operation.
            /// </summary>
            /// <param name="ipAddress">The ip address.</param>
            /// <param name="timeout">The timeout.</param>
            /// <param name="buffer">The buffer.</param>
            /// <param name="pingOptions">The ping options.</param>
            /// <returns>Task&lt;PingReply&gt;.</returns>
            /// <autogeneratedoc />
            public async Task<PingReply> PingIpAddressAsync(IPAddress ipAddress, int timeout, byte[] buffer,
                PingOptions pingOptions)
            {
                await _semaphoreSlim.WaitAsync();
                PingReply pingReply = null;

                Task pingTask = Task.Run(async () =>
                {
                    pingReply = await _ping.SendPingAsync(ipAddress, timeout, buffer, pingOptions);
                });

                await pingTask.ContinueWith(t => { _semaphoreSlim.Release(); });

                return pingReply;
            }

            /// <summary>
            /// ping ip address as an asynchronous operation.
            /// </summary>
            /// <param name="ipAddress">The ip address.</param>
            /// <param name="timeout">The timeout.</param>
            /// <returns>Task&lt;PingReply&gt;.</returns>
            /// <autogeneratedoc />
            public async Task<PingReply> PingIpAddressAsync(IPAddress ipAddress, int timeout)
            {
                // Create a buffer of 32 ASCII bytes of data to be transmitted.
                byte[] buffer = Encoding.ASCII.GetBytes(DefaultDataBuffer);

                return await PingIpAddressAsync(ipAddress, timeout, buffer, _pingOptions);
            }

            /// <summary>
            /// ping ip address as an asynchronous operation.
            /// </summary>
            /// <param name="ipAddress">The ip address.</param>
            /// <returns>Task&lt;PingReply&gt;.</returns>
            /// <autogeneratedoc />
            public async Task<PingReply> PingIpAddressAsync(IPAddress ipAddress) =>
                await PingIpAddressAsync(ipAddress, WindowsDefaultTimeout);


            // NOTE: Underlying Ping Service does not support concurrent Pings
            /// <summary>
            /// ping network as an asynchronous operation.
            /// </summary>
            /// <param name="ipAddressSubnet">The ip address subnet.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <param name="timeout">The timeout.</param>
            /// <param name="pingOptions">The ping options.</param>
            /// <param name="pingCallLimit">The ping call limit.</param>
            /// <param name="shuffle">if set to <c>true</c> [shuffle].</param>
            /// <returns>Task.</returns>
            /// <autogeneratedoc />
            public async Task PingNetworkAsync(IPAddressSubnet ipAddressSubnet, CancellationToken cancellationToken,
                int timeout, PingOptions pingOptions, int pingCallLimit = int.MaxValue, bool shuffle = true)
            {
                var pingJob = new PingJob(Guid.NewGuid(), 0, ipAddressSubnet, DateTimeOffset.Now);

                var networkEnumerator = new NetworkEnumerator(ipAddressSubnet).ToList();

                // Shuffle Order of Network Addresses
                if (shuffle)
                    networkEnumerator.Shuffle();

                foreach (var ipAddress in networkEnumerator.Take(pingCallLimit))
                {
                    // Return if cancellation is requested
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _logger.LogInformation("PingNetworkAsync task cancelled");
                        return;
                    }

                    pingJob.NextTask(ipAddress);

                    // Create a buffer of 32 ASCII bytes of data to be transmitted.

                    byte[] buffer = pingJob.ToBuffer();

                    var pingTask = PingIpAddressAsync(ipAddress, timeout, buffer, pingOptions);
                    var pingReply = pingTask.Result;

                    var pingReplyEventArgs = new PingReplyEventArgs(pingJob, pingReply, cancellationToken);

                    OnPingReply?.Invoke(this, pingReplyEventArgs);

                    // Return if cancellation is requested by Events
                    if (pingReplyEventArgs.CancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    if (pingReply.Status != IPStatus.Success) continue;

                    if (WaitBetweenPings > 0)
                    {
                        await Task.Delay(WaitBetweenPings, cancellationToken);
                    }
                }
            }

            /// <summary>
            /// Pings the network asynchronous.
            /// </summary>
            /// <param name="ipAddressSubnet">The ip address subnet.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            /// <param name="timeout">The timeout.</param>
            /// <param name="pingCallLimit">The ping call limit.</param>
            /// <param name="shuffle">if set to <c>true</c> [shuffle].</param>
            /// <returns>Task.</returns>
            /// <autogeneratedoc />
            public Task PingNetworkAsync(IPAddressSubnet ipAddressSubnet, CancellationToken cancellationToken,
                int timeout, int pingCallLimit, bool shuffle)
            {
                return PingNetworkAsync(ipAddressSubnet, cancellationToken, timeout, _pingOptions, pingCallLimit,
                    shuffle);
            }
        }
    }
}