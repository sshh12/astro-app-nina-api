using NINA.Core.Model;
using NINA.Core.Utility;
using NINA.Equipment.Equipment.MyCamera;
using NINA.Equipment.Equipment.MyDome;
using NINA.Equipment.Equipment.MyFilterWheel;
using NINA.Equipment.Equipment.MyTelescope;
using NINA.Equipment.Equipment.MyFocuser;
using NINA.Equipment.Equipment.MyGuider;
using NINA.Equipment.Equipment.MySwitch;
using NINA.Equipment.Equipment.MyRotator;
using NINA.Equipment.Equipment.MyFlatDevice;
using NINA.Core.Model.Equipment;
using NINA.Equipment.Equipment.MyWeatherData;
using NINA.Equipment.Equipment.MySafetyMonitor;
using NINA.Equipment.Interfaces.Mediator;
using NINA.WPF.Base.Interfaces.Mediator;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NINA.Astrometry;

namespace Plugin.NINA.AstroAppHTTPAPI.Equipment {

    public enum CameraAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2,
        BINNING_UPDATED = 3,
    }

    public class CameraEventArgs : EventArgs {
        public CameraAction Action { get; set; }

        public CameraEventArgs(CameraAction action) {
            Action = action;
        }
    }

    public enum DomeAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2,
        OPENED = 3,
        SYNCED = 4,
        CLOSED = 5,
        PARKED = 6,
        SLEWED = 7,
        HOMED = 8,
        FOLLOWING_UPDATED = 9,
    }


    public class DomeEventArgs : EventArgs {
        public DomeAction Action { get; set; }

        public DomeEventArgs(DomeAction action) {
            Action = action;
        }
    }

    public enum MountAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2,
        PARKED = 3,
        HOMED = 4,
        SLEWED = 5,
        UNPARKED = 6,
    }

    public class MountEventArgs : EventArgs {
        public MountAction Action { get; set; }

        public MountEventArgs(MountAction action) {
            Action = action;
        }
    }

    public enum FilterWheelAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2,
        FILTER_CHANGED = 3
    }

    public class FilterWheelEventArgs : EventArgs {
        public FilterWheelAction Action { get; set; }

        public FilterWheelEventArgs(FilterWheelAction action) {
            Action = action;
        }
    }

    public enum FocuserAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2,
        POSITION_UPDATED = 3
    }

    public class FocuserEventArgs : EventArgs {
        public FocuserAction Action { get; set; }

        public FocuserEventArgs(FocuserAction action) {
            Action = action;
        }
    }

    public enum GuiderAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2
    }

    public class GuiderEventArgs : EventArgs {
        public GuiderAction Action { get; set; }

        public GuiderEventArgs(GuiderAction action) {
            Action = action;
        }
    }

    public enum SwitchAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2
    }

    public class SwitchEventArgs : EventArgs {
        public SwitchAction Action { get; set; }

        public SwitchEventArgs(SwitchAction action) {
            Action = action;
        }
    }

    public enum RotatorAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2
    }

    public class RotatorEventArgs : EventArgs {
        public RotatorAction Action { get; set; }

        public RotatorEventArgs(RotatorAction action) {
            Action = action;
        }
    }

    public enum FlatDeviceAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2
    }

    public class FlatDeviceEventArgs : EventArgs {
        public FlatDeviceAction Action { get; set; }

        public FlatDeviceEventArgs(FlatDeviceAction action) {
            Action = action;
        }
    }

    public enum WeatherAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2
    }

    public class WeatherEventArgs : EventArgs {
        public WeatherAction Action { get; set; }

        public WeatherEventArgs(WeatherAction action) {
            Action = action;
        }
    }

    public enum SafetyMonitorAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2
    }

    public class SafetyMonitorEventArgs : EventArgs {
        public SafetyMonitorAction Action { get; set; }

        public SafetyMonitorEventArgs(SafetyMonitorAction action) {
            Action = action;
        }
    }

    public class EquipmentManager {
        private IApplicationStatusMediator statusMediator;
        private ICameraMediator camera;
        private IDomeMediator dome;
        private IFilterWheelMediator filterWheel;
        private IFocuserMediator focuser;
        private IGuiderMediator guider;
        private ISwitchMediator switches;
        private IRotatorMediator rotator;
        private IFlatDeviceMediator flatDevice;
        private IWeatherDataMediator weather;
        private ISafetyMonitorMediator safetyMonitor;
        private ITelescopeMediator mount;
        private static CancellationTokenSource domeTokenSource;
        private static CancellationTokenSource mountTokenSource;
        private static CancellationTokenSource filterWheelTokenSource;
        private static CancellationTokenSource focuserTokenSource;
        public event EventHandler<CameraEventArgs> CameraUpdated;
        public event EventHandler<DomeEventArgs> DomeUpdated;
        public event EventHandler<MountEventArgs> MountUpdated;
        public event EventHandler<FilterWheelEventArgs> FilterWheelUpdated;
        public event EventHandler<FocuserEventArgs> FocuserUpdated;
        public event EventHandler<GuiderEventArgs> GuiderUpdated;
        public event EventHandler<SwitchEventArgs> SwitchUpdated;
        public event EventHandler<RotatorEventArgs> RotatorUpdated;
        public event EventHandler<FlatDeviceEventArgs> FlatDeviceUpdated;
        public event EventHandler<WeatherEventArgs> WeatherUpdated;
        public event EventHandler<SafetyMonitorEventArgs> SafetyMonitorUpdated;

        public EquipmentManager(
                IApplicationStatusMediator statusMediator,
                ICameraMediator camera,
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
            this.statusMediator = statusMediator;
            this.camera = camera;
            this.camera.Connected += (sender, e) => {
                CameraUpdated?.Invoke(this, new CameraEventArgs(CameraAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.camera.Disconnected += (sender, e) => {
                CameraUpdated?.Invoke(this, new CameraEventArgs(CameraAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.dome = dome;
            this.dome.Connected += (sender, e) => {
                DomeUpdated?.Invoke(this, new DomeEventArgs(DomeAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.dome.Disconnected += (sender, e) => {
                DomeUpdated?.Invoke(this, new DomeEventArgs(DomeAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.dome.Opened += (sender, e) => {
                DomeUpdated?.Invoke(this, new DomeEventArgs(DomeAction.OPENED));
                return Task.CompletedTask;
            };
            this.dome.Synced += (sender, e) => {
                DomeUpdated?.Invoke(this, new DomeEventArgs(DomeAction.SYNCED));
            };
            this.dome.Closed += (sender, e) => {
                DomeUpdated?.Invoke(this, new DomeEventArgs(DomeAction.CLOSED));
                return Task.CompletedTask;
            };
            this.dome.Parked += (sender, e) => {
                DomeUpdated?.Invoke(this, new DomeEventArgs(DomeAction.PARKED));
                return Task.CompletedTask;
            };
            this.dome.Slewed += (sender, e) => {
                DomeUpdated?.Invoke(this, new DomeEventArgs(DomeAction.SLEWED));
                return Task.CompletedTask;
            };
            this.dome.Homed += (sender, e) => {
                DomeUpdated?.Invoke(this, new DomeEventArgs(DomeAction.HOMED));
                return Task.CompletedTask;
            };
            this.mount = mount;
            this.mount.Connected += (sender, e) => {
                MountUpdated?.Invoke(this, new MountEventArgs(MountAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.mount.Disconnected += (sender, e) => {
                MountUpdated?.Invoke(this, new MountEventArgs(MountAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.mount.Parked += (sender, e) => {
                MountUpdated?.Invoke(this, new MountEventArgs(MountAction.PARKED));
                return Task.CompletedTask;
            };
            this.mount.Homed += (sender, e) => {
                MountUpdated?.Invoke(this, new MountEventArgs(MountAction.HOMED));
                return Task.CompletedTask;
            };
            this.mount.Slewed += (sender, e) => {
                MountUpdated?.Invoke(this, new MountEventArgs(MountAction.SLEWED));
                return Task.CompletedTask;
            };
            this.mount.Unparked += (sender, e) => {
                MountUpdated?.Invoke(this, new MountEventArgs(MountAction.UNPARKED));
                return Task.CompletedTask;
            };
            this.filterWheel = filterWheel;
            this.filterWheel.Connected += (sender, e) => {
                FilterWheelUpdated?.Invoke(this, new FilterWheelEventArgs(FilterWheelAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.filterWheel.Disconnected += (sender, e) => {
                FilterWheelUpdated?.Invoke(this, new FilterWheelEventArgs(FilterWheelAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.filterWheel.FilterChanged += (sender, e) => {
                FilterWheelUpdated?.Invoke(this, new FilterWheelEventArgs(FilterWheelAction.FILTER_CHANGED));
                return Task.CompletedTask;
            };
            this.focuser = focuser;
            this.focuser.Connected += (sender, e) => {
                FocuserUpdated?.Invoke(this, new FocuserEventArgs(FocuserAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.focuser.Disconnected += (sender, e) => {
                FocuserUpdated?.Invoke(this, new FocuserEventArgs(FocuserAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.guider = guider;
            this.guider.Connected += (sender, e) => {
                GuiderUpdated?.Invoke(this, new GuiderEventArgs(GuiderAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.guider.Disconnected += (sender, e) => {
                GuiderUpdated?.Invoke(this, new GuiderEventArgs(GuiderAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.switches = switches;
            this.switches.Connected += (sender, e) => {
                SwitchUpdated?.Invoke(this, new SwitchEventArgs(SwitchAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.switches.Disconnected += (sender, e) => {
                SwitchUpdated?.Invoke(this, new SwitchEventArgs(SwitchAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.rotator = rotator;
            this.rotator.Connected += (sender, e) => {
                RotatorUpdated?.Invoke(this, new RotatorEventArgs(RotatorAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.rotator.Disconnected += (sender, e) => {
                RotatorUpdated?.Invoke(this, new RotatorEventArgs(RotatorAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.flatDevice = flatDevice;
            this.flatDevice.Connected += (sender, e) => {
                FlatDeviceUpdated?.Invoke(this, new FlatDeviceEventArgs(FlatDeviceAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.flatDevice.Disconnected += (sender, e) => {
                FlatDeviceUpdated?.Invoke(this, new FlatDeviceEventArgs(FlatDeviceAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.weather = weather;
            this.weather.Connected += (sender, e) => {
                WeatherUpdated?.Invoke(this, new WeatherEventArgs(WeatherAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.weather.Disconnected += (sender, e) => {
                WeatherUpdated?.Invoke(this, new WeatherEventArgs(WeatherAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.safetyMonitor = safetyMonitor;
            this.safetyMonitor.Connected += (sender, e) => {
                SafetyMonitorUpdated?.Invoke(this, new SafetyMonitorEventArgs(SafetyMonitorAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.safetyMonitor.Disconnected += (sender, e) => {
                SafetyMonitorUpdated?.Invoke(this, new SafetyMonitorEventArgs(SafetyMonitorAction.DISCONNECTED));
                return Task.CompletedTask;
            };
        }

        public async Task CameraConnect() {
            if (!camera.GetInfo().Connected) {
                await camera.Rescan();
                await camera.Connect();
            }
        }

        public async Task DomeConnect() {
            if (!dome.GetInfo().Connected) {
                await dome.Rescan();
                await dome.Connect();
            }
        }

        public async Task MountConnect() {
            if (!mount.GetInfo().Connected) {
                await mount.Rescan();
                await mount.Connect();
            }
        }

        public async Task FilterWheelConnect() {
            if (!filterWheel.GetInfo().Connected) {
                await filterWheel.Rescan();
                await filterWheel.Connect();
            }
        }

        public async Task FocuserConnect() {
            if (!focuser.GetInfo().Connected) {
                await focuser.Rescan();
                await focuser.Connect();
            }
        }

        public async Task GuiderConnect() {
            if (!guider.GetInfo().Connected) {
                await guider.Rescan();
                await guider.Connect();
            }
        }

        public async Task SwitchConnect() {
            if (!switches.GetInfo().Connected) {
                await switches.Rescan();
                await switches.Connect();
            }
        }

        public async Task RotatorConnect() {
            if (!rotator.GetInfo().Connected) {
                await rotator.Rescan();
                await rotator.Connect();
            }
        }

        public async Task FlatDeviceConnect() {
            if (!flatDevice.GetInfo().Connected) {
                await flatDevice.Rescan();
                await flatDevice.Connect();
            }
        }

        public async Task WeatherConnect() {
            if (!weather.GetInfo().Connected) {
                await weather.Rescan();
                await weather.Connect();
            }
        }

        public async Task SafetyMonitorConnect() {
            if (!safetyMonitor.GetInfo().Connected) {
                await safetyMonitor.Rescan();
                await safetyMonitor.Connect();
            }
        }

        public async Task CameraDisconnect() {
            if (camera.GetInfo().Connected) {
                await camera.Disconnect();
            }
        }

        public async Task DomeDisconnect() {
            if (dome.GetInfo().Connected) {
                await dome.Disconnect();
            }
        }

        public async Task MountDisconnect() {
            if (mount.GetInfo().Connected) {
                await mount.Disconnect();
            }
        }

        public async Task FilterWheelDisconnect() {
            if (filterWheel.GetInfo().Connected) {
                await filterWheel.Disconnect();
            }
        }

        public async Task FocuserDisconnect() {
            if (focuser.GetInfo().Connected) {
                await focuser.Disconnect();
            }
        }

        public async Task GuiderDisconnect() {
            if (guider.GetInfo().Connected) {
                await guider.Disconnect();
            }
        }

        public async Task SwitchDisconnect() {
            if (switches.GetInfo().Connected) {
                await switches.Disconnect();
            }
        }

        public async Task RotatorDisconnect() {
            if (rotator.GetInfo().Connected) {
                await rotator.Disconnect();
            }
        }

        public async Task FlatDeviceDisconnect() {
            if (flatDevice.GetInfo().Connected) {
                await flatDevice.Disconnect();
            }
        }

        public async Task WeatherDisconnect() {
            if (weather.GetInfo().Connected) {
                await weather.Disconnect();
            }
        }

        public async Task SafetyMonitorDisconnect() {
            if (safetyMonitor.GetInfo().Connected) {
                await safetyMonitor.Disconnect();
            }
        }

        public async Task CameraSetBinning(short x, short y) {
            camera.SetBinning(x, y);
            CameraUpdated?.Invoke(this, new CameraEventArgs(CameraAction.BINNING_UPDATED));
            await Task.CompletedTask;
        }

        public CameraInfo CameraInfo() {
            return camera.GetInfo();
        }

        public DomeInfo DomeInfo() {
            return dome.GetInfo();
        }

        public TelescopeInfo MountInfo() {
            return mount.GetInfo();
        }

        public FilterWheelInfo FilterWheelInfo() {
            return filterWheel.GetInfo();
        }

        public FocuserInfo FocuserInfo() {
            return focuser.GetInfo();
        }

        public GuiderInfo GuiderInfo() {
            return guider.GetInfo();
        }

        public SwitchInfo SwitchInfo() {
            return switches.GetInfo();
        }

        public RotatorInfo RotatorInfo() {
            return rotator.GetInfo();
        }

        public FlatDeviceInfo FlatDeviceInfo() {
            return flatDevice.GetInfo();
        }

        public WeatherDataInfo WeatherDataInfo() {
            return weather.GetInfo();
        }

        public SafetyMonitorInfo SafetyMonitorInfo() {
            return safetyMonitor.GetInfo();
        }

        public async Task DomeSetShutterOpen(bool open) {
            if (dome.GetInfo().Connected) {
                domeTokenSource?.Cancel();
                domeTokenSource = new CancellationTokenSource();
                if (open) {
                    await dome.OpenShutter(domeTokenSource.Token);
                } else {
                    await dome.CloseShutter(domeTokenSource.Token);
                }
            }
        }

        public async Task DomeSlew(double azimuth) {
            if (dome.GetInfo().Connected) {
                domeTokenSource?.Cancel();
                domeTokenSource = new CancellationTokenSource();
                await dome.SlewToAzimuth(azimuth, domeTokenSource.Token);
            }
        }

        public async Task DomePark() {
            if (dome.GetInfo().Connected && !dome.GetInfo().AtPark) {
                domeTokenSource?.Cancel();
                domeTokenSource = new CancellationTokenSource();
                await dome.Park(domeTokenSource.Token);
            }
        }

        public async Task MountPark() {
            var progress = new Progress<ApplicationStatus>(status => {
                Logger.Info($"TelescopePark: {status.Progress}");
            });
            if (mount.GetInfo().Connected && !mount.GetInfo().AtPark) {
                if (mount.GetInfo().Slewing) {
                    mount.StopSlew();
                }
                mountTokenSource?.Cancel();
                mountTokenSource = new CancellationTokenSource();
                await mount.ParkTelescope(progress, mountTokenSource.Token);
            }
        }

        public async Task MountUnpark() {
            var progress = new Progress<ApplicationStatus>(status => {
                Logger.Info($"TelescopeUnpark: {status.Progress}");
            });
            if (mount.GetInfo().Connected && mount.GetInfo().AtPark) {
                mountTokenSource?.Cancel();
                mountTokenSource = new CancellationTokenSource();
                await mount.UnparkTelescope(progress, mountTokenSource.Token);
            }
        }

        public async Task DomeFindHome() {
            if (dome.GetInfo().Connected) {
                domeTokenSource?.Cancel();
                domeTokenSource = new CancellationTokenSource();
                await dome.FindHome(domeTokenSource.Token);
            }
        }

        public bool DomeIsFollowing() {
            return dome.IsFollowingScope;
        }

        public async Task DomeSetFollowing(bool following) {
            if (dome.GetInfo().Connected) {
                domeTokenSource?.Cancel();
                domeTokenSource = new CancellationTokenSource();
                if (following) {
                    await dome.EnableFollowing(domeTokenSource.Token);
                } else {
                    await dome.DisableFollowing(domeTokenSource.Token);
                }
            }
        }

        public async Task DomeSyncToMount() {
            if (dome.GetInfo().Connected && mount.GetInfo().Connected) {
                domeTokenSource?.Cancel();
                domeTokenSource = new CancellationTokenSource();
                await dome.SyncToScopeCoordinates(mount.GetCurrentPosition(), mount.GetInfo().SideOfPier, domeTokenSource.Token);
            }
        }

        public async Task ScopeSlew(double ra, double dec, Epoch epoch, Coordinates.RAType ratype) {
            if (mount.GetInfo().Connected) {
                var coords = new Coordinates(ra, dec, epoch, ratype);
                mountTokenSource?.Cancel();
                mountTokenSource = new CancellationTokenSource();
                await mount.SlewToCoordinatesAsync(coords, mountTokenSource.Token);
            }
        }

        public async Task FilterWheelSetFilter(short pos) {
            if (filterWheel.GetInfo().Connected) {
                filterWheelTokenSource?.Cancel();
                filterWheelTokenSource = new CancellationTokenSource();
                // TODO: Do we need an offset?
                var filter = new FilterInfo("", 0, pos);
                await filterWheel.ChangeFilter(filter, filterWheelTokenSource.Token);
            }
        }

        public async Task FocuserSetPosition(int pos) {
            if (focuser.GetInfo().Connected) {
                focuserTokenSource?.Cancel();
                focuserTokenSource = new CancellationTokenSource();
                await focuser.MoveFocuser(pos, focuserTokenSource.Token);
            }
        }
    }

}
