using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class GuiderRouteController : DeviceRouteController {

        public GuiderRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public void GuiderStatus() {
            var info = equipmentManager.GuiderInfo();
            var response = GuiderStatusResponse.FromGuiderInfo(info, GuiderAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task GuiderConnect() {
            await equipmentManager.GuiderConnect();
            var info = equipmentManager.GuiderInfo();
            var response = GuiderStatusResponse.FromGuiderInfo(info, GuiderAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task GuiderDisconnect() {
            await equipmentManager.GuiderDisconnect();
            var info = equipmentManager.GuiderInfo();
            var response = GuiderStatusResponse.FromGuiderInfo(info, GuiderAction.DISCONNECTED);
            RespondWithJSON(response);
        }

    }
}
