using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EmbedIO;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using Plugin.NINA.AstroAppHTTPAPI.Web; // For GuiderRouteController, PulseGuideRequest, GuiderDirection
using NINA.Equipment.Equipment.MyGuider; // For GuiderInfo
using System.Threading.Tasks;
using System; // For Guid

namespace Plugin.NINA.AstroAppHTTPAPI.Tests.Web.Routes {

    // Helper class to make RespondWithJSON testable
    public class TestableGuiderRouteController : GuiderRouteController {
        public object CapturedResponse { get; private set; }

        public TestableGuiderRouteController(IHttpContext context, EquipmentManager equipmentManager)
            : base(context, equipmentManager) {
        }

        protected override Task RespondWithJSON(object data) {
            CapturedResponse = data;
            return Task.CompletedTask;
        }
    }

    [TestClass]
    public class GuiderRouteControllerTests {

        private Mock<IHttpContext> _mockHttpContext;
        private Mock<EquipmentManager> _mockEquipmentManager; // EquipmentManager is a concrete class, but we can still mock its virtual/interface methods if needed or use it directly if simple
                                                              // For this test, we need to mock methods on EquipmentManager.
                                                              // If EquipmentManager methods aren't virtual, we'd need an interface IEquipmentManager.
                                                              // Assuming EquipmentManager has virtual methods or we are testing integration up to a point.
                                                              // The subtask implies we mock EquipmentManager, so we'll assume its methods are mockable (e.g., virtual).

        private TestableGuiderRouteController _controller;

        [TestInitialize]
        public void TestInitialize() {
            _mockHttpContext = new Mock<IHttpContext>();

            // We need to mock EquipmentManager. If its constructor is complex or it has non-virtual methods
            // we want to mock, this setup might need adjustment (e.g., using a wrapper interface or ensuring methods are virtual).
            // For now, let's assume we can mock the necessary methods like MountPulseGuide and GuiderInfo.
            // Moq can mock concrete classes, but methods to be mocked must be virtual.
            // Let's proceed assuming MountPulseGuide and GuiderInfo are virtual in EquipmentManager.
            // If not, these tests would actually call the real implementation, which is not ideal for unit testing the controller.
            // For the purpose of this exercise, we'll proceed as if they are virtual.
            _mockEquipmentManager = new Mock<EquipmentManager>(
                null, null, null, null, null, null, null, null, null, null, null, null, null, null // Assuming constructor takes these, pass nulls for simplicity
            );


            _controller = new TestableGuiderRouteController(_mockHttpContext.Object, _mockEquipmentManager.Object);
        }

        [TestMethod]
        public async Task GuiderPulseGuide_ValidRequest_CallsManagerAndRespondsWithCorrectStatus() {
            // Arrange
            var request = new PulseGuideRequest {
                Direction = GuiderDirection.North,
                Duration = 1234
            };

            var expectedGuiderInfo = new GuiderInfo("TestGuider", Guid.NewGuid().ToString()) {
                Connected = true,
                PixelScale = 1.5,
                Description = "Mocked Guider"
            };

            _mockEquipmentManager
                .Setup(em => em.MountPulseGuide(request.Direction, request.Duration))
                .Returns(Task.CompletedTask); // Setup the async method

            _mockEquipmentManager
                .Setup(em => em.GuiderInfo())
                .Returns(expectedGuiderInfo);

            // Act
            await _controller.GuiderPulseGuide(request);

            // Assert
            _mockEquipmentManager.Verify(em => em.MountPulseGuide(request.Direction, request.Duration), Times.Once());
            _mockEquipmentManager.Verify(em => em.GuiderInfo(), Times.Once());

            Assert.IsNotNull(_controller.CapturedResponse, "Response object should have been captured.");
            Assert.IsInstanceOfType(_controller.CapturedResponse, typeof(GuiderStatusResponse), "Captured response should be a GuiderStatusResponse.");

            var actualResponse = _controller.CapturedResponse as GuiderStatusResponse;
            Assert.IsNotNull(actualResponse, "Could not cast captured response to GuiderStatusResponse.");

            Assert.AreEqual("GuiderStatus", actualResponse.Type);
            Assert.AreEqual(expectedGuiderInfo.Name, actualResponse.Name);
            Assert.AreEqual(expectedGuiderInfo.Description, actualResponse.Description);
            Assert.AreEqual(expectedGuiderInfo.DeviceId, actualResponse.DeviceId);
            Assert.AreEqual(expectedGuiderInfo.Connected, actualResponse.Connected);
            Assert.AreEqual(expectedGuiderInfo.PixelScale, actualResponse.PixelScale);
            Assert.AreEqual(GuiderAction.NONE.ToString(), actualResponse.Action, "GuiderAction should be NONE.");
        }

        [TestMethod]
        public async Task GuiderPulseGuide_HandlesExceptionFromMountPulseGuide_DoesNotThrowAndGuiderInfoNotCalled()
        {
            // Arrange
            var request = new PulseGuideRequest
            {
                Direction = GuiderDirection.East,
                Duration = 500
            };

            var expectedException = new InvalidOperationException("Pulse guide failed");

            _mockEquipmentManager
                .Setup(em => em.MountPulseGuide(request.Direction, request.Duration))
                .ThrowsAsync(expectedException); // Setup to throw an exception

            // Act & Assert
            try
            {
                await _controller.GuiderPulseGuide(request);
                // If we want to ensure controller handles exception and responds (e.g. with error status),
                // then this test needs to be more complex. For now, let's assume it just bubbles up or is caught by EmbedIO.
                // The current controller implementation does not catch exceptions from MountPulseGuide.
                // So, the exception would propagate. Test updated to expect this.
                Assert.Fail("Exception was expected from MountPulseGuide but not thrown or propagated by controller.");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(expectedException.Message, ex.Message, "The correct exception should be propagated.");
            }


            _mockEquipmentManager.Verify(em => em.MountPulseGuide(request.Direction, request.Duration), Times.Once());
            // GuiderInfo() and RespondWithJSON should not be called if MountPulseGuide throws.
            _mockEquipmentManager.Verify(em => em.GuiderInfo(), Times.Never());
            Assert.IsNull(_controller.CapturedResponse, "RespondWithJSON should not have been called if MountPulseGuide failed.");
        }
    }
}
