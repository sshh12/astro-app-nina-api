using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class MountRouteController : DeviceRouteController {

        public MountRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async Task MountStatus() {
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task MountConnect() {
            await equipmentManager.MountConnect();
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task MountDisconnect() {
            await equipmentManager.MountDisconnect();
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.DISCONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/park")]
        public async Task MountPark() {
            await equipmentManager.MountPark();
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.PARKED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/unpark")]
        public async Task MountUnpark() {
            await equipmentManager.MountUnpark();
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.UNPARKED);
            RespondWithJSON(response);
        }
    }
}
