using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class DomeRouteController : DeviceRouteController {

        public DomeRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public void DomeStatus() {
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task DomeConnect() {
            await equipmentManager.DomeConnect();
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task DomeDisconnect() {
            await equipmentManager.DomeDisconnect();
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.DISCONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/open")]
        public async Task DomeOpen() {
            await equipmentManager.DomeSetShutterOpen(true);
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.OPENED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/close")]
        public async Task DomeClose() {
            await equipmentManager.DomeSetShutterOpen(false);
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.CLOSED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/rotate")]
        public async Task DomeSlew([JsonData] DomeSlewRequest request) {
            await equipmentManager.DomeSlew(request.Azimuth);
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.SLEWED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/park")]
        public async Task DomePark() {
            await equipmentManager.DomePark();
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.PARKED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/home")]
        public async Task DomeHome() {
            await equipmentManager.DomeFindHome();
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, DomeAction.HOMED);
            RespondWithJSON(response);
        }
    }
}