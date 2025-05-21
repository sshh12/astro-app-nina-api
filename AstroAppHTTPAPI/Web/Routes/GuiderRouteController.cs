using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;
using NINA.Equipment.Equipment.MyGuider;
using System;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public enum GuiderDirection {
        North,
        South,
        East,
        West
    }

    public class PulseGuideRequest {
        public GuiderDirection Direction { get; set; }
        public int Duration { get; set; }
    }

    public class GuiderStatusResponse {
        public string Type = "GuiderStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public double? PixelScale { get; set; }
        public string Action { get; set; }

        public static GuiderStatusResponse FromGuiderInfo(GuiderInfo info, GuiderAction action) {
            return new GuiderStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                PixelScale = Double.IsNaN(info.PixelScale) ? null : info.PixelScale,
                Action = action.ToString(),
            };
        }
    }

    public class GuiderRouteController : DeviceRouteController {

        public GuiderRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async void GuiderStatus() {
            var info = equipmentManager.GuiderInfo();
            var response = GuiderStatusResponse.FromGuiderInfo(info, GuiderAction.NONE);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task GuiderConnect() {
            await equipmentManager.GuiderConnect();
            var info = equipmentManager.GuiderInfo();
            var response = GuiderStatusResponse.FromGuiderInfo(info, GuiderAction.CONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task GuiderDisconnect() {
            await equipmentManager.GuiderDisconnect();
            var info = equipmentManager.GuiderInfo();
            var response = GuiderStatusResponse.FromGuiderInfo(info, GuiderAction.DISCONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/pulseguide")]
        public async Task GuiderPulseGuide([JsonData] PulseGuideRequest request) {
            await equipmentManager.MountPulseGuide(request.Direction, request.Duration);
            var info = equipmentManager.GuiderInfo();
            // Using GuiderAction.NONE for now as there isn't a specific action for pulse guiding.
            // A more specific action like GUIDING_PULSED could be added to GuiderAction enum later if needed.
            var response = GuiderStatusResponse.FromGuiderInfo(info, GuiderAction.NONE);
            await RespondWithJSON(response);
        }
    }
}
