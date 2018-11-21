﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ILocalComputer.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using PureActive.Network.Abstractions.Computer;

namespace PureActive.Network.Abstractions.Local
{
    /// <summary>
    /// Interface ILocalComputer
    /// Implements the <see cref="PureActive.Network.Abstractions.Computer.IComputer" />
    /// Implements the <see cref="PureActive.Network.Abstractions.Local.ILocalNetworkDevice" />
    /// </summary>
    /// <seealso cref="PureActive.Network.Abstractions.Computer.IComputer" />
    /// <seealso cref="PureActive.Network.Abstractions.Local.ILocalNetworkDevice" />
    /// <autogeneratedoc />
    public interface ILocalComputer : IComputer, ILocalNetworkDevice
    {
    }
}