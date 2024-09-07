using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using NINA.Equipment.Equipment.MyCamera;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;
using System;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class CameraStatusResponse {
        public string Type = "CameraStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public int? Battery { get; set; }
        public bool Connected { get; set; }
        public int Offset { get; set; }
        public int? Gain { get; set; }
        public int? DefaultGain { get; set; }
        public double? Temperature { get; set; }
        public double? TemperatureSetPoint { get; set; }
        public bool AtTargetTemp { get; set; }
        public double? TargetTemp { get; set; }
        public bool CoolerOn { get; set; }
        public double? CoolerPower { get; set; }
        public bool HasDewHeater { get; set; }
        public short BinX { get; set; }
        public short BinY { get; set; }
        public int USBLimit { get; set; }
        public int XSize { get; set; }
        public int YSize { get; set; }
        public double? PixelSize { get; set; }
        public string SensorType { get; set; }
        public string Action { get; set; }

        public static CameraStatusResponse FromCameraInfo(CameraInfo info, bool atTargetTemp, double targetTemp, CameraAction action) {
            return new CameraStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Battery = info.Battery < 0 ? null : info.Battery,
                Connected = info.Connected,
                Offset = info.Offset,
                Gain = info.Gain < 0 ? null : info.Gain,
                DefaultGain = info.DefaultGain < 0 ? null : info.DefaultGain,
                Temperature = Double.IsNaN(info.Temperature) ? null : info.Temperature,
                TemperatureSetPoint = Double.IsNaN(info.TemperatureSetPoint) ? null : info.TemperatureSetPoint,
                AtTargetTemp = atTargetTemp,
                TargetTemp = Double.IsNaN(targetTemp) ? null : targetTemp,
                CoolerOn = info.CoolerOn,
                CoolerPower = Double.IsNaN(info.CoolerPower) ? null : info.CoolerPower,
                HasDewHeater = info.HasDewHeater,
                BinX = info.BinX,
                BinY = info.BinY,
                USBLimit = info.USBLimit,
                XSize = info.XSize,
                YSize = info.YSize,
                PixelSize = Double.IsNaN(info.PixelSize) ? null : info.PixelSize,
                SensorType = info.SensorType.ToString(),
                Action = action.ToString(),
            };
        }

    }

    public class CameraBinningRequest {
        public short X { get; set; }
        public short Y { get; set; }
    }

    public class CameraCaptureRequest {
        public double ExposureTime { get; set; }
    }

    public class CameraRouteController : DeviceRouteController {

        public CameraRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        private async Task respondWithInfo(CameraAction action) {
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, equipmentManager.CameraAtTargetTemp(), equipmentManager.CameraTargetTemp(), action);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Get, "/")]
        public async void CameraStatus() {
            await respondWithInfo(CameraAction.NONE);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task CameraConnect() {
            await equipmentManager.CameraConnect();
            await respondWithInfo(CameraAction.CONNECTED);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task CameraDisconnect() {
            await equipmentManager.CameraDisconnect();
            await respondWithInfo(CameraAction.DISCONNECTED);
        }

        [Route(HttpVerbs.Patch, "/binning")]
        public async Task CameraSetBinning([JsonData] CameraBinningRequest request) {
            await equipmentManager.CameraSetBinning(request.X, request.Y);
            await respondWithInfo(CameraAction.BINNING_UPDATED);
        }

        [Route(HttpVerbs.Post, "/capture")]
        public async Task CameraCapture([JsonData] CameraCaptureRequest request) {
            await equipmentManager.CameraCapture(request.ExposureTime);
            await respondWithInfo(CameraAction.NONE);
        }
    }
}
