using EmbedIO;
using EmbedIO.Authentication;
using EmbedIO.Cors;
using EmbedIO.WebApi;
using NINA.Core.Utility.Notification;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class WebServerManager {
        private Thread serverThread;
        private WebServer server;
        private WebSocketHandler webSocketHandler;
        private int port;
        private EquipmentManager equipmentManager;
        private System.Timers.Timer statusTimer;
        private string apiKey;
        public string ServerUrls { get; private set; } = "";
        private bool useHttps;
        private string certificatePath;
        private string certificatePassword;

        public WebServerManager(int port, bool useHttps, string apiKey, EquipmentManager equipmentManager, 
            string certificatePath, string certificatePassword) {
            this.port = port;
            this.equipmentManager = equipmentManager;
            this.apiKey = apiKey;
            this.useHttps = useHttps;
            this.certificatePath = certificatePath;
            this.certificatePassword = certificatePassword;
        }

        private void CreateServer() {
            webSocketHandler = new WebSocketHandler("/events/v1", equipmentManager, apiKey);
            
            var urlPrefix = useHttps ? $"https://*:{port}" : $"http://*:{port}";
            
            var options = new WebServerOptions()
                .WithUrlPrefix(urlPrefix)
                .WithMode(HttpListenerMode.EmbedIO);

            if (useHttps) {
                try {
                    // For HTTPS, we need to ensure the certificate is installed and bound to the port
                    options.WithCertificate(GetCertificate());
                } catch (Exception ex) {
                    Notification.ShowError($"HTTPS configuration failed: {ex.Message}");
                    // Fallback to HTTP
                    urlPrefix = $"http://*:{port}";
                    options.WithUrlPrefix(urlPrefix);
                }
            }

            server = new WebServer(options)
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
            
            UpdateServerUrls();
        }

        private void UpdateServerUrls() {
            try {
                var protocol = useHttps ? "https" : "http";
                var addresses = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(x => x.OperationalStatus == OperationalStatus.Up)
                    .SelectMany(x => x.GetIPProperties().UnicastAddresses)
                    .Where(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(x => $"{protocol}://{x.Address}:{port}")
                    .ToList();
                    
                ServerUrls = string.Join("\n", addresses);
                
                // Add localhost if not already included
                if (!addresses.Any(x => x.Contains("127.0.0.1"))) {
                    ServerUrls = $"{protocol}://127.0.0.1:{port}\n" + ServerUrls;
                }
            } catch (Exception ex) {
                ServerUrls = $"Error getting server URLs: {ex.Message}";
            }
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
            UpdateServerUrls();
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
                // Clear the ServerUrls when stopping
                ServerUrls = "";
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

        private X509Certificate2 GetCertificate() 
        {
            try {
                if (string.IsNullOrEmpty(certificatePath)) {
                    throw new Exception("No certificate path specified");
                }

                return new X509Certificate2(
                    certificatePath, 
                    certificatePassword ?? ""
                );
            }
            catch (Exception ex) {
                throw new Exception($"Failed to load certificate: {ex.Message}");
            }
        }
    }
}
