using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class TelescopeRouteController : DeviceRouteController {

        public TelescopeRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async Task TelescopeStatus() {
            var info = equipmentManager.TelescopeInfo();
            var response = TelescopeStatusResponse.FromTelescopeInfo(info, TelescopeAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task TelescopeConnect() {
            await equipmentManager.TelescopeConnect();
            var info = equipmentManager.TelescopeInfo();
            var response = TelescopeStatusResponse.FromTelescopeInfo(info, TelescopeAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task TelescopeDisconnect() {
            await equipmentManager.TelescopeDisconnect();
            var info = equipmentManager.TelescopeInfo();
            var response = TelescopeStatusResponse.FromTelescopeInfo(info, TelescopeAction.DISCONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/park")]
        public async Task TelescopePark() {
            await equipmentManager.TelescopePark();
            var info = equipmentManager.TelescopeInfo();
            var response = TelescopeStatusResponse.FromTelescopeInfo(info, TelescopeAction.PARKED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/unpark")]
        public async Task TelescopeUnpark() {
            await equipmentManager.TelescopeUnpark();
            var info = equipmentManager.TelescopeInfo();
            var response = TelescopeStatusResponse.FromTelescopeInfo(info, TelescopeAction.UNPARKED);
            RespondWithJSON(response);
        }
    }
}
