using EmbedIO;
using NINA.Core.Utility.Notification;
using System;
using System.Threading;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {


    public class WebServerManager {
        private Thread serverThread;
        private WebServer Server;
        private int port;

        public WebServerManager(int port) {
            this.port = port;
        }

        private void CreateServer() {
            Server = new WebServer(o => o
                .WithUrlPrefix($"http://*:{port}")
                .WithMode(HttpListenerMode.EmbedIO));
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
                Notification.ShowError("Failed to stop web server, see NINA log for details");
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
                Notification.ShowSuccess("Web server started, listening at:");
                server.RunAsync().Wait();
            } catch (Exception ex) {
                Notification.ShowError("Failed to start web server, see NINA log for details");
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
