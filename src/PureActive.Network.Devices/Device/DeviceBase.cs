﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.System;
using PureActive.Logging.Abstractions.Interfaces;
using PureActive.Logging.Abstractions.Types;
using PureActive.Logging.Extensions.Types;
using PureActive.Network.Abstractions.Device;
using PureActive.Network.Abstractions.PureObject;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.PureObject;

namespace PureActive.Network.Devices.Device
{
    public abstract class DeviceBase : PureObjectBase, IDevice
    {
        protected DeviceBase(ICommonServices commonServices, DeviceType deviceType = DeviceType.UnknownDevice,
            IPureLogger logger = null) :
            base(commonServices?.LoggerFactory, logger)
        {
            DeviceType = deviceType;
            CommonServices = commonServices;
        }

        public ICommonServices CommonServices { get; }
        public DeviceType DeviceType { get; set; }

        public override int CompareTo(IPureObject other)
        {
            if (!(other is DeviceBase))
                throw new ArgumentException("Object must be of type DeviceBase.");

            return CompareTo((DeviceBase) other);
        }

        public override IEnumerable<IPureLogPropertyLevel> GetLogPropertyListLevel(LogLevel logLevel,
            LoggableFormat loggableFormat)
        {
            var logPropertyLevels = loggableFormat.IsWithParents()
                ? base.GetLogPropertyListLevel(logLevel, loggableFormat)?.ToList()
                : new List<IPureLogPropertyLevel>();

            if (logLevel <= LogLevel.Information)
            {
                logPropertyLevels?.Add(new PureLogPropertyLevel(nameof(DeviceType), DeviceType, LogLevel.Information));
            }

            return logPropertyLevels;
        }

        public int CompareTo(DeviceBase other)
        {
            if (other == null) return 1;

            if (ObjectId.Equals(other.ObjectId))
                return 0;

            return DeviceType.CompareTo(other.DeviceType);
        }
    }
}