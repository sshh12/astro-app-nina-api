﻿using EmbedIO;
using EmbedIO.Authentication;
using EmbedIO.Cors;
using EmbedIO.WebApi;
using NINA.Core.Utility.Notification;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System;
using System.Linq;
using System.Threading;
using System.Timers;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class WebServerManager {
        private Thread serverThread;
        private WebServer server;
        private WebSocketHandler webSocketHandler;
        private int port;
        private EquipmentManager equipmentManager;
        private System.Timers.Timer statusTimer;
        private string apiKey;

        public WebServerManager(int port, string apiKey, EquipmentManager equipmentManager) {
            this.port = port;
            this.equipmentManager = equipmentManager;
            this.apiKey = apiKey;
        }

        private void CreateServer() {
            webSocketHandler = new WebSocketHandler("/events/v1", equipmentManager, apiKey);
            server = new WebServer(o => o
                .WithUrlPrefix($"http://*:{port}")
                .WithMode(HttpListenerMode.EmbedIO))
                .WithModule(new CorsModule("/", "*", "*", "*"))
                .WithModule(webSocketHandler)
                .WithModule(new BasicAuthenticationModule("/").WithAccount("user", apiKey))
                .WithWebApi("/api/v1/camera", m => m.WithController(() => new CameraRouteController(null, equipmentManager)))
                .WithWebApi("/api/v1/dome", m => m.WithController(() => new DomeRouteController(null, equipmentManager)))
                .WithWebApi("/api/v1/mount", m => m.WithController(() => new MountRouteController(null, equipmentManager)))
                .WithWebApi("/api/v1/rotator", m => m.WithController(() => new RotatorRouteController(null, equipmentManager)))
                .WithWebApi("/api/v1/switch", m => m.WithController(() => new SwitchRouteController(null, equipmentManager)))
                .WithWebApi("/api/v1/focuser", m => m.WithController(() => new FocuserRouteController(null, equipmentManager)))
                .WithWebApi("/api/v1/filterwheel", m => m.WithController(() => new FilterWheelRouteController(null, equipmentManager)))
                .WithWebApi("/api/v1/flatdevice", m => m.WithController(() => new FlatDeviceRouteController(null, equipmentManager)))
                .WithWebApi("/api/v1/safetymonitor", m => m.WithController(() => new SafetyMonitorController(null, equipmentManager)))
                .WithWebApi("/api/v1/weather", m => m.WithController(() => new WeatherRouteController(null, equipmentManager)));
        }

        public void Start() {
            if (server != null) {
                return;
            }
            CreateServer();
            serverThread = new Thread(() => RunServerTask(server));
            serverThread.Name = "WebServerThread";
            serverThread.SetApartmentState(ApartmentState.STA);
            serverThread.Start();
            statusTimer = new System.Timers.Timer(10000);
            statusTimer.Elapsed += OnStatusTimerElapsed;
            statusTimer.Start();
        }

        public void Stop() {
            if (server == null) {
                return;
            }
            try {
                server?.Dispose();
                server = null;
                statusTimer.Stop();
                statusTimer = null;
                Notification.ShowSuccess("Web server stopped");
            } catch (Exception ex) {
                Notification.ShowError($"Failed to stop web server {ex}");
            }
        }

        public void Restart() {
            if (server != null) {
                Stop();
            }
            Start();
        }

        [STAThread]
        private void RunServerTask(WebServer server) {
            try {
                var localAddress = server.Listener.Prefixes.FirstOrDefault();
                Notification.ShowSuccess($"Web server started, listening at {localAddress}");
                server.RunAsync().Wait();
            } catch (Exception ex) {
                Notification.ShowError($"Failed to start web server {ex}");
            }
        }

        private void OnStatusTimerElapsed(object sender, ElapsedEventArgs e) {
            webSocketHandler.PostStatus();
        }

        public int Port {
            get => port;
            set {
                port = value;
            }
        }

        public string ApiKey {
            get => apiKey;
            set {
                apiKey = value;
            }

        }

    }
}
