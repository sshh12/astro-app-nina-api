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
            this.equipmentManager.MountUpdated += async (object sender, MountEventArgs e) => await PostMountStatus(e);
            this.equipmentManager.FilterWheelUpdated += async (object sender, FilterWheelEventArgs e) => await PostFilterWheelStatus(e);
            this.equipmentManager.FocuserUpdated += async (object sender, FocuserEventArgs e) => await PostFocuserStatus(e);
            this.equipmentManager.GuiderUpdated += async (object sender, GuiderEventArgs e) => await PostGuiderStatus(e);
            this.equipmentManager.RotatorUpdated += async (object sender, RotatorEventArgs e) => await PostRotatorStatus(e);
            this.equipmentManager.FlatDeviceUpdated += async (object sender, FlatDeviceEventArgs e) => await PostFlatDeviceStatus(e);
            this.equipmentManager.WeatherUpdated += async (object sender, WeatherEventArgs e) => await PostWeatherStatus(e);
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

        private async Task PostMountStatus(MountEventArgs e) {
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        private async Task PostFilterWheelStatus(FilterWheelEventArgs e) {
            var info = equipmentManager.FilterWheelInfo();
            var response = FilterWheelStatusResponse.FromFilterWheelInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        private async Task PostFocuserStatus(FocuserEventArgs e) {
            var info = equipmentManager.FocuserInfo();
            var response = FocuserStatusResponse.FromFocuserInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        private async Task PostGuiderStatus(GuiderEventArgs e) {
            var info = equipmentManager.GuiderInfo();
            var response = GuiderStatusResponse.FromGuiderInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        private async Task PostRotatorStatus(RotatorEventArgs e) {
            var info = equipmentManager.RotatorInfo();
            var response = RotatorStatusResponse.FromRotatorInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        private async Task PostFlatDeviceStatus(FlatDeviceEventArgs e) {
            var info = equipmentManager.FlatDeviceInfo();
            var response = FlatDeviceStatusResponse.FromFlatDeviceInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        private async Task PostWeatherStatus(WeatherEventArgs e) {
            var info = equipmentManager.WeatherDataInfo();
            var response = WeatherStatusResponse.FromWeatherInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        private async Task PostSafetyMonitorStatus(SafetyMonitorEventArgs e) {
            var info = equipmentManager.SafetyMonitorInfo();
            var response = SafetyMonitorStatusResponse.FromSafetyMonitorInfo(info, e.Action);
            await BroadcastAuthedClientsAsync(JsonConvert.SerializeObject(response, jsonSettings));
        }

        public async Task BroadcastAuthedClientsAsync(string message) {
            await BroadcastAsync(message, (client) => authedClients.Contains(client.Id));
        }

        public void PostStatus() {
            Task.WaitAll(
                PostCameraStatus(new CameraEventArgs(CameraAction.NONE)),
                PostDomeStatus(new DomeEventArgs(DomeAction.NONE)),
                PostMountStatus(new MountEventArgs(MountAction.NONE)),
                PostFilterWheelStatus(new FilterWheelEventArgs(FilterWheelAction.NONE)),
                PostFocuserStatus(new FocuserEventArgs(FocuserAction.NONE)),
                PostGuiderStatus(new GuiderEventArgs(GuiderAction.NONE)),
                PostRotatorStatus(new RotatorEventArgs(RotatorAction.NONE)),
                PostFlatDeviceStatus(new FlatDeviceEventArgs(FlatDeviceAction.NONE)),
                PostWeatherStatus(new WeatherEventArgs(WeatherAction.NONE)),
                PostSafetyMonitorStatus(new SafetyMonitorEventArgs(SafetyMonitorAction.NONE))
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
                PostStatus();
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
