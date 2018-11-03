using Microsoft.Extensions.Logging;
using PureActive.Hosting.Abstractions.System;
using PureActive.Hosting.CommonServices;
using PureActive.Logging.Abstractions.Types;
using PureActive.Network.Abstractions.Types;
using PureActive.Network.Devices.Device;
using PureActive.Serilog.Sink.Xunit.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace PureActive.Network.Services.UnitTests.Device
{
    public class DeviceUnitTests : LoggingUnitTestBase<DeviceUnitTests>
    {
        private readonly ICommonServices _commonServices;

        public DeviceUnitTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _commonServices = CommonServices.CreateInstance(TestLoggerFactory, "DeviceUnitTests");
        }

        public class DeviceTest : DeviceBase
        {
            public DeviceTest(ICommonServices commonServices, DeviceType deviceType) : 
                base(commonServices, deviceType)
            {

            }
        }

        [Fact]
        public void TestDeviceToLog()
        {
            var deviceTest = new DeviceTest(_commonServices, DeviceType.UnknownDevice);

            TestOutputHelper.WriteLine(deviceTest.ToString(LogLevel.Debug, LoggableFormat.ToLog));
        }


        [Fact]
        public void TestDeviceToString()
        {
            var deviceTest = new DeviceTest(_commonServices, DeviceType.UnknownDevice);

            TestOutputHelper.WriteLine(deviceTest.ToString(LogLevel.Debug, LoggableFormat.ToString));
        }

        [Fact]
        public void TestDeviceToLogParents()
        {
            var deviceTest = new DeviceTest(_commonServices, DeviceType.UnknownDevice);

            TestOutputHelper.WriteLine(deviceTest.ToString(LogLevel.Debug, LoggableFormat.ToLogWithParents));
        }

        [Fact]
        public void TestDeviceToStringParents()
        {
            var deviceTest = new DeviceTest(_commonServices, DeviceType.UnknownDevice);

            TestOutputHelper.WriteLine(deviceTest.ToString(LogLevel.Debug, LoggableFormat.ToStringWithParents));
        }
    }
}