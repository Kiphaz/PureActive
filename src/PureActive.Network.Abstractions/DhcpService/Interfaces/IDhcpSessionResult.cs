﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="IDhcpSessionResult.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Net.NetworkInformation;
using PureActive.Network.Abstractions.DeviceInfo;
using PureActive.Network.Abstractions.DhcpService.Types;

namespace PureActive.Network.Abstractions.DhcpService.Interfaces
{
    /// <summary>
    /// Interface IDhcpSessionResult
    /// </summary>
    /// <autogeneratedoc />
    public interface IDhcpSessionResult
    {
        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>The session identifier.</value>
        /// <autogeneratedoc />
        uint SessionId { get; set; }

        /// <summary>
        /// Gets or sets the DHCP session state current.
        /// </summary>
        /// <value>The DHCP session state current.</value>
        /// <autogeneratedoc />
        DhcpSessionState DhcpSessionStateCurrent { get; set; }
        /// <summary>
        /// Gets or sets the DHCP session state start.
        /// </summary>
        /// <value>The DHCP session state start.</value>
        /// <autogeneratedoc />
        DhcpSessionState DhcpSessionStateStart { get; set; }

        /// <summary>
        /// Gets the DHCP discovered device.
        /// </summary>
        /// <value>The DHCP discovered device.</value>
        /// <autogeneratedoc />
        IDhcpDiscoveredDevice DhcpDiscoveredDevice { get; }

        /// <summary>
        /// Gets the device information.
        /// </summary>
        /// <value>The device information.</value>
        /// <autogeneratedoc />
        IDeviceInfo DeviceInfo { get; }

        /// <summary>
        /// Gets the network device information.
        /// </summary>
        /// <value>The network device information.</value>
        /// <autogeneratedoc />
        INetworkDeviceInfo NetworkDeviceInfo { get; }


        /// <summary>
        /// Updates the state of the session.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="dhcpSessionState">State of the DHCP session.</param>
        /// <param name="physicalAddress">The physical address.</param>
        /// <autogeneratedoc />
        void UpdateSessionState(uint sessionId, DhcpSessionState dhcpSessionState, PhysicalAddress physicalAddress);

        /// <summary>
        /// Determines whether [is full session].
        /// </summary>
        /// <returns><c>true</c> if [is full session]; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        bool IsFullSession();
        /// <summary>
        /// Determines whether [is current session] [the specified session identifier].
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns><c>true</c> if [is current session] [the specified session identifier]; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        bool IsCurrentSession(uint sessionId);

        /// <summary>
        /// Determines whether [is duplicate request] [the specified DHCP message].
        /// </summary>
        /// <param name="dhcpMessage">The DHCP message.</param>
        /// <returns><c>true</c> if [is duplicate request] [the specified DHCP message]; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        bool IsDuplicateRequest(IDhcpMessage dhcpMessage);
    }
}