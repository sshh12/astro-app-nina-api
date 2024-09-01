using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class WeatherRouteController : DeviceRouteController {

        public WeatherRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public void WeatherStatus() {
            var info = equipmentManager.WeatherDataInfo();
            var response = WeatherStatusResponse.FromWeatherInfo(info, WeatherAction.NONE);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task WeatherConnect() {
            await equipmentManager.WeatherConnect();
            var info = equipmentManager.WeatherDataInfo();
            var response = WeatherStatusResponse.FromWeatherInfo(info, WeatherAction.CONNECTED);
            RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task WeatherDisconnect() {
            await equipmentManager.WeatherDisconnect();
            var info = equipmentManager.WeatherDataInfo();
            var response = WeatherStatusResponse.FromWeatherInfo(info, WeatherAction.DISCONNECTED);
            RespondWithJSON(response);
        }

    }
}
