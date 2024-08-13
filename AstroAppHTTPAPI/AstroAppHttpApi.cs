using NINA.Core.Utility;
using NINA.Plugin;
using NINA.Plugin.Interfaces;
using NINA.Profile;
using NINA.Profile.Interfaces;
using NINA.WPF.Base.Interfaces.ViewModel;
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

        [ImportingConstructor]
        public AstroAppHttpApi(IProfileService profileService, IOptionsVM options) {
            if (Settings.Default.UpdateSettings) {
                Settings.Default.Upgrade();
                Settings.Default.UpdateSettings = false;
                CoreUtil.SaveSettings(Settings.Default);
            }
            this.pluginSettings = new PluginOptionsAccessor(profileService, Guid.Parse(this.Identifier));
            this.profileService = profileService;

            if (string.IsNullOrEmpty(ApiKey)) {
                ApiKey = GenerateRandomKey();
            }

            if (WebServerEnabled) {
                Logger.Info("Running Webserver");
            }
        }

        public override Task Teardown() {
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
            }
        }

        public int Port {
            get => Settings.Default.Port;
            set {
                Settings.Default.Port = value;
                CoreUtil.SaveSettings(Settings.Default);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Port)));
            }
        }

        public bool WebServerEnabled {
            get => Settings.Default.WebServerEnabled;
            set {
                Settings.Default.WebServerEnabled = value;
                CoreUtil.SaveSettings(Settings.Default);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WebServerEnabled)));
            }
        }

    }
}
