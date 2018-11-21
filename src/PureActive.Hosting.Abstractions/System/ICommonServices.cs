﻿// ***********************************************************************
// Assembly         : PureActive.Hosting.Abstractions
// Author           : SteveBu
// Created          : 11-01-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="ICommonServices.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using PureActive.Core.Abstractions.Async;
using PureActive.Core.Abstractions.System;

namespace PureActive.Hosting.Abstractions.System
{
    /// <summary>
    /// Interface ICommonServices
    /// Implements the <see cref="PureActive.Hosting.Abstractions.System.IHostedServiceInternal" />
    /// </summary>
    /// <seealso cref="PureActive.Hosting.Abstractions.System.IHostedServiceInternal" />
    /// <autogeneratedoc />
    public interface ICommonServices : IHostedServiceInternal
    {
        /// <summary>
        /// Gets the process runner.
        /// </summary>
        /// <value>The process runner.</value>
        /// <autogeneratedoc />
        IProcessRunner ProcessRunner { get; }
        /// <summary>
        /// Gets the file system.
        /// </summary>
        /// <value>The file system.</value>
        /// <autogeneratedoc />
        IFileSystem FileSystem { get; }
        /// <summary>
        /// Gets the operation runner.
        /// </summary>
        /// <value>The operation runner.</value>
        /// <autogeneratedoc />
        IOperationRunner OperationRunner { get; }
        /// <summary>
        /// Gets the operating system.
        /// </summary>
        /// <value>The operating system.</value>
        /// <autogeneratedoc />
        IOperatingSystem OperatingSystem { get; }
    }
}