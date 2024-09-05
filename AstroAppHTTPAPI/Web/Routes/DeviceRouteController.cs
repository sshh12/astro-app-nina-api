using EmbedIO;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class DeviceRouteController : WebApiController {

        protected EquipmentManager equipmentManager;
        protected JsonSerializerSettings jsonSettings;

        public DeviceRouteController(IHttpContext context, EquipmentManager equipmentManager) {
            this.equipmentManager = equipmentManager;
            jsonSettings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Include
            };
        }

        public async Task RespondWithJSON(object? value) {
            HttpContext.Response.StatusCode = 200;
            HttpContext.Response.ContentType = "application/json";
            using (var writer = HttpContext.OpenResponseText()) {
                await writer.WriteAsync(JsonConvert.SerializeObject(value, jsonSettings));
            }
        }

    }

}