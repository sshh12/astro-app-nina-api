using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class RouteController : WebApiController {

        private EquipmentManager equipmentManager;
        private JsonSerializerSettings jsonSettings;

        public RouteController(IHttpContext context, EquipmentManager equipmentManager) {
            this.equipmentManager = equipmentManager;
            jsonSettings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Include
            };
        }

        [Route(HttpVerbs.Get, "/")]
        public string Index() {
            return "https://github.com/sshh12/astro-app-nina-api";
        }

        [Route(HttpVerbs.Post, "/camera/connect")]
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

        [Route(HttpVerbs.Get, "/camera")]
        public string CameraStatus() {
            return equipmentManager.Camera.GetInfo().Connected.ToString();
        }
    }
}
