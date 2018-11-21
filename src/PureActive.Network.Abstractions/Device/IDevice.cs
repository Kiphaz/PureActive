﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="IDevice.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using PureActive.Hosting.Abstractions.System;
using PureActive.Network.Abstractions.PureObject;
using PureActive.Network.Abstractions.Types;

namespace PureActive.Network.Abstractions.Device
{
    /// <summary>
    /// Interface IDevice
    /// Implements the <see cref="PureActive.Network.Abstractions.PureObject.IPureObject" />
    /// </summary>
    /// <seealso cref="PureActive.Network.Abstractions.PureObject.IPureObject" />
    /// <autogeneratedoc />
    public interface IDevice : IPureObject
    {
        /// <summary>
        /// Gets the common services.
        /// </summary>
        /// <value>The common services.</value>
        /// <autogeneratedoc />
        ICommonServices CommonServices { get; }
        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        /// <value>The type of the device.</value>
        /// <autogeneratedoc />
        DeviceType DeviceType { get; set; }
    }
}