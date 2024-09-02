using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class DomeRouteController : DeviceRouteController {

        public DomeRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        private void respondWithInfo(DomeAction action) {
            var info = equipmentManager.DomeInfo();
            var following = equipmentManager.DomeIsFollowing();
            var response = DomeStatusResponse.FromDomeInfo(info, following, action);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Get, "/")]
        public void DomeStatus() {
            respondWithInfo(DomeAction.NONE);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task DomeConnect() {
            await equipmentManager.DomeConnect();
            respondWithInfo(DomeAction.CONNECTED);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task DomeDisconnect() {
            await equipmentManager.DomeDisconnect();
            respondWithInfo(DomeAction.DISCONNECTED);
        }

        [Route(HttpVerbs.Post, "/open")]
        public async Task DomeOpen() {
            await equipmentManager.DomeSetShutterOpen(true);
            respondWithInfo(DomeAction.OPENED);
        }

        [Route(HttpVerbs.Post, "/close")]
        public async Task DomeClose() {
            await equipmentManager.DomeSetShutterOpen(false);
            respondWithInfo(DomeAction.CLOSED);
        }

        [Route(HttpVerbs.Post, "/rotate")]
        public async Task DomeSlew([JsonData] DomeSlewRequest request) {
            await equipmentManager.DomeSlew(request.Azimuth);
            respondWithInfo(DomeAction.SLEWED);
        }

        [Route(HttpVerbs.Post, "/park")]
        public async Task DomePark() {
            await equipmentManager.DomePark();
            respondWithInfo(DomeAction.PARKED);
        }

        [Route(HttpVerbs.Post, "/home")]
        public async Task DomeHome() {
            await equipmentManager.DomeFindHome();
            respondWithInfo(DomeAction.HOMED);
        }

        [Route(HttpVerbs.Post, "/sync")]
        public async Task DomeSync() {
            await equipmentManager.DomeSyncToMount();
            respondWithInfo(DomeAction.SYNCED);
        }

        [Route(HttpVerbs.Patch, "/following")]
        public async Task DomeSetFollowing([JsonData] DomeFollowingRequest request) {
            await equipmentManager.DomeSetFollowing(request.Enabled);
            respondWithInfo(DomeAction.FOLLOWING_UPDATED);
        }
    }
}