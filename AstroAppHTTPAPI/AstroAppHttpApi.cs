﻿using NINA.Core.Utility;
using NINA.Core.Utility.Notification;
using NINA.Equipment.Interfaces.Mediator;
using NINA.Plugin;
using NINA.Plugin.Interfaces;
using NINA.Profile.Interfaces;
using NINA.WPF.Base.Interfaces.Mediator;
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
        private static WebServerManager serverManager;
        private EquipmentManager equipmentManager;

        public RelayCommand RestartServerCommand { get; set; }

        [ImportingConstructor]
        public AstroAppHttpApi(
                IProfileService profileService,
                IOptionsVM options,
                IApplicationStatusMediator statusMediator,
                ICameraMediator camera,
                IImagingMediator imaging,
                IImageSaveMediator imageSave,
                IDomeMediator dome,
                IFilterWheelMediator filterWheel,
                IFocuserMediator focuser,
                IGuiderMediator guider,
                ISwitchMediator switches,
                IRotatorMediator rotator,
                IFlatDeviceMediator flatDevice,
                IWeatherDataMediator weather,
                ISafetyMonitorMediator safetyMonitor,
                ITelescopeMediator mount
        ) {
            if (Settings.Default.UpdateSettings) {
                Settings.Default.Upgrade();
                Settings.Default.UpdateSettings = false;
                CoreUtil.SaveSettings(Settings.Default);
            }

            if (string.IsNullOrEmpty(ApiKey)) {
                ApiKey = GenerateRandomKey();
            }

            equipmentManager = new EquipmentManager(
                statusMediator,
                camera,
                imaging,
                imageSave,
                dome,
                filterWheel,
                focuser,
                guider,
                switches,
                rotator,
                flatDevice,
                weather,
                safetyMonitor,
                mount
            );
            serverManager = new WebServerManager(
                Port, 
                EnableSSL, 
                ApiKey, 
                equipmentManager,
                CertificatePath,
                CertificatePassword
            );

            if (WebServerEnabled) {
                serverManager.Start();
            }

            RestartServerCommand = new RelayCommand((obj) => {
                if (WebServerEnabled) {
                    serverManager.Stop();
                    serverManager.Start();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServerUrls)));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServerUrls)));
            }
        }

        public string ServerUrls {
            get => serverManager?.ServerUrls ?? "";
            private set {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServerUrls)));
            }
        }

        public bool EnableSSL {
            get => Settings.Default.EnableSSL;
            set {
                Settings.Default.EnableSSL = value;
                CoreUtil.SaveSettings(Settings.Default);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EnableSSL)));
                if (WebServerEnabled) {
                    serverManager.Stop();
                    serverManager = new WebServerManager(
                        Port, 
                        value, 
                        ApiKey, 
                        equipmentManager,
                        CertificatePath,
                        CertificatePassword
                    );
                    serverManager.Start();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServerUrls)));
                }
            }
        }

        public string CertificatePath {
            get => Settings.Default.CertificatePath;
            set {
                Settings.Default.CertificatePath = value;
                CoreUtil.SaveSettings(Settings.Default);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CertificatePath)));
                if (WebServerEnabled && EnableSSL) {
                    serverManager.Restart();
                }
            }
        }

        public string CertificatePassword {
            get => Settings.Default.CertificatePassword;
            set {
                Settings.Default.CertificatePassword = value;
                CoreUtil.SaveSettings(Settings.Default);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CertificatePassword)));
                if (WebServerEnabled && EnableSSL) {
                    serverManager.Restart();
                }
            }
        }

    }
}
