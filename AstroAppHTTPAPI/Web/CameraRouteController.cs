using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class CameraRouteController : WebApiController {

        private EquipmentManager equipmentManager;
        private JsonSerializerSettings jsonSettings;

        public CameraRouteController(IHttpContext context, EquipmentManager equipmentManager) {
            this.equipmentManager = equipmentManager;
            jsonSettings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Include
            };
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task<string> CameraConnect() {
            await equipmentManager.CameraConnect();
            var info = equipmentManager.Camera.GetInfo();
            var response = new CameraConnectedResponse {
                Name = info.Name,
                DeviceId = info.DeviceId,
                Success = true,
            };
            return JsonConvert.SerializeObject(response, jsonSettings);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task<string> CameraDisconnect() {
            await equipmentManager.CameraDisconnect();
            var response = new CameraDisconnectedResponse {
                Success = true,
            };
            return JsonConvert.SerializeObject(response, jsonSettings);
        }

        [Route(HttpVerbs.Get, "/")]
        public string CameraStatus() {
            var info = equipmentManager.Camera.GetInfo();
            var response = CameraStatusResponse.FromCameraInfo(info);
            return JsonConvert.SerializeObject(response, jsonSettings);
        }

        [Route(HttpVerbs.Patch, "/binning")]
        public async Task<string> CameraSetBinning([JsonData] CameraBinningRequest request) {
            await equipmentManager.CameraSetBinning(request.x, request.y);
            var info = equipmentManager.Camera.GetInfo();
            var response = CameraStatusResponse.FromCameraInfo(info);
            return JsonConvert.SerializeObject(response, jsonSettings);
        }
    }
}
