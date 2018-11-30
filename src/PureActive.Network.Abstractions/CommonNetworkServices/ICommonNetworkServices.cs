﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ICommonNetworkServices.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.ArpService;
using PureActive.Network.Abstractions.Networking;
using PureActive.Network.Abstractions.PingService;

namespace PureActive.Network.Abstractions.CommonNetworkServices
{
    /// <inheritdoc />
    /// <summary>
    /// Interface ICommonNetworkServices
    /// Implements the <see cref="T:PureActive.Hosting.Abstractions.System.ICommonServices" />
    /// </summary>
    /// <seealso cref="T:PureActive.Hosting.Abstractions.System.ICommonServices" />
    /// <autogeneratedoc />
    public interface ICommonNetworkServices : ICommonServices
    {
        /// <summary>
        /// Gets the common services.
        /// </summary>
        /// <value>The common services.</value>
        /// <autogeneratedoc />
        ICommonServices CommonServices { get; }

        /// <summary>
        /// Gets the ping service.
        /// </summary>
        /// <value>The ping service.</value>
        /// <autogeneratedoc />
        IPingService PingService { get; }
        /// <summary>
        /// Gets the arp service.
        /// </summary>
        /// <value>The arp service.</value>
        /// <autogeneratedoc />
        IArpService ArpService { get; }

        /// <summary>
        /// Gets the networking service.
        /// </summary>
        /// <value>The networking service interface.</value>
        /// <autogeneratedoc />
        INetworkingService NetworkingService { get; }
    }
}