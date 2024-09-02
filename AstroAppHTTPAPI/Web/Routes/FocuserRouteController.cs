using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class FocuserRouteController : DeviceRouteController {

        public FocuserRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        private void respondWithInfo(FocuserAction action) {
            var info = equipmentManager.FocuserInfo();
            var response = FocuserStatusResponse.FromFocuserInfo(info, action);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Get, "/")]
        public void FocuserStatus() {
            respondWithInfo(FocuserAction.NONE);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task FocuserConnect() {
            await equipmentManager.FocuserConnect();
            respondWithInfo(FocuserAction.CONNECTED);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task FocuserDisconnect() {
            await equipmentManager.FocuserDisconnect();
            respondWithInfo(FocuserAction.DISCONNECTED);
        }

        [Route(HttpVerbs.Patch, "/position")]
        public async Task FocuserSetPosition([JsonData] FocuserPositionRequest request) {
            await equipmentManager.FocuserSetPosition(request.Position);
            respondWithInfo(FocuserAction.POSITION_UPDATED);
        }

    }
}
