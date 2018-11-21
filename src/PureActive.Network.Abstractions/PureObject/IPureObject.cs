﻿// ***********************************************************************
// Assembly         : PureActive.Network.Abstractions
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="IPureObject.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using PureActive.Logging.Abstractions.Interfaces;

namespace PureActive.Network.Abstractions.PureObject
{
    /// <summary>
    /// Interface IPureObject
    /// Implements the <see cref="System.IComparable{IPureObject}" />
    /// Implements the <see cref="PureActive.Logging.Abstractions.Interfaces.IPureLoggable" />
    /// Implements the <see cref="PureActive.Network.Abstractions.PureObject.IPureObjectCloneable" />
    /// Implements the <see cref="PureActive.Network.Abstractions.PureObject.IPureObjectTimestamp" />
    /// </summary>
    /// <seealso cref="System.IComparable{IPureObject}" />
    /// <seealso cref="PureActive.Logging.Abstractions.Interfaces.IPureLoggable" />
    /// <seealso cref="PureActive.Network.Abstractions.PureObject.IPureObjectCloneable" />
    /// <seealso cref="PureActive.Network.Abstractions.PureObject.IPureObjectTimestamp" />
    /// <autogeneratedoc />
    public interface IPureObject : IComparable<IPureObject>, IPureLoggable, IPureObjectCloneable, IPureObjectTimestamp
    {
        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>The object identifier.</value>
        /// <autogeneratedoc />
        Guid ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the object version.
        /// </summary>
        /// <value>The object version.</value>
        /// <autogeneratedoc />
        ulong ObjectVersion { get; set; }

        /// <summary>
        /// Determines whether [is same object identifier] [the specified pure object other].
        /// </summary>
        /// <param name="pureObjectOther">The pure object other.</param>
        /// <returns><c>true</c> if [is same object identifier] [the specified pure object other]; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        bool IsSameObjectId(IPureObject pureObjectOther);

        /// <summary>
        /// Determines whether [is same object version] [the specified pure object other].
        /// </summary>
        /// <param name="pureObjectOther">The pure object other.</param>
        /// <returns><c>true</c> if [is same object version] [the specified pure object other]; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        bool IsSameObjectVersion(IPureObject pureObjectOther);
    }
}