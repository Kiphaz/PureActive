﻿// ***********************************************************************
// Assembly         : PureActive.Network.Services.DhcpService
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="DhcpService.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.Types;
using PureActive.Hosting.Hosting;
using PureActive.Logging.Extensions.Extensions;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.DhcpService.Events;
using PureActive.Network.Abstractions.DhcpService.Interfaces;
using PureActive.Network.Abstractions.DhcpService.Types;
using PureActive.Network.Abstractions.Extensions;
using PureActive.Network.Core.Sockets;
using PureActive.Network.Services.DhcpService.Events;
using PureActive.Network.Services.DhcpService.Session;

namespace PureActive.Network.Services.DhcpService
{
    /// <summary>
    /// Class DhcpService.
    /// Implements the <see cref="Hosting.Hosting.HostedServiceInternal{DhcpService}" />
    /// Implements the <see cref="IDhcpService" />
    /// Implements the <see cref="IDhcpSessionMgr" />
    /// </summary>
    /// <seealso cref="Hosting.Hosting.HostedServiceInternal{DhcpService}" />
    /// <seealso cref="IDhcpService" />
    /// <seealso cref="IDhcpSessionMgr" />
    /// <autogeneratedoc />
    public class DhcpService : HostedServiceInternal<DhcpService>, IDhcpService, IDhcpSessionMgr
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpService"/> class.
        /// </summary>
        /// <param name="commonNetworkServices">The common network services.</param>
        /// <param name="applicationLifetime">The application lifetime.</param>
        /// <exception cref="ArgumentNullException">commonNetworkServices</exception>
        /// <autogeneratedoc />
        public DhcpService(ICommonNetworkServices commonNetworkServices,
            IApplicationLifetime applicationLifetime = null) :
            base(commonNetworkServices?.CommonServices, applicationLifetime, ServiceHost.DhcpService)
        {
            CommonNetworkServices =
                commonNetworkServices ?? throw new ArgumentNullException(nameof(commonNetworkServices));
            _hostedSocketService =
                new HostedSocketService(commonNetworkServices.NetworkingService, commonNetworkServices.CommonServices?.LoggerFactory?.CreatePureLogger<SocketService>());
        }

        /// <summary>
        /// Gets the common network services.
        /// </summary>
        /// <value>The common network services.</value>
        /// <autogeneratedoc />
        public ICommonNetworkServices CommonNetworkServices { get; }

        #region Private Properties

        /// <summary>
        /// The hosted socket service
        /// </summary>
        /// <autogeneratedoc />
        private readonly HostedSocketService _hostedSocketService;

        /// <summary>
        /// The listener DHCP server
        /// </summary>
        /// <autogeneratedoc />
        private UdpListener _listenerDhcpServer;

        /// <summary>
        /// The DHCP sessions
        /// </summary>
        /// <autogeneratedoc />
        private readonly Dictionary<PhysicalAddress, DhcpSession> _dhcpSessions =
            new Dictionary<PhysicalAddress, DhcpSession>();

        /// <summary>
        /// The sessions lock
        /// </summary>
        /// <autogeneratedoc />
        private readonly object _sessionsLock = new object();

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        /// Interface IP address.
        /// </summary>
        /// <value>The interface address.</value>
        public IPAddress InterfaceAddress
        {
            get => _hostedSocketService.InterfaceAddress;
            set => _hostedSocketService.InterfaceAddress = value;
        }

        /// <summary>
        /// Gets the DHCP session MGR.
        /// </summary>
        /// <value>The DHCP session MGR.</value>
        /// <autogeneratedoc />
        public IDhcpSessionMgr DhcpSessionMgr => this;

        #endregion Public Properties

        #region Methods

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.StartPending;

            _listenerDhcpServer = new UdpListener(CommonServices.LoggerFactory)
            {
                InterfaceAddress = IPAddress.Any,
                BufferSize = DhcpConstants.DhcpMaxMessageSize,
                ReceiveTimeout = DhcpConstants.DhcpReceiveTimeout,
                SendTimeout = DhcpConstants.DhcpSendTimeout
            };

            _listenerDhcpServer.ClientConnected += OnDhcpServerClientConnect;
            _listenerDhcpServer.ClientDisconnected += OnDhcpServerClientDisconnect;

            var retVal = _listenerDhcpServer.Start(DhcpConstants.DhcpServicePort, true);

            if (retVal)
                Logger?.LogInformation("DhcpService started listening on port {DhcpServicePort}",
                    DhcpConstants.DhcpServicePort);
            else
                Logger?.LogDebug("DhcpService failed to start listening on port {DhcpServicePort}",
                    DhcpConstants.DhcpServicePort);

            ServiceHostStatus = ServiceHostStatus.Running;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns>Task.</returns>
        /// <autogeneratedoc />
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            ServiceHostStatus = ServiceHostStatus.StopPending;

            try
            {
                var retVal = _listenerDhcpServer.Stop();

                if (retVal)
                    Logger?.LogInformation("DhcpService stopped listening on port {DhcpServicePort}",
                        DhcpConstants.DhcpServicePort);
                else
                    Logger?.LogDebug("DhcpService failed to stop listening on port {DhcpServicePort}",
                        DhcpConstants.DhcpServicePort);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "DhcpService failed to stop listening on port {DhcpServicePort}",
                    DhcpConstants.DhcpServicePort);

                return Task.FromException(ex);
            }
            finally
            {
                ServiceHostStatus = ServiceHostStatus.Stopped;
            }
        }

        /// <summary>
        /// Remote client connects and makes a request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ClientConnectedEventArgs"/> instance containing the event data.</param>
        private void OnDhcpServerClientConnect(object sender, ClientConnectedEventArgs args)
        {
            SocketBuffer channelBuffer = args.ChannelBuffer;

            if (channelBuffer != null
                && args.Channel.IsConnected
                && channelBuffer.BytesTransferred >= DhcpConstants.DhcpMinMessageSize
                && channelBuffer.BytesTransferred <= DhcpConstants.DhcpMaxMessageSize)
            {
                Logger?.LogTrace(
                    "DHCP PACKET received on {LocalEndPoint} with channel id {ChannelId} was received from {RemoteEndPoint} and queued for processing...",
                    args.Channel.Socket.LocalEndPoint,
                    args.Channel.ChannelId,
                    args.Channel.RemoteEndpoint);

                DhcpMessageEventArgs messageArgs = new DhcpMessageEventArgs(this, args.Channel, args.ChannelBuffer);
                OnDhcpMessageReceived(this, messageArgs);

                ThreadPool.QueueUserWorkItem(ProcessRequest, messageArgs);
            }
        }


        /// <summary>
        /// Gets the <see cref="DhcpSession"/> with the specified physical address.
        /// </summary>
        /// <param name="physicalAddress">The physical address.</param>
        /// <returns>DhcpSession.</returns>
        /// <autogeneratedoc />
        public DhcpSession this[PhysicalAddress physicalAddress]
        {
            get
            {
                lock (_sessionsLock)
                {
                    return _dhcpSessions[physicalAddress];
                }
            }
        }

        /// <summary>
        /// Finds the or create DHCP session.
        /// </summary>
        /// <param name="physicalAddress">The physical address.</param>
        /// <returns>IDhcpSession.</returns>
        /// <autogeneratedoc />
        public IDhcpSession FindOrCreateDhcpSession(PhysicalAddress physicalAddress)
        {
            lock (_sessionsLock)
            {
                if (_dhcpSessions.TryGetValue(physicalAddress, out var dhcpSession)) return dhcpSession;

                dhcpSession = new DhcpSession(this, physicalAddress);
                _dhcpSessions.Add(physicalAddress, dhcpSession);

                return dhcpSession;
            }
        }


        /// <summary>
        /// Remote client is disconnected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ClientDisconnectedEventArgs"/> instance containing the event data.</param>
        private void OnDhcpServerClientDisconnect(object sender, ClientDisconnectedEventArgs args)
        {
            Logger?.LogDebug(args.Exception, "DhcpServer client was disconnected");
        }

        /// <summary>
        /// Logs the level from DHCP message processed.
        /// </summary>
        /// <param name="dhcpMessageProcessed">The DHCP message processed.</param>
        /// <returns>LogLevel.</returns>
        /// <autogeneratedoc />
        private LogLevel LogLevelFromDhcpMessageProcessed(DhcpMessageProcessed dhcpMessageProcessed)
        {
            switch (dhcpMessageProcessed)
            {
                case DhcpMessageProcessed.Duplicate:
                    break;
                case DhcpMessageProcessed.Success:
                    return LogLevel.Information;
                case DhcpMessageProcessed.Unknown:
                case DhcpMessageProcessed.Ignored:
                case DhcpMessageProcessed.Failed:
                    return LogLevel.Debug;
            }

            return LogLevel.Trace;
        }

        /// <summary>
        /// Process boot REQUEST and send reply back to remote client.
        /// </summary>
        /// <param name="state">The state.</param>
        private void ProcessRequest(object state)
        {
            DhcpMessageEventArgs args = (DhcpMessageEventArgs) state;

            if (args.RequestMessage.Operation == OperationCode.BootRequest ||
                args.RequestMessage.Operation == OperationCode.BootReply)
            {
                DhcpMessageProcessed dhcpMessageProcessed = DhcpMessageProcessed.Unknown;

                var dhcpSession = DhcpSessionMgr?.FindOrCreateDhcpSession(args.RequestMessage.ClientHardwareAddress);

                if (dhcpSession != null)
                {
                    // classify the client message type
                    switch (args.MessageType)
                    {
                        case MessageType.Discover:
                            dhcpMessageProcessed = dhcpSession.ProcessDiscover(args.RequestMessage);
                            break;

                        case MessageType.Request:
                        {
                            dhcpMessageProcessed = dhcpSession.ProcessRequest(args.RequestMessage);

                            if (dhcpMessageProcessed == DhcpMessageProcessed.Success)
                            {
                                var dhcpDiscoveredDevice = dhcpSession.DhcpDiscoveredDevice;

                                if (dhcpDiscoveredDevice != null)
                                {
                                    // Fix up IPAddress by calling ArpService
                                    if (dhcpDiscoveredDevice.IpAddress.Equals(IPAddress.None))
                                    {
                                        dhcpDiscoveredDevice.IpAddress =
                                            dhcpDiscoveredDevice.PhysicalAddress.Equals(PhysicalAddress.None)
                                                ? IPAddress.None
                                                : CommonNetworkServices.ArpService.GetIPAddress(
                                                    dhcpDiscoveredDevice.PhysicalAddress, true);
                                    }

                                    OnDhcpDiscoveredDevice(this,
                                        new DhcpDiscoveredDeviceEvent(this, dhcpDiscoveredDevice));
                                }
                            }

                            break;
                        }

                        case MessageType.Decline:
                            dhcpMessageProcessed = dhcpSession.ProcessDecline(args.RequestMessage);
                            break;

                        case MessageType.Release:
                            dhcpMessageProcessed = dhcpSession.ProcessRelease(args.RequestMessage);
                            break;

                        case MessageType.Inform:
                            dhcpMessageProcessed = dhcpSession.ProcessInform(args.RequestMessage);
                            break;

                        case MessageType.Ack:
                            dhcpMessageProcessed = dhcpSession.ProcessAck(args.RequestMessage);
                            break;

                        case MessageType.Nak:
                        case MessageType.Offer:
                            break;
                    }
                }

                var msgLogLevel = LogLevelFromDhcpMessageProcessed(dhcpMessageProcessed);

                // log that the packet was successfully parsed
                using (args.RequestMessage.PushLogProperties(msgLogLevel))
                {
                    Logger?.Log(msgLogLevel,
                        "DHCP {DhcpMessageType} message with session id {DhcpSessionId} from client {ClientHardwareAddress} with status {DhcpMessageProcessed} on thread #{ThreadId}",
                        args.MessageType, args.RequestMessage.SessionId,
                        args.RequestMessage.ClientHardwareAddress.ToColonString(),
                        DhcpMessageProcessedString.GetName(dhcpMessageProcessed),
                        Thread.CurrentThread.ManagedThreadId);
                }
            }
            else
            {
                using (args.RequestMessage.PushLogProperties(LogLevel.Debug))
                {
                    Logger?.LogDebug("UNKNOWN operation code {OperationCode} received and ignored",
                        args.RequestMessage.Operation);
                }
            }
        }

        #endregion Methods

        #region Events

        /// <summary>
        /// A dhcp message was received and processed.
        /// </summary>
        public event DhcpMessageEventHandler OnDhcpMessageReceived = delegate { };

        /// <summary>
        /// A dhcp message was received and processed.
        /// </summary>
        public event EventHandler<DhcpDiscoveredDeviceEvent> OnDhcpDiscoveredDevice = delegate { };

        #endregion Events
    }
}