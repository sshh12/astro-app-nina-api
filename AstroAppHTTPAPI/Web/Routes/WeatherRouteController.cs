using EmbedIO;
using EmbedIO.Routing;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;
using NINA.Equipment.Equipment.MyWeatherData;
using System;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class WeatherStatusResponse {
        public string Type = "WeatherStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? DewPoint { get; set; }
        public double? WindSpeed { get; set; }
        public double? WindDirection { get; set; }
        public double? Pressure { get; set; }
        public double? SkyQuality { get; set; }
        public double? SkyBrightness { get; set; }
        public double? RainRate { get; set; }
        public double? CloudCover { get; set; }
        public double? StarFWHM { get; set; }
        public string Action { get; set; }

        public static WeatherStatusResponse FromWeatherInfo(WeatherDataInfo info, WeatherAction action) {
            return new WeatherStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Temperature = Double.IsNaN(info.Temperature) ? null : info.Temperature,
                Humidity = Double.IsNaN(info.Humidity) ? null : info.Humidity,
                DewPoint = Double.IsNaN(info.DewPoint) ? null : info.DewPoint,
                WindSpeed = Double.IsNaN(info.WindSpeed) ? null : info.WindSpeed,
                WindDirection = Double.IsNaN(info.WindDirection) ? null : info.WindDirection,
                Pressure = Double.IsNaN(info.Pressure) ? null : info.Pressure,
                SkyQuality = Double.IsNaN(info.SkyQuality) ? null : info.SkyQuality,
                SkyBrightness = Double.IsNaN(info.SkyBrightness) ? null : info.SkyBrightness,
                RainRate = Double.IsNaN(info.RainRate) ? null : info.RainRate,
                CloudCover = Double.IsNaN(info.CloudCover) ? null : info.CloudCover,
                StarFWHM = Double.IsNaN(info.StarFWHM) ? null : info.StarFWHM,
                Action = action.ToString(),
            };
        }
    }

    public class WeatherRouteController : DeviceRouteController {

        public WeatherRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async void WeatherStatus() {
            var info = equipmentManager.WeatherDataInfo();
            var response = WeatherStatusResponse.FromWeatherInfo(info, WeatherAction.NONE);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task WeatherConnect() {
            await equipmentManager.WeatherConnect();
            var info = equipmentManager.WeatherDataInfo();
            var response = WeatherStatusResponse.FromWeatherInfo(info, WeatherAction.CONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task WeatherDisconnect() {
            await equipmentManager.WeatherDisconnect();
            var info = equipmentManager.WeatherDataInfo();
            var response = WeatherStatusResponse.FromWeatherInfo(info, WeatherAction.DISCONNECTED);
            await RespondWithJSON(response);
        }

    }
}
