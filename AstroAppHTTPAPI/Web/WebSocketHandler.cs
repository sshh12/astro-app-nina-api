using EmbedIO.WebSockets;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class WebSocketHandler : WebSocketModule {

        private EquipmentManager equipmentManager;

        public WebSocketHandler(string urlPath, EquipmentManager equipmentManager)
            : base(urlPath, true) {
            this.equipmentManager = equipmentManager;
            this.equipmentManager.CameraUpdated += CameraUpdated;
        }

        private void CameraUpdated(object sender, System.EventArgs e) {
            BroadcastAsync($"Camera status: {equipmentManager.Camera.GetInfo().Connected}");
        }

        public void PostStatus() {
            BroadcastAsync($"Broadcast: {equipmentManager.Camera.GetInfo().Connected}");
        }

        protected override Task OnMessageReceivedAsync(
        IWebSocketContext context,
        byte[] rxBuffer,
        IWebSocketReceiveResult rxResult)
        => SendToOthersAsync(context, Encoding.GetString(rxBuffer));

        protected override Task OnClientConnectedAsync(IWebSocketContext context)
            => Task.WhenAll(
                SendAsync(context, "Welcome to the chat room!"),
                SendToOthersAsync(context, "Someone joined the chat room."));

        protected override Task OnClientDisconnectedAsync(IWebSocketContext context)
            => SendToOthersAsync(context, "Someone left the chat room.");

        private Task SendToOthersAsync(IWebSocketContext context, string payload)
            => BroadcastAsync(payload, c => c != context);
    }

}
