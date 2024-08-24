using EmbedIO.WebSockets;
using Newtonsoft.Json;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class WebSocketHandler : WebSocketModule {

        private EquipmentManager equipmentManager;
        private JsonSerializerSettings jsonSettings;

        public WebSocketHandler(string urlPath, EquipmentManager equipmentManager)
            : base(urlPath, true) {
            this.equipmentManager = equipmentManager;
            this.equipmentManager.CameraUpdated += (object sender, System.EventArgs e) => PostCameraStatus();
            jsonSettings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Include
            };
        }

        private void PostCameraStatus() {
            var info = equipmentManager.Camera.GetInfo();
            var response = CameraStatusResponse.FromCameraInfo(info);
            BroadcastAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        public void PostStatus() {
            PostCameraStatus();
        }

        protected override Task OnMessageReceivedAsync(
                IWebSocketContext context,
                byte[] rxBuffer,
                IWebSocketReceiveResult rxResult) {
            return Task.CompletedTask;
        }

        protected override Task OnClientConnectedAsync(IWebSocketContext context) {
            var response = new WebSocketConnectedResponse();
            return SendAsync(context, JsonConvert.SerializeObject(response, jsonSettings));
        }


    }

}
