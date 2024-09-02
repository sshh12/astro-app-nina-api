using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class FilterWheelRouteController : DeviceRouteController {

        public FilterWheelRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        private async void respondWithInfo(FilterWheelAction action) {
            var info = equipmentManager.FilterWheelInfo();
            var response = FilterWheelStatusResponse.FromFilterWheelInfo(info, action);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Get, "/")]
        public void FilterWheelStatus() {
            respondWithInfo(FilterWheelAction.NONE);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task FilterWheelConnect() {
            await equipmentManager.FilterWheelConnect();
            respondWithInfo(FilterWheelAction.CONNECTED);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task FilterWheelDisconnect() {
            await equipmentManager.FilterWheelDisconnect();
            respondWithInfo(FilterWheelAction.DISCONNECTED);
        }

        [Route(HttpVerbs.Patch, "/filter")]
        public async Task FilterWheelSetFilter([JsonData] FilterWheelFilterRequest request) {
            await equipmentManager.FilterWheelSetFilter(request.Position);
            respondWithInfo(FilterWheelAction.NONE);
        }


    }
}
