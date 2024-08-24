using EmbedIO.WebSockets;
using Newtonsoft.Json;
using NINA.Core.Utility;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class WebSocketHandler : WebSocketModule {

        private EquipmentManager equipmentManager;
        private JsonSerializerSettings jsonSettings;
        private string apiKey;
        private HashSet<string> authedClients;

        public WebSocketHandler(string urlPath, EquipmentManager equipmentManager, string apiKey)
            : base(urlPath, true) {
            this.equipmentManager = equipmentManager;
            this.equipmentManager.CameraUpdated += async (object sender, CameraEventArgs e) => await PostCameraStatus(e);
            this.equipmentManager.DomeUpdated += async (object sender, DomeEventArgs e) => await PostDomeStatus(e);
            this.equipmentManager.TelescopeUpdated += async (object sender, TelescopeEventArgs e) => await PostTelescopeStatus(e);
            this.apiKey = apiKey;
            jsonSettings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Include
            };
            authedClients = new HashSet<string>();
        }

        private async Task PostCameraStatus(CameraEventArgs e) {
            var info = equipmentManager.CameraInfo();
            var response = CameraStatusResponse.FromCameraInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        private async Task PostDomeStatus(DomeEventArgs e) {
            var info = equipmentManager.DomeInfo();
            var response = DomeStatusResponse.FromDomeInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        private async Task PostTelescopeStatus(TelescopeEventArgs e) {
            var info = equipmentManager.TelescopeInfo();
            var response = TelescopeStatusResponse.FromTelescopeInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        public async Task BroadcastAuthedClientsAsync(string message) {
            await BroadcastAsync(message, (client) => authedClients.Contains(client.Id));
        }

        public void PostStatus() {
            Task.WaitAll(
                PostCameraStatus(new CameraEventArgs(CameraAction.NONE)),
                PostDomeStatus(new DomeEventArgs(DomeAction.NONE)),
                PostTelescopeStatus(new TelescopeEventArgs(TelescopeAction.NONE))
            );
        }

        protected override async Task OnMessageReceivedAsync(
                IWebSocketContext context,
                byte[] rxBuffer,
                IWebSocketReceiveResult rxResult) {
            var resp = Encoding.GetString(rxBuffer);
            var authSuccessful = false;
            try {
                var jsonMessage = JsonConvert.DeserializeObject<WebSocketAuthRequest>(resp);
                var reqApiKey = jsonMessage?.ApiKey;

                if (!string.IsNullOrEmpty(reqApiKey) && reqApiKey == apiKey) {
                    authSuccessful = true;
                }
            } catch (Exception) { }

            var socketResp = new WebSocketAuthenticatedResponse { Success = authSuccessful };
            if (authSuccessful) {
                Logger.Info($"WebSocket client authenticated: {context.Id}");
                authedClients.Add(context.Id);
                await SendAsync(context, JsonConvert.SerializeObject(socketResp, jsonSettings));
            } else {
                Logger.Info($"WebSocket client auth failed: {context.Id}");
                await SendAsync(context, JsonConvert.SerializeObject(socketResp, jsonSettings));
                await context.WebSocket.CloseAsync();
            }
        }

        protected override async Task OnClientConnectedAsync(IWebSocketContext context) {
            var response = new WebSocketConnectedResponse();
            Logger.Info($"WebSocket client connected: {context.Id}");
            await SendAsync(context, JsonConvert.SerializeObject(response, jsonSettings));
        }

        protected override Task OnClientDisconnectedAsync(IWebSocketContext context) {
            authedClients.Remove(context.Id);
            Logger.Info($"WebSocket client disconnected: {context.Id}");
            return Task.CompletedTask;
        }

    }

}
