using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class DomeRouteController : WebApiController {

        private EquipmentManager equipmentManager;
        private JsonSerializerSettings jsonSettings;

        public DomeRouteController(IHttpContext context, EquipmentManager equipmentManager) {
            this.equipmentManager = equipmentManager;
            jsonSettings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Include
            };
        }

        [Route(HttpVerbs.Get, "/")]
        public string DomeStatus() {
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.NONE);
            return JsonConvert.SerializeObject(response, jsonSettings);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task<string> DomeConnect() {
            await equipmentManager.DomeConnect();
            var info = equipmentManager.DomeInfo();
            var response = new DomeConnectedResponse {
                Name = info.Name,
                DeviceId = info.DeviceId,
                Success = true,
            };
            return JsonConvert.SerializeObject(response, jsonSettings);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task<string> DomeDisconnect() {
            await equipmentManager.DomeDisconnect();
            var response = new DomeDisconnectedResponse {
                Success = true,
            };
            return JsonConvert.SerializeObject(response, jsonSettings);
        }

        [Route(HttpVerbs.Post, "/open")]
        public async Task<string> DomeOpen() {
            await equipmentManager.DomeSetShutterOpen(true);
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.OPENED);
            return JsonConvert.SerializeObject(response, jsonSettings);
        }

        [Route(HttpVerbs.Post, "/close")]
        public async Task<string> DomeClose() {
            await equipmentManager.DomeSetShutterOpen(false);
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.CLOSED);
            return JsonConvert.SerializeObject(response, jsonSettings);
        }

        [Route(HttpVerbs.Post, "/rotate")]
        public async Task<string> DomeSlew([JsonData] DomeSlewRequest request) {
            await equipmentManager.DomeSlew(request.Azimuth);
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.SLEWED);
            return JsonConvert.SerializeObject(response, jsonSettings);
        }

        [Route(HttpVerbs.Post, "/park")]
        public async Task<string> DomePark() {
            await equipmentManager.DomePark();
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.PARKED);
            return JsonConvert.SerializeObject(response, jsonSettings);
        }

        [Route(HttpVerbs.Post, "/home")]
        public async Task<string> DomeHome() {
            await equipmentManager.DomeFindHome();
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.HOMED);
            return JsonConvert.SerializeObject(response, jsonSettings);
        }
    }
}