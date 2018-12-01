﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="IPAddressExtensions.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Net;

namespace PureActive.Hosting.Abstractions.Extensions
{
    /// <summary>
    /// Class IPAddressExtensions.
    /// </summary>
    /// <autogeneratedoc />
    public static class IPAddressExtensions
    {
        /// <summary>
        /// The string subnet class a
        /// </summary>
        /// <autogeneratedoc />
        public const string StringSubnetClassA = "255.0.0.0";
        /// <summary>
        /// The string subnet class b
        /// </summary>
        /// <autogeneratedoc />
        public const string StringSubnetClassB = "255.255.0.0";
        /// <summary>
        /// The string subnet class c
        /// </summary>
        /// <autogeneratedoc />
        public const string StringSubnetClassC = "255.255.255.0";
        /// <summary>
        /// The subnet class a
        /// </summary>
        /// <autogeneratedoc />
        public static readonly IPAddress SubnetClassA = IPAddress.Parse(StringSubnetClassA);
        /// <summary>
        /// The subnet class b
        /// </summary>
        /// <autogeneratedoc />
        public static readonly IPAddress SubnetClassB = IPAddress.Parse(StringSubnetClassB);
        /// <summary>
        /// The subnet class c
        /// </summary>
        /// <autogeneratedoc />
        public static readonly IPAddress SubnetClassC = IPAddress.Parse(StringSubnetClassC);

        /// <summary>
        /// Google Public DNS Server
        /// </summary>
        /// <autogeneratedoc />
        public static readonly IPAddress GooglePublicDnsServerAddress = IPAddress.Parse("8.8.8.8");


        /// <summary>
        /// Returns broadcast address given an IPAddress and subnet Mask
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        /// <returns>IPAddress.</returns>
        /// <exception cref="ArgumentException">Lengths of IP address and subnet mask do not match!</exception>
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            var ipAddressBytes = address.GetAddressBytes();
            var subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAddressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match!");

            var broadcastAddress = new byte[ipAddressBytes.Length];

            for (var i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte) (ipAddressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }

            return new IPAddress(broadcastAddress);
        }

        /// <summary>
        /// Gets the broadcast address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IPAddress.</returns>
        /// <autogeneratedoc />
        public static IPAddress GetBroadcastAddress(this IPAddress address) =>
            GetBroadcastAddress(address, SubnetClassC);


        /// <summary>
        /// Returns Network Address given and IPAddress and subnetMask. Used to compare
        /// two IPAddresses to see if they are on the same subnet
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        /// <returns>IPAddress.</returns>
        /// <exception cref="ArgumentException">Lengths of IP address and subnet mask do not match!</exception>
        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            var ipAddressBytes = address.GetAddressBytes();
            var subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAddressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match!");

            var networkAddress = new byte[ipAddressBytes.Length];

            for (var i = 0; i < networkAddress.Length; i++)
            {
                networkAddress[i] = (byte) (ipAddressBytes[i] & subnetMaskBytes[i]);
            }

            return new IPAddress(networkAddress);
        }

        /// <summary>
        /// Gets the network address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IPAddress.</returns>
        /// <autogeneratedoc />
        public static IPAddress GetNetworkAddress(this IPAddress address) => GetNetworkAddress(address, SubnetClassC);

        /// <summary>
        /// Determines whether [is address on same subnet] [the specified address].
        /// </summary>
        /// <param name="address2">The address2.</param>
        /// <param name="address">The address.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        /// <returns><c>true</c> if [is address on same subnet] [the specified address]; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        public static bool IsAddressOnSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
        {
            if (address2 == null) throw new ArgumentNullException(nameof(address2));
            if (address == null) throw new ArgumentNullException(nameof(address));
            if (subnetMask == null) throw new ArgumentNullException(nameof(subnetMask));

            IPAddress network1 = address.GetNetworkAddress(subnetMask);
            IPAddress network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }

        /// <summary>
        /// Converts to long.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>System.Int64.</returns>
        /// <autogeneratedoc />
        public static long ToLong(this IPAddress address) => BitConverter.ToUInt32(address.GetAddressBytes(), 0);

        /// <summary>
        /// Converts to longbackwards.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>System.Int64.</returns>
        /// <autogeneratedoc />
        public static long ToLongBackwards(this IPAddress address)
        {
            byte[] byteIP = address.GetAddressBytes();

            uint ip = (uint) byteIP[0] << 24;
            ip += (uint) byteIP[1] << 16;
            ip += (uint) byteIP[2] << 8;
            ip += byteIP[3];

            return ip;
        }

        /// <summary>
        /// Increments the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IPAddress.</returns>
        /// <autogeneratedoc />
        public static IPAddress Increment(this IPAddress address)
        {
            byte[] bytes = address.GetAddressBytes();

            for (int k = bytes.Length - 1; k >= 0; k--)
            {
                if (bytes[k] == byte.MaxValue)
                {
                    bytes[k] = 0;
                    continue;
                }

                bytes[k]++;

                return new IPAddress(bytes);
            }

            // Un-incrementable, return the original address.
            return address;
        }


        /// <summary>
        /// Decrements the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IPAddress.</returns>
        /// <autogeneratedoc />
        public static IPAddress Decrement(this IPAddress address)
        {
            byte[] bytes = address.GetAddressBytes();

            for (int k = bytes.Length - 1; k >= 0; k--)
            {
                if (bytes[k] == byte.MinValue)
                {
                    bytes[k] = byte.MaxValue;
                    continue;
                }

                bytes[k]--;

                return new IPAddress(bytes);
            }

            // Un-decrementable, return the original address.
            return address;
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>System.Int32.</returns>
        /// <autogeneratedoc />
        public static int CompareTo(this IPAddress x, IPAddress y)
        {
            var result = x.AddressFamily.CompareTo(y.AddressFamily);

            if (result != 0)
                return result;

            var xBytes = x.GetAddressBytes();
            var yBytes = y.GetAddressBytes();

            var octets = Math.Min(xBytes.Length, yBytes.Length);

            for (var i = 0; i < octets; i++)
            {
                var octetResult = xBytes[i].CompareTo(yBytes[i]);
                if (octetResult != 0)
                    return octetResult;
            }

            return 0;
        }
    }
}