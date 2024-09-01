using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class FocuserRouteController : DeviceRouteController {

        public FocuserRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public void FocuserStatus() {
            var info = equipmentManager.FocuserInfo();
            var response = FocuserStatusResponse.FromFocuserInfo(info, FocuserAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task FocuserConnect() {
            await equipmentManager.FocuserConnect();
            var info = equipmentManager.FocuserInfo();
            var response = FocuserStatusResponse.FromFocuserInfo(info, FocuserAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task FocuserDisconnect() {
            await equipmentManager.FocuserDisconnect();
            var info = equipmentManager.FocuserInfo();
            var response = FocuserStatusResponse.FromFocuserInfo(info, FocuserAction.DISCONNECTED);
            RespondWithJSON(response);
        }

    }
}
