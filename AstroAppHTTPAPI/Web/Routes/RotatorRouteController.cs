using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;
using NINA.Equipment.Equipment.MyRotator;
using System;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class RotatorStatusResponse {
        public string Type = "RotatorStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public double? Position { get; set; }
        public bool IsMoving { get; set; }
        public double? StepSize { get; set; }
        public float? MechanicalPosition { get; set; }
        public bool Reverse { get; set; }
        public string Action { get; set; }

        public static RotatorStatusResponse FromRotatorInfo(RotatorInfo info, RotatorAction action) {
            return new RotatorStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Position = Double.IsNaN(info.Position) ? null : info.Position,
                IsMoving = info.IsMoving,
                StepSize = Double.IsNaN(info.StepSize) ? null : info.StepSize,
                MechanicalPosition = float.IsNaN(info.MechanicalPosition) ? null : info.MechanicalPosition,
                Reverse = info.Reverse,
                Action = action.ToString(),
            };
        }
    }

    public class RotatorRouteController : DeviceRouteController {

        public RotatorRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async void RotatorStatus() {
            var info = equipmentManager.RotatorInfo();
            var response = RotatorStatusResponse.FromRotatorInfo(info, RotatorAction.NONE);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task RotatorConnect() {
            await equipmentManager.RotatorConnect();
            var info = equipmentManager.RotatorInfo();
            var response = RotatorStatusResponse.FromRotatorInfo(info, RotatorAction.CONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task RotatorDisconnect() {
            await equipmentManager.RotatorDisconnect();
            var info = equipmentManager.RotatorInfo();
            var response = RotatorStatusResponse.FromRotatorInfo(info, RotatorAction.DISCONNECTED);
            await RespondWithJSON(response);
        }

    }
}
