using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;
using NINA.Equipment.Equipment.MySafetyMonitor;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class SafetyMonitorStatusResponse {
        public string Type = "SafetyMonitorStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public bool IsSafe { get; set; }
        public string Action { get; set; }

        public static SafetyMonitorStatusResponse FromSafetyMonitorInfo(SafetyMonitorInfo info, SafetyMonitorAction action) {
            return new SafetyMonitorStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                IsSafe = info.IsSafe,
                Action = action.ToString(),
            };
        }
    }

    public class SafetyMonitorController : DeviceRouteController {

        public SafetyMonitorController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async void SafetyMonitorStatus() {
            var info = equipmentManager.SafetyMonitorInfo();
            var response = SafetyMonitorStatusResponse.FromSafetyMonitorInfo(info, SafetyMonitorAction.NONE);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task SafetyMonitorConnect() {
            await equipmentManager.SafetyMonitorConnect();
            var info = equipmentManager.SafetyMonitorInfo();
            var response = SafetyMonitorStatusResponse.FromSafetyMonitorInfo(info, SafetyMonitorAction.CONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task SafetyMonitorDisconnect() {
            await equipmentManager.SafetyMonitorDisconnect();
            var info = equipmentManager.SafetyMonitorInfo();
            var response = SafetyMonitorStatusResponse.FromSafetyMonitorInfo(info, SafetyMonitorAction.DISCONNECTED);
            await RespondWithJSON(response);
        }
    }
}
