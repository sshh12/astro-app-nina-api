using NINA.Core.Utility;
using NINA.Core.Utility.Notification;
using NINA.Equipment.Interfaces.Mediator;
using NINA.Plugin;
using NINA.Plugin.Interfaces;
using NINA.Profile;
using NINA.Profile.Interfaces;
using NINA.WPF.Base.Interfaces.ViewModel;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using Plugin.NINA.AstroAppHTTPAPI.Web;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Settings = Plugin.NINA.AstroAppHTTPAPI.Properties.Settings;

namespace Plugin.NINA.AstroAppHTTPAPI {

    [Export(typeof(IPluginManifest))]
    public class AstroAppHttpApi : PluginBase, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IPluginOptionsAccessor pluginSettings;
        private readonly IProfileService profileService;
        private static WebServerManager serverManager;
        private EquipmentManager equipmentManager;

        public RelayCommand RestartServerCommand { get; set; }


        [ImportingConstructor]
        public AstroAppHttpApi(IProfileService profileService, IOptionsVM options, ICameraMediator camera, IDomeMediator dome) {
            if (Settings.Default.UpdateSettings) {
                Settings.Default.Upgrade();
                Settings.Default.UpdateSettings = false;
                CoreUtil.SaveSettings(Settings.Default);
            }
            pluginSettings = new PluginOptionsAccessor(profileService, Guid.Parse(Identifier));
            this.profileService = profileService;

            if (string.IsNullOrEmpty(ApiKey)) {
                ApiKey = GenerateRandomKey();
            }

            equipmentManager = new EquipmentManager(camera, dome);
            serverManager = new WebServerManager(Port, ApiKey, equipmentManager);

            if (WebServerEnabled) {
                serverManager.Start();
            }

            RestartServerCommand = new RelayCommand((obj) => {
                if (WebServerEnabled) {
                    serverManager.Stop();
                    serverManager.Start();
                } else {
                    Notification.ShowError("Start the server first!");
                }
            });
        }

        public override Task Teardown() {
            serverManager.Stop();
            return base.Teardown();
        }

        private string GenerateRandomKey() {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] key = new char[16];

            for (int i = 0; i < 16; i++) {
                key[i] = chars[random.Next(chars.Length)];
            }

            return new string(key);
        }

        public string ApiKey {
            get => Settings.Default.ApiKey;
            set {
                Settings.Default.ApiKey = value;
                CoreUtil.SaveSettings(Settings.Default);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ApiKey)));
                serverManager.ApiKey = value;
                if (WebServerEnabled) {
                    serverManager.Restart();
                }
            }
        }

        public int Port {
            get => Settings.Default.Port;
            set {
                Settings.Default.Port = value;
                CoreUtil.SaveSettings(Settings.Default);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Port)));
                serverManager.Port = value;
                if (WebServerEnabled) {
                    serverManager.Restart();
                }
            }
        }

        public bool WebServerEnabled {
            get => Settings.Default.WebServerEnabled;
            set {
                Settings.Default.WebServerEnabled = value;
                CoreUtil.SaveSettings(Settings.Default);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WebServerEnabled)));
                if (value) {
                    serverManager.Start();
                } else {
                    serverManager.Stop();
                }
            }
        }

    }
}
