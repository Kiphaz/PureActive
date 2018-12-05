﻿// ***********************************************************************
// Assembly         : PureActive.Network.Devices
// Author           : SteveBu
// Created          : 11-02-2018
// License          : Licensed under MIT License, see https://github.com/PureActive/PureActive/blob/master/LICENSE
//
// Last Modified By : SteveBu
// Last Modified On : 11-20-2018
// ***********************************************************************
// <copyright file="PureObjectBase.cs" company="BushChang Corporation">
//     © 2018 BushChang Corporation. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Abstractions.PureObject;

namespace PureActive.Network.Devices.PureObject
{
    /// <summary>
    /// Class PureObjectBase.
    /// Implements the <see cref="Logging.Extensions.Types.PureLoggableBase{PureObjectBase}" />
    /// Implements the <see cref="IPureObject" />
    /// Implements the <see cref="System.IEquatable{PureObjectBase}" />
    /// </summary>
    /// <seealso cref="Logging.Extensions.Types.PureLoggableBase{PureObjectBase}" />
    /// <seealso cref="IPureObject" />
    /// <seealso cref="System.IEquatable{PureObjectBase}" />
    /// <autogeneratedoc />
    public abstract class PureObjectBase : PureLoggableBase<PureObjectBase>, IPureObject, IEquatable<PureObjectBase>
    {
        /// <summary>
        /// The object version start
        /// </summary>
        /// <autogeneratedoc />
        protected static readonly ulong ObjectVersionStart = 1;
        /// <summary>
        /// The object version default increment
        /// </summary>
        /// <autogeneratedoc />
        protected static readonly ulong ObjectVersionDefaultIncrement = 1;

        private DateTimeOffset _createdTimestamp;
        private Guid _objectId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PureObjectBase"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">loggerFactory</exception>
        /// <autogeneratedoc />
        protected PureObjectBase(IPureLoggerFactory loggerFactory, IPureLogger logger = null) :
            base(loggerFactory, logger)
        {
            // Handled by base
            // if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

            _createdTimestamp = ModifiedTimestamp = DateTimeOffset.Now;
            _objectId = Guid.NewGuid();
            ObjectVersion = ObjectVersionStart;

            Logger = logger ?? LoggerFactory.CreatePureLogger<PureObjectBase>();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        /// <autogeneratedoc />
        public bool Equals(PureObjectBase other)
        {
            return other != null && ObjectId.Equals(other.ObjectId);
        }

        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>The object identifier.</value>
        /// <autogeneratedoc />
        public Guid ObjectId => _objectId;

        /// <summary>
        /// Gets or sets the created timestamp.
        /// </summary>
        /// <value>The created timestamp.</value>
        /// <autogeneratedoc />
        public DateTimeOffset CreatedTimestamp => _createdTimestamp;

        /// <summary>
        /// Gets or sets the modified timestamp.
        /// </summary>
        /// <value>The modified timestamp.</value>
        /// <autogeneratedoc />
        public DateTimeOffset ModifiedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the object version.
        /// </summary>
        /// <value>The object version.</value>
        /// <autogeneratedoc />
        public ulong ObjectVersion { get; set; }

        /// <summary>
        /// Determines whether [is same object identifier] [the specified object other].
        /// </summary>
        /// <param name="objectOther">The object other.</param>
        /// <returns><c>true</c> if [is same object identifier] [the specified object other]; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        public bool IsSameObjectId(IPureObject objectOther) =>
            objectOther != null && ObjectId.Equals(objectOther.ObjectId);

        /// <summary>
        /// Determines whether [is same object version] [the specified object other].
        /// </summary>
        /// <param name="objectOther">The object other.</param>
        /// <returns><c>true</c> if [is same object version] [the specified object other]; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        public bool IsSameObjectVersion(IPureObject objectOther) =>
            objectOther != null && ObjectVersion == objectOther.ObjectVersion;

        /// <summary>
        /// Copies the instance.
        /// </summary>
        /// <returns>IPureObject.</returns>
        /// <autogeneratedoc />
        public virtual IPureObject CopyInstance()
        {
            return MemberwiseClone() as IPureObject;
        }

        /// <summary>
        /// Clones the instance.
        /// </summary>
        /// <returns>IPureObject.</returns>
        /// <autogeneratedoc />
        public virtual IPureObject CloneInstance()
        {
            var objectClone = (PureObjectBase) MemberwiseClone();

            // Establishes new ObjectId, CreatedTimestamp and ModifiedTimestamp
            objectClone._objectId = Guid.NewGuid();
            objectClone._createdTimestamp = ModifiedTimestamp = DateTimeOffset.Now;

            return objectClone;
        }

        /// <summary>
        /// Updates the instance.
        /// </summary>
        /// <param name="objectUpdate">The object update.</param>
        /// <returns>IPureObject.</returns>
        /// <autogeneratedoc />
        public virtual IPureObject UpdateInstance(IPureObject objectUpdate)
        {
            if (objectUpdate == null) throw new ArgumentNullException(nameof(objectUpdate));

            if (ObjectId.Equals(objectUpdate.ObjectId))
            {
                if (objectUpdate.ObjectVersion > ObjectVersion)
                {
                    Logger?.LogTrace("IPureObject:UpdateInstance: ");
                    ObjectVersion = objectUpdate.ObjectVersion;
                }

                IncrementObjectVersion();
                ModifiedTimestamp = DateTimeOffset.Now;
            }
            else
            {
                Logger?.LogDebug("UpdateInstance called when {CurObjectId} != {UpdateObjectId}", ObjectId,
                    objectUpdate.ObjectId);
            }

            return this;
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        /// <autogeneratedoc />
        public virtual int CompareTo(IPureObject other)
        {
            return other == null ? 1 : ObjectId.CompareTo(other.ObjectId);
        }


        /// <summary>
        /// Gets the log property list level.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="loggableFormat">The loggable format.</param>
        /// <returns>IEnumerable&lt;IPureLogPropertyLevel&gt;.</returns>
        /// <autogeneratedoc />
        public override IEnumerable<IPureLogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel,
            LoggableFormat loggableFormat)
        {
            var logPropertyLevels = loggableFormat.IsWithParents()
                ? base.GetLogPropertyListLevel(logLevel, loggableFormat)?.ToList()
                : new List<IPureLogPropertyLevel>();

            if (logLevel <= LogLevel.Information)
            {
                logPropertyLevels?.Add(new PureLogPropertyLevel(nameof(ObjectId), ObjectId, LogLevel.Information));
                logPropertyLevels?.Add(new PureLogPropertyLevel(nameof(ObjectVersion), ObjectVersion,
                    LogLevel.Information));
                logPropertyLevels?.Add(new PureLogPropertyLevel(nameof(CreatedTimestamp), CreatedTimestamp,
                    LogLevel.Information));
                logPropertyLevels?.Add(new PureLogPropertyLevel(nameof(ModifiedTimestamp), ModifiedTimestamp,
                    LogLevel.Information));
            }

            return logPropertyLevels?.Where(p => p.MinimumLogLevel.CompareTo(logLevel) >= 0);
        }

        /// <summary>
        /// Increments the object version.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        /// <autogeneratedoc />
        protected ulong IncrementObjectVersion() => IncrementObjectVersion(ObjectVersionDefaultIncrement);
        /// <summary>
        /// Increments the object version.
        /// </summary>
        /// <param name="incAmt">The inc amt.</param>
        /// <returns>System.UInt64.</returns>
        /// <autogeneratedoc />
        protected ulong IncrementObjectVersion(ulong incAmt) => ObjectVersion = ObjectVersion + incAmt;

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        /// <autogeneratedoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as PureObjectBase);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <autogeneratedoc />
        public override int GetHashCode()
        {
            return ObjectId.GetHashCode();
        }
    }
}