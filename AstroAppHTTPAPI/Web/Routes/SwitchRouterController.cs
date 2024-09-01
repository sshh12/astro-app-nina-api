using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class SwitchRouteController : DeviceRouteController {

        public SwitchRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public void SwitchStatus() {
            var info = equipmentManager.SwitchInfo();
            var response = SwitchStatusResponse.FromSwitchInfo(info, SwitchAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task SwitchConnect() {
            await equipmentManager.SwitchConnect();
            var info = equipmentManager.SwitchInfo();
            var response = SwitchStatusResponse.FromSwitchInfo(info, SwitchAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task SwitchDisconnect() {
            await equipmentManager.SwitchDisconnect();
            var info = equipmentManager.SwitchInfo();
            var response = SwitchStatusResponse.FromSwitchInfo(info, SwitchAction.DISCONNECTED);
            RespondWithJSON(response);
        }

    }
}
