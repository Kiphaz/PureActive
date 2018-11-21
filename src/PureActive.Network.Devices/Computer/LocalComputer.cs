﻿// ***********************************************************************
// Assembly         : PureActive.Network.Devices
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="LocalComputer.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using PureActive.Network.Abstractions.CommonNetworkServices;
using PureActive.Network.Abstractions.Local;
using PureActive.Network.Abstractions.Network;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Network;

namespace PureActive.Network.Devices.Computer
{
    /// <summary>
    /// Class LocalComputer.
    /// Implements the <see cref="PureActive.Network.Devices.Computer.ComputerBase" />
    /// Implements the <see cref="PureActive.Network.Abstractions.Local.ILocalComputer" />
    /// </summary>
    /// <seealso cref="PureActive.Network.Devices.Computer.ComputerBase" />
    /// <seealso cref="PureActive.Network.Abstractions.Local.ILocalComputer" />
    /// <autogeneratedoc />
    public class LocalComputer : ComputerBase, ILocalComputer
    {
        /// <summary>
        /// The local network collection
        /// </summary>
        /// <autogeneratedoc />
        private readonly LocalNetworkCollection _localNetworkCollection;

        /// <summary>
        /// The network adapter collection
        /// </summary>
        /// <autogeneratedoc />
        private readonly NetworkAdapterCollection _networkAdapterCollection;
        /// <summary>
        /// The is initialized
        /// </summary>
        /// <autogeneratedoc />
        private bool _isInitialized;


        /// <summary>
        /// Initializes a new instance of the <see cref="LocalComputer"/> class.
        /// </summary>
        /// <param name="commonNetworkServices">The common network services.</param>
        /// <param name="deviceType">Type of the device.</param>
        /// <autogeneratedoc />
        public LocalComputer(ICommonNetworkServices commonNetworkServices, DeviceType deviceType)
            : base(commonNetworkServices, deviceType)
        {
            _networkAdapterCollection = new NetworkAdapterCollection();
            _localNetworkCollection = new LocalNetworkCollection(commonNetworkServices);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is monitoring.
        /// </summary>
        /// <value><c>true</c> if this instance is monitoring; otherwise, <c>false</c>.</value>
        /// <autogeneratedoc />
        public bool IsMonitoring { get; internal set; }

        /// <summary>
        /// Gets the primary network.
        /// </summary>
        /// <value>The primary network.</value>
        /// <autogeneratedoc />
        public INetwork PrimaryNetwork
        {
            get
            {
                if (!_isInitialized)
                {
                    // Initialize NetworkAdapters which will populate Local Networks
                    var networkAdapter = NetworkAdapters;

                    if (networkAdapter != null)
                    {
                        Logger.LogInformation("NetworkAdapters Initialized");
                    }
                }

                return _localNetworkCollection.PrimaryNetwork;
            }
        }

        /// <summary>
        /// Gets the local networks.
        /// </summary>
        /// <value>The local networks.</value>
        /// <autogeneratedoc />
        public ILocalNetworkCollection LocalNetworks => _localNetworkCollection;

        /// <summary>
        /// Gets the network adapters.
        /// </summary>
        /// <value>The network adapters.</value>
        /// <autogeneratedoc />
        public INetworkAdapterCollection NetworkAdapters
        {
            get
            {
                if (_isInitialized) return _networkAdapterCollection;

                foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    var networkAdapter = new NetworkAdapter(CommonNetworkServices, networkInterface);

                    _localNetworkCollection.AddAdapterToNetwork(networkAdapter);
                    _networkAdapterCollection.Add(networkAdapter);
                }

                _isInitialized = true;

                return _networkAdapterCollection;
            }
        }

        /// <summary>
        /// Starts the network event monitor.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public bool StartNetworkEventMonitor()
        {
            if (!IsMonitoring)
            {
                IsMonitoring = true;
                NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
                NetworkChange.NetworkAddressChanged += NetworkAddressChanged;
            }

            return IsMonitoring;
        }

        /// <summary>
        /// Stops the network event monitor.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public bool StopNetworkEventMonitor()
        {
            if (IsMonitoring)
            {
                IsMonitoring = false;
                NetworkChange.NetworkAvailabilityChanged -= NetworkAvailabilityChanged;
                NetworkChange.NetworkAddressChanged -= NetworkAddressChanged;
            }

            return IsMonitoring;
        }


        /// <summary>
        /// Networks the availability changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetworkAvailabilityEventArgs"/> instance containing the event data.</param>
        /// <autogeneratedoc />
        public static void NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
        }

        /// <summary>
        /// Networks the address changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <autogeneratedoc />
        public static void NetworkAddressChanged(object sender, EventArgs e)
        {
        }
    }
}