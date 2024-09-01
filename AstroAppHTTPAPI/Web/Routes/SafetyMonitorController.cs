using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class SafetyMonitorController : DeviceRouteController {

        public SafetyMonitorController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public void SafetyMonitorStatus() {
            var info = equipmentManager.SafetyMonitorInfo();
            var response = SafetyMonitorStatusResponse.FromSafetyMonitorInfo(info, SafetyMonitorAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task SafetyMonitorConnect() {
            await equipmentManager.SafetyMonitorConnect();
            var info = equipmentManager.SafetyMonitorInfo();
            var response = SafetyMonitorStatusResponse.FromSafetyMonitorInfo(info, SafetyMonitorAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task SafetyMonitorDisconnect() {
            await equipmentManager.SafetyMonitorDisconnect();
            var info = equipmentManager.SafetyMonitorInfo();
            var response = SafetyMonitorStatusResponse.FromSafetyMonitorInfo(info, SafetyMonitorAction.DISCONNECTED);
            RespondWithJSON(response);
        }
    }
}
