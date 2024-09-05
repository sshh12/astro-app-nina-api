using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;
using NINA.Equipment.Equipment.MyFocuser;
using System;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class FocuserStatusResponse {
        public string Type = "FocuserStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public int Position { get; set; }
        public bool TempComp { get; set; }
        public bool TempCompAvailable { get; set; }
        public double? Temperature { get; set; }
        public double? StepSize { get; set; }
        public string Action { get; set; }

        public static FocuserStatusResponse FromFocuserInfo(FocuserInfo info, FocuserAction action) {
            return new FocuserStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Position = info.Position,
                TempComp = info.TempComp,
                TempCompAvailable = info.TempCompAvailable,
                Temperature = Double.IsNaN(info.Temperature) ? null : info.Temperature,
                StepSize = Double.IsNaN(info.StepSize) ? null : info.StepSize,
                Action = action.ToString(),
            };
        }
    }

    public class FocuserPositionRequest {
        public int Position { get; set; }
    }

    public class FocuserRouteController : DeviceRouteController {

        public FocuserRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        private async Task respondWithInfo(FocuserAction action) {
            var info = equipmentManager.FocuserInfo();
            var response = FocuserStatusResponse.FromFocuserInfo(info, action);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Get, "/")]
        public async Task FocuserStatus() {
            await respondWithInfo(FocuserAction.NONE);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task FocuserConnect() {
            await equipmentManager.FocuserConnect();
            await respondWithInfo(FocuserAction.CONNECTED);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task FocuserDisconnect() {
            await equipmentManager.FocuserDisconnect();
            await respondWithInfo(FocuserAction.DISCONNECTED);
        }

        [Route(HttpVerbs.Patch, "/position")]
        public async Task FocuserSetPosition([JsonData] FocuserPositionRequest request) {
            await equipmentManager.FocuserSetPosition(request.Position);
            await respondWithInfo(FocuserAction.POSITION_UPDATED);
        }

    }
}
