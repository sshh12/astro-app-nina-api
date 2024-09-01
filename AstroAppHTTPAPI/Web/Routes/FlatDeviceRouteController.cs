using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class FlatDeviceRouteController : DeviceRouteController {

        public FlatDeviceRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public void FlatDeviceStatus() {
            var info = equipmentManager.FlatDeviceInfo();
            var response = FlatDeviceStatusResponse.FromFlatDeviceInfo(info, FlatDeviceAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task FlatDeviceConnect() {
            await equipmentManager.FlatDeviceConnect();
            var info = equipmentManager.FlatDeviceInfo();
            var response = FlatDeviceStatusResponse.FromFlatDeviceInfo(info, FlatDeviceAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task FlatDeviceDisconnect() {
            await equipmentManager.FlatDeviceDisconnect();
            var info = equipmentManager.FlatDeviceInfo();
            var response = FlatDeviceStatusResponse.FromFlatDeviceInfo(info, FlatDeviceAction.DISCONNECTED);
            RespondWithJSON(response);
        }

    }
}
