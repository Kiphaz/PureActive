﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="INetwork.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using PureActive.Hosting.Abstractions.Types;

namespace PureActive.Network.Abstractions.Network
{
    /// <summary>
    /// Interface INetwork
    /// </summary>
    /// <autogeneratedoc />
    public interface INetwork
    {
        /// <summary>
        /// Gets the network ip address subnet.
        /// </summary>
        /// <value>The network ip address subnet.</value>
        /// <autogeneratedoc />
        IPAddressSubnet NetworkIPAddressSubnet { get; }
        /// <summary>
        /// Gets the network adapter collection.
        /// </summary>
        /// <value>The network adapter collection.</value>
        /// <autogeneratedoc />
        INetworkAdapterCollection NetworkAdapterCollection { get; }
        /// <summary>
        /// Gets the network gateway.
        /// </summary>
        /// <value>The network gateway.</value>
        /// <autogeneratedoc />
        INetworkGateway NetworkGateway { get; }

        /// <summary>
        /// Gets the adapter count.
        /// </summary>
        /// <value>The adapter count.</value>
        /// <autogeneratedoc />
        int AdapterCount { get; }

        /// <summary>
        /// Adds the adapter to network.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        bool AddAdapterToNetwork(INetworkAdapter networkAdapter);
        /// <summary>
        /// Removes the adapter from network.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        bool RemoveAdapterFromNetwork(INetworkAdapter networkAdapter);
        /// <summary>
        /// Adapters the connected to network.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        bool AdapterConnectedToNetwork(INetworkAdapter networkAdapter);
    }
}