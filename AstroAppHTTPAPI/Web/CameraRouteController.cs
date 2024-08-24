using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class CameraRouteController : DeviceRouteController {

        public CameraRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async Task CameraStatus() {
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, CameraAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task CameraConnect() {
            await equipmentManager.CameraConnect();
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, CameraAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task CameraDisconnect() {
            await equipmentManager.CameraDisconnect();
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, CameraAction.DISCONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Patch, "/binning")]
        public async Task CameraSetBinning([JsonData] CameraBinningRequest request) {
            await equipmentManager.CameraSetBinning(request.X, request.Y);
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, CameraAction.BINNING_UPDATED);
            RespondWithJSON(response);
        }
    }
}
