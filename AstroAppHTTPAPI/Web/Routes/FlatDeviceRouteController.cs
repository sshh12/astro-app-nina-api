using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;
using NINA.Equipment.Equipment.MyFlatDevice;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class FlatDeviceStatusResponse {
        public string Type = "FlatDeviceStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public int Brightness { get; set; }
        public int MaxBrightness { get; set; }
        public int MinBrightness { get; set; }
        public string CoverState { get; set; }
        public bool LightOn { get; set; }
        public string Action { get; set; }

        public static FlatDeviceStatusResponse FromFlatDeviceInfo(FlatDeviceInfo info, FlatDeviceAction action) {
            return new FlatDeviceStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Brightness = info.Brightness,
                MaxBrightness = info.MaxBrightness,
                MinBrightness = info.MinBrightness,
                CoverState = info.CoverState.ToString(),
                LightOn = info.LightOn,
                Action = action.ToString(),
            };
        }
    }

    public class FlatDeviceRouteController : DeviceRouteController {

        public FlatDeviceRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async Task FlatDeviceStatus() {
            var info = equipmentManager.FlatDeviceInfo();
            var response = FlatDeviceStatusResponse.FromFlatDeviceInfo(info, FlatDeviceAction.NONE);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task FlatDeviceConnect() {
            await equipmentManager.FlatDeviceConnect();
            var info = equipmentManager.FlatDeviceInfo();
            var response = FlatDeviceStatusResponse.FromFlatDeviceInfo(info, FlatDeviceAction.CONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task FlatDeviceDisconnect() {
            await equipmentManager.FlatDeviceDisconnect();
            var info = equipmentManager.FlatDeviceInfo();
            var response = FlatDeviceStatusResponse.FromFlatDeviceInfo(info, FlatDeviceAction.DISCONNECTED);
            await RespondWithJSON(response);
        }

    }
}
