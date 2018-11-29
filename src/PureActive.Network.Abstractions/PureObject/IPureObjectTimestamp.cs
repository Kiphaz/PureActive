﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-13-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="IPureObjectTimestamp.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace PureActive.Network.Abstractions.PureObject
{
    /// <summary>
    /// Interface IPureObjectTimestamp
    /// </summary>
    /// <autogeneratedoc />
    public interface IPureObjectTimestamp
    {
        /// <summary>
        /// Gets or sets the created timestamp.
        /// </summary>
        /// <value>The created timestamp.</value>
        /// <autogeneratedoc />
        DateTimeOffset CreatedTimestamp { get; }
        /// <summary>
        /// Gets or sets the modified timestamp.
        /// </summary>
        /// <value>The modified timestamp.</value>
        /// <autogeneratedoc />
        DateTimeOffset ModifiedTimestamp { get; set; }
    }
}