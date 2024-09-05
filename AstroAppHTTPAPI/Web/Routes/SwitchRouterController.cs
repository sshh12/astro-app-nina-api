using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;
using NINA.Equipment.Equipment.MySwitch;
using System;
using NINA.Equipment.Interfaces;
using System.Collections.Immutable;
using System.Linq;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class Gauge {
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Value { get; set; }

        public static Gauge FromSwitch(ISwitch info) {
            return new Gauge {
                Name = info.Name,
                Description = info.Description,
                Value = Double.IsNaN(info.Value) ? null : info.Value,
            };
        }
    }

    public class SwitchStatusResponse {
        public string Type = "SwitchStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public Gauge[] Gauges { get; set; }
        public string Action { get; set; }

        public static SwitchStatusResponse FromSwitchInfo(SwitchInfo info, SwitchAction action) {
            return new SwitchStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Gauges = info.ReadonlySwitches?.ToImmutableList().Select(s => Gauge.FromSwitch(s)).ToArray(),
                Action = action.ToString(),
            };
        }
    }

    public class SwitchRouteController : DeviceRouteController {

        public SwitchRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async void SwitchStatus() {
            var info = equipmentManager.SwitchInfo();
            var response = SwitchStatusResponse.FromSwitchInfo(info, SwitchAction.NONE);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task SwitchConnect() {
            await equipmentManager.SwitchConnect();
            var info = equipmentManager.SwitchInfo();
            var response = SwitchStatusResponse.FromSwitchInfo(info, SwitchAction.CONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task SwitchDisconnect() {
            await equipmentManager.SwitchDisconnect();
            var info = equipmentManager.SwitchInfo();
            var response = SwitchStatusResponse.FromSwitchInfo(info, SwitchAction.DISCONNECTED);
            await RespondWithJSON(response);
        }

    }
}
