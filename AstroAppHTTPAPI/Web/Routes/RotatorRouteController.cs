using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class RotatorRouteController : DeviceRouteController {

        public RotatorRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public void RotatorStatus() {
            var info = equipmentManager.RotatorInfo();
            var response = RotatorStatusResponse.FromRotatorInfo(info, RotatorAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task RotatorConnect() {
            await equipmentManager.RotatorConnect();
            var info = equipmentManager.RotatorInfo();
            var response = RotatorStatusResponse.FromRotatorInfo(info, RotatorAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task RotatorDisconnect() {
            await equipmentManager.RotatorDisconnect();
            var info = equipmentManager.RotatorInfo();
            var response = RotatorStatusResponse.FromRotatorInfo(info, RotatorAction.DISCONNECTED);
            RespondWithJSON(response);
        }

    }
}
