﻿// ***********************************************************************
// Assembly         : PureActive.Hosting.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ServiceHostStatus.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace PureActive.Hosting.Abstractions.Types
{
    /// <summary>
    /// Enum ServiceHostStatus
    /// </summary>
    /// <autogeneratedoc />
    public enum ServiceHostStatus
    {
        /// <summary>
        /// The unknown
        /// </summary>
        /// <autogeneratedoc />
        Unknown = 0,
        /// <summary>
        /// The continue pending
        /// </summary>
        /// <autogeneratedoc />
        ContinuePending = 5,
        /// <summary>
        /// The paused
        /// </summary>
        /// <autogeneratedoc />
        Paused = 7,
        /// <summary>
        /// The pause pending
        /// </summary>
        /// <autogeneratedoc />
        PausePending = 6,
        /// <summary>
        /// The running
        /// </summary>
        /// <autogeneratedoc />
        Running = 4,
        /// <summary>
        /// The start pending
        /// </summary>
        /// <autogeneratedoc />
        StartPending = 2,
        /// <summary>
        /// The stopped
        /// </summary>
        /// <autogeneratedoc />
        Stopped = 1,
        /// <summary>
        /// The stop pending
        /// </summary>
        /// <autogeneratedoc />
        StopPending = 3
    }
}