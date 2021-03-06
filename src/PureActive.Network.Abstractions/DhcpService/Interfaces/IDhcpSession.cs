﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-05-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="IDhcpSession.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using PureActive.Network.Abstractions.DhcpService.Types;

namespace PureActive.Network.Abstractions.DhcpService.Interfaces
{
    /// <summary>
    /// Interface IDhcpSession
    /// </summary>
    /// <autogeneratedoc />
    public interface IDhcpSession
    {
        /// <summary>
        /// Gets or sets the state of the request.
        /// </summary>
        /// <value>The state of the request.</value>
        /// <autogeneratedoc />
        RequestState RequestState { get; set; }

        /// <summary>
        /// Gets or sets the state of the DHCP session.
        /// </summary>
        /// <value>The state of the DHCP session.</value>
        /// <autogeneratedoc />
        DhcpSessionState DhcpSessionState { get; set; }


        /// <summary>
        /// Gets the created timestamp.
        /// </summary>
        /// <value>The created timestamp.</value>
        /// <autogeneratedoc />
        DateTimeOffset CreatedTimestamp { get; }
        /// <summary>
        /// Gets or sets the updated timestamp.
        /// </summary>
        /// <value>The updated timestamp.</value>
        /// <autogeneratedoc />
        DateTimeOffset UpdatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the session time out.
        /// </summary>
        /// <value>The session time out.</value>
        /// <autogeneratedoc />
        TimeSpan SessionTimeOut { get; set; }

        /// <summary>
        /// Gets the DHCP discovered device.
        /// </summary>
        /// <value>The DHCP discovered device.</value>
        /// <autogeneratedoc />
        IDhcpDiscoveredDevice DhcpDiscoveredDevice { get; }

        /// <summary>
        /// Updates the timestamp.
        /// </summary>
        /// <returns>DateTimeOffset.</returns>
        /// <autogeneratedoc />
        DateTimeOffset UpdateTimestamp();

        /// <summary>
        /// Determines whether [has session expired] [the specified time stamp].
        /// </summary>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="timeSpan">The time span.</param>
        /// <returns><c>true</c> if [has session expired] [the specified time stamp]; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        bool HasSessionExpired(DateTimeOffset timeStamp, TimeSpan timeSpan);

        /// <summary>
        /// Determines whether [has session expired].
        /// </summary>
        /// <returns><c>true</c> if [has session expired]; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        bool HasSessionExpired();

        /// <summary>
        /// Processes the discover.
        /// </summary>
        /// <param name="dhcpMessage">The DHCP message.</param>
        /// <returns>DhcpMessageProcessed.</returns>
        /// <autogeneratedoc />
        DhcpMessageProcessed ProcessDiscover(IDhcpMessage dhcpMessage);


        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="dhcpMessage">The DHCP message.</param>
        /// <returns>DhcpMessageProcessed.</returns>
        /// <autogeneratedoc />
        DhcpMessageProcessed ProcessRequest(IDhcpMessage dhcpMessage);


        /// <summary>
        /// Processes the decline.
        /// </summary>
        /// <param name="dhcpMessage">The DHCP message.</param>
        /// <returns>DhcpMessageProcessed.</returns>
        /// <autogeneratedoc />
        DhcpMessageProcessed ProcessDecline(IDhcpMessage dhcpMessage);


        /// <summary>
        /// Processes the release.
        /// </summary>
        /// <param name="dhcpMessage">The DHCP message.</param>
        /// <returns>DhcpMessageProcessed.</returns>
        /// <autogeneratedoc />
        DhcpMessageProcessed ProcessRelease(IDhcpMessage dhcpMessage);


        /// <summary>
        /// Processes the inform.
        /// </summary>
        /// <param name="dhcpMessage">The DHCP message.</param>
        /// <returns>DhcpMessageProcessed.</returns>
        /// <autogeneratedoc />
        DhcpMessageProcessed ProcessInform(IDhcpMessage dhcpMessage);


        /// <summary>
        /// Processes the ack.
        /// </summary>
        /// <param name="dhcpMessage">The DHCP message.</param>
        /// <returns>DhcpMessageProcessed.</returns>
        /// <autogeneratedoc />
        DhcpMessageProcessed ProcessAck(IDhcpMessage dhcpMessage);
    }
}