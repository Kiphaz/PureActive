﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="INetworkMap.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.Local;
using PureActive.Network.Abstractions.NetworkDevice;

namespace PureActive.Network.Abstractions.Network
{
    /// <summary>
    /// Interface INetworkMap
    /// Implements the <see cref="PureActive.Network.Abstractions.NetworkDevice.INetworkDevice" />
    /// Implements the <see cref="PureActive.Hosting.Abstractions.System.IHostedServiceInternal" />
    /// </summary>
    /// <seealso cref="PureActive.Network.Abstractions.NetworkDevice.INetworkDevice" />
    /// <seealso cref="PureActive.Hosting.Abstractions.System.IHostedServiceInternal" />
    /// <autogeneratedoc />
    public interface INetworkMap : INetworkDevice, IHostedServiceInternal
    {
        /// <summary>
        /// Gets the local networks.
        /// </summary>
        /// <value>The local networks.</value>
        /// <autogeneratedoc />
        ILocalNetworkCollection LocalNetworks { get; }
        /// <summary>
        /// Gets the primary network.
        /// </summary>
        /// <value>The primary network.</value>
        /// <autogeneratedoc />
        INetwork PrimaryNetwork { get; }

        /// <summary>
        /// Gets the local network device.
        /// </summary>
        /// <value>The local network device.</value>
        /// <autogeneratedoc />
        ILocalNetworkDevice LocalNetworkDevice { get; }
        /// <summary>
        /// Gets the updated timestamp.
        /// </summary>
        /// <value>The updated timestamp.</value>
        /// <autogeneratedoc />
        DateTimeOffset UpdatedTimestamp { get; }

        /// <summary>
        /// Updates the timestamp.
        /// </summary>
        /// <returns>DateTimeOffset.</returns>
        /// <autogeneratedoc />
        DateTimeOffset UpdateTimestamp();
    }
}