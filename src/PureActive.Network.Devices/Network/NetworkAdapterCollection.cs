﻿// ***********************************************************************
// Assembly         : PureActive.Network.Devices
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="NetworkAdapterCollection.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using PureActive.Network.Abstractions.Network;

namespace PureActive.Network.Devices.Network
{
    /// <summary>
    /// Class NetworkAdapterCollection.
    /// Implements the <see cref="INetworkAdapterCollection" />
    /// </summary>
    /// <seealso cref="INetworkAdapterCollection" />
    /// <autogeneratedoc />
    public class NetworkAdapterCollection : INetworkAdapterCollection
    {
        /// <summary>
        /// The network adapters
        /// </summary>
        /// <autogeneratedoc />
        private readonly List<INetworkAdapter> _networkAdapters = new List<INetworkAdapter>();

        /// <summary>
        /// Gets or sets the <see cref="INetworkAdapter"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>INetworkAdapter.</returns>
        /// <autogeneratedoc />
        public INetworkAdapter this[int index]
        {
            get => _networkAdapters[index];
            set => _networkAdapters.Insert(index, value);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        /// <autogeneratedoc />
        public int Count => _networkAdapters?.Count ?? 0;

        /// <summary>
        /// Adds the specified network adapter.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public bool Add(INetworkAdapter networkAdapter)
        {
            if (_networkAdapters.Contains(networkAdapter))
                return false;

            _networkAdapters.Add(networkAdapter);

            return true;
        }

        /// <summary>
        /// Removes the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public bool Remove(int index)
        {
            if (index < 0 && index >= _networkAdapters.Count)
                return false;

            _networkAdapters.RemoveAt(index);

            return true;
        }

        /// <summary>
        /// Removes the specified network adapter.
        /// </summary>
        /// <param name="networkAdapter">The network adapter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        public bool Remove(INetworkAdapter networkAdapter)
        {
            return _networkAdapters.Remove(networkAdapter);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        /// <autogeneratedoc />
        public IEnumerator<INetworkAdapter> GetEnumerator()
        {
            return _networkAdapters.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>IEnumerator.</returns>
        /// <autogeneratedoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}