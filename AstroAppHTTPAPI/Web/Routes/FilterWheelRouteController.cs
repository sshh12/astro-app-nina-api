using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class FilterWheelRouteController : DeviceRouteController {

        public FilterWheelRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public void FilterWheelStatus() {
            var info = equipmentManager.FilterWheelInfo();
            var response = FilterWheelStatusResponse.FromFilterWheelInfo(info, FilterWheelAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task FilterWheelConnect() {
            await equipmentManager.FilterWheelConnect();
            var info = equipmentManager.FilterWheelInfo();
            var response = FilterWheelStatusResponse.FromFilterWheelInfo(info, FilterWheelAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task FilterWheelDisconnect() {
            await equipmentManager.FilterWheelDisconnect();
            var info = equipmentManager.FilterWheelInfo();
            var response = FilterWheelStatusResponse.FromFilterWheelInfo(info, FilterWheelAction.DISCONNECTED);
            RespondWithJSON(response);
        }

    }
}
