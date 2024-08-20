using EmbedIO;
using EmbedIO.WebApi;
using NINA.Core.Utility.Notification;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System;
using System.Threading;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {


    public class WebServerManager {
        private Thread serverThread;
        private WebServer Server;
        private int port;
        private EquipmentManager equipmentManager;

        public WebServerManager(int port, EquipmentManager equipmentManager) {
            this.port = port;
            this.equipmentManager = equipmentManager;
        }

        private void CreateServer() {
            Server = new WebServer(o => o
                .WithUrlPrefix($"http://*:{port}")
                .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/api/v1", m => m.WithController(() => new RouteController(null, equipmentManager)));
        }

        public void Start() {
            if (Server != null) {
                return;
            }
            CreateServer();
            serverThread = new Thread(() => RunServerTask(Server));
            serverThread.Name = "WebServerThread";
            serverThread.SetApartmentState(ApartmentState.STA);
            serverThread.Start();
        }

        public void Stop() {
            if (Server == null) {
                return;
            }
            try {
                Server?.Dispose();
                Server = null;
                Notification.ShowSuccess("Web server stopped");
            } catch (Exception ex) {
                Notification.ShowError($"Failed to stop web server {ex}");
            }
        }

        public void Restart() {
            if (Server != null) {
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

        public int Port {
            get => port;
            set {
                port = value;
                if (Server != null) {
                    Restart();
                }
            }
        }
    }


}
