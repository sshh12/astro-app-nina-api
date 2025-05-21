using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NINA.Core.Model;
using NINA.Core.Utility;
using NINA.Equipment.Equipment.MyCamera;
using NINA.Equipment.Equipment.MyDome;
using NINA.Equipment.Equipment.MyFilterWheel;
using NINA.Equipment.Equipment.MyTelescope;
using NINA.Equipment.Equipment.MyFocuser;
using NINA.Equipment.Equipment.MyGuider;
using NINA.Equipment.Equipment.MySwitch;
using NINA.Equipment.Equipment.MyRotator;
using NINA.Equipment.Equipment.MyFlatDevice;
using NINA.Equipment.Equipment.MyWeatherData;
using NINA.Equipment.Equipment.MySafetyMonitor;
using NINA.Equipment.Interfaces.Mediator;
using NINA.WPF.Base.Interfaces.Mediator;
using NINA.Equipment.Interfaces;
using NINA.Equipment.Interfaces.Guiding;
using NINA.Equipment.Model;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using Plugin.NINA.AstroAppHTTPAPI.Web; // For GuiderDirection
using System;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Tests.Equipment {

    [TestClass]
    public class EquipmentManagerTests {

        private Mock<IApplicationStatusMediator> _mockStatusMediator;
        private Mock<ICameraMediator> _mockCameraMediator;
        private Mock<IImagingMediator> _mockImagingMediator;
        private Mock<IImageSaveMediator> _mockImageSaveMediator;
        private Mock<IDomeMediator> _mockDomeMediator;
        private Mock<IFilterWheelMediator> _mockFilterWheelMediator;
        private Mock<IFocuserMediator> _mockFocuserMediator;
        private Mock<IGuiderMediator> _mockGuiderMediator;
        private Mock<ISwitchMediator> _mockSwitchMediator;
        private Mock<IRotatorMediator> _mockRotatorMediator;
        private Mock<IFlatDeviceMediator> _mockFlatDeviceMediator;
        private Mock<IWeatherDataMediator> _mockWeatherMediator;
        private Mock<ISafetyMonitorMediator> _mockSafetyMonitorMediator;
        private Mock<ITelescopeMediator> _mockTelescopeMediator;

        private EquipmentManager _equipmentManager;

        [TestInitialize]
        public void TestInitialize() {
            _mockStatusMediator = new Mock<IApplicationStatusMediator>();
            _mockCameraMediator = new Mock<ICameraMediator>();
            _mockImagingMediator = new Mock<IImagingMediator>();
            _mockImageSaveMediator = new Mock<IImageSaveMediator>();
            _mockDomeMediator = new Mock<IDomeMediator>();
            _mockFilterWheelMediator = new Mock<IFilterWheelMediator>();
            _mockFocuserMediator = new Mock<IFocuserMediator>();
            _mockGuiderMediator = new Mock<IGuiderMediator>();
            _mockSwitchMediator = new Mock<ISwitchMediator>();
            _mockRotatorMediator = new Mock<IRotatorMediator>();
            _mockFlatDeviceMediator = new Mock<IFlatDeviceMediator>();
            _mockWeatherMediator = new Mock<IWeatherDataMediator>();
            _mockSafetyMonitorMediator = new Mock<ISafetyMonitorMediator>();
            _mockTelescopeMediator = new Mock<ITelescopeMediator>();

            // Setup default GetInfo() for TelescopeMediator to indicate connected
            var telescopeInfo = new TelescopeInfo("MockTelescope", Guid.NewGuid().ToString()) { Connected = true };
            _mockTelescopeMediator.Setup(m => m.GetInfo()).Returns(telescopeInfo);

            _equipmentManager = new EquipmentManager(
                _mockStatusMediator.Object,
                _mockCameraMediator.Object,
                _mockImagingMediator.Object,
                _mockImageSaveMediator.Object,
                _mockDomeMediator.Object,
                _mockFilterWheelMediator.Object,
                _mockFocuserMediator.Object,
                _mockGuiderMediator.Object,
                _mockSwitchMediator.Object,
                _mockRotatorMediator.Object,
                _mockFlatDeviceMediator.Object,
                _mockWeatherMediator.Object,
                _mockSafetyMonitorMediator.Object,
                _mockTelescopeMediator.Object
            );
        }

        private async Task TestPulseGuideDirection(GuiderDirection apiDirection, NINA.Equipment.Interfaces.Guiding.PulseGuideDirection ninaDirection) {
            int duration = 1500;
            bool eventRaised = false;
            _equipmentManager.MountUpdated += (sender, args) => {
                eventRaised = true;
                Assert.AreEqual(MountAction.NONE, args.Action);
            };

            await _equipmentManager.MountPulseGuide(apiDirection, duration);

            _mockTelescopeMediator.Verify(m => m.PulseGuide(ninaDirection, duration), Times.Once());
            Assert.IsTrue(eventRaised, $"MountUpdated event was not raised for {apiDirection}");
        }

        [TestMethod]
        public async Task MountPulseGuide_North_CallsTelescopePulseGuideNorthAndRaisesEvent() {
            await TestPulseGuideDirection(GuiderDirection.North, NINA.Equipment.Interfaces.Guiding.PulseGuideDirection.North);
        }

        [TestMethod]
        public async Task MountPulseGuide_South_CallsTelescopePulseGuideSouthAndRaisesEvent() {
            await TestPulseGuideDirection(GuiderDirection.South, NINA.Equipment.Interfaces.Guiding.PulseGuideDirection.South);
        }

        [TestMethod]
        public async Task MountPulseGuide_East_CallsTelescopePulseGuideEastAndRaisesEvent() {
            await TestPulseGuideDirection(GuiderDirection.East, NINA.Equipment.Interfaces.Guiding.PulseGuideDirection.East);
        }

        [TestMethod]
        public async Task MountPulseGuide_West_CallsTelescopePulseGuideWestAndRaisesEvent() {
            await TestPulseGuideDirection(GuiderDirection.West, NINA.Equipment.Interfaces.Guiding.PulseGuideDirection.West);
        }

        [TestMethod]
        public async Task MountPulseGuide_MountNotConnected_DoesNotCallPulseGuideAndDoesNotRaiseEvent() {
            // Arrange
            var telescopeInfo = new TelescopeInfo("MockTelescope", Guid.NewGuid().ToString()) { Connected = false };
            _mockTelescopeMediator.Setup(m => m.GetInfo()).Returns(telescopeInfo);
            int duration = 1000;
            bool eventRaised = false;
            _equipmentManager.MountUpdated += (sender, args) => {
                eventRaised = true; // Should not happen
            };

            // Act
            await _equipmentManager.MountPulseGuide(GuiderDirection.North, duration);

            // Assert
            _mockTelescopeMediator.Verify(m => m.PulseGuide(It.IsAny<NINA.Equipment.Interfaces.Guiding.PulseGuideDirection>(), It.IsAny<int>()), Times.Never());
            Assert.IsFalse(eventRaised, "MountUpdated event should not be raised when mount is not connected.");
        }
    }
}
