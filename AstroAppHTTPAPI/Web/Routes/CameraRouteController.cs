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
        public short BinX { get; set; }
        public short BinY { get; set; }
        public int XSize { get; set; }
        public int YSize { get; set; }
        public double? PixelSize { get; set; }
        public string SensorType { get; set; }
        public string Action { get; set; }

        public static CameraStatusResponse FromCameraInfo(CameraInfo info, CameraAction action) {
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
                BinX = info.BinX,
                BinY = info.BinY,
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

    public class CameraRouteController : DeviceRouteController {

        public CameraRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async void CameraStatus() {
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, CameraAction.NONE);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task CameraConnect() {
            await equipmentManager.CameraConnect();
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, CameraAction.CONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task CameraDisconnect() {
            await equipmentManager.CameraDisconnect();
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, CameraAction.DISCONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Patch, "/binning")]
        public async Task CameraSetBinning([JsonData] CameraBinningRequest request) {
            await equipmentManager.CameraSetBinning(request.X, request.Y);
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, CameraAction.BINNING_UPDATED);
            await RespondWithJSON(response);
        }
    }
}
