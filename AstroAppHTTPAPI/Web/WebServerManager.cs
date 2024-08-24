using EmbedIO;
using EmbedIO.WebApi;
using NINA.Core.Utility.Notification;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System;
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

        public WebServerManager(int port, EquipmentManager equipmentManager) {
            this.port = port;
            this.equipmentManager = equipmentManager;
        }

        private void CreateServer() {
            webSocketHandler = new WebSocketHandler("/events/v1", equipmentManager);
            server = new WebServer(o => o
                .WithUrlPrefix($"http://*:{port}")
                .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/api/v1", m => m.WithController(() => new RouteController(null, equipmentManager)))
                .WithModule(webSocketHandler);
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
                Notification.ShowSuccess($"Web server started, listening at //:{Port}");
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
                if (server != null) {
                    Restart();
                }
            }
        }
    }


}
