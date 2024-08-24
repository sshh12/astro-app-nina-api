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
            this.equipmentManager.CameraUpdated += (object sender, CameraEventArgs e) => PostCameraStatus(e);
            jsonSettings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Include
            };
        }

        private void PostCameraStatus(CameraEventArgs e) {
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, e.Action);
            BroadcastAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        public void PostStatus() {
            PostCameraStatus(new CameraEventArgs(CameraAction.NONE));
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
