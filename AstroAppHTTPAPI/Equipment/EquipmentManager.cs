using NINA.Core.Model;
using NINA.Core.Utility;
using NINA.Equipment.Equipment.MyCamera;
using NINA.Equipment.Equipment.MyDome;
using NINA.Equipment.Equipment.MyTelescope;
using NINA.Equipment.Interfaces.Mediator;
using NINA.WPF.Base.Interfaces.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

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
    }


    public class DomeEventArgs : EventArgs {
        public DomeAction Action { get; set; }

        public DomeEventArgs(DomeAction action) {
            Action = action;
        }
    }

    public enum TelescopeAction {
        NONE = 0,
        CONNECTED = 1,
        DISCONNECTED = 2,
        PARKED = 3,
        HOMED = 4,
        SLEWED = 5,
        UNPARKED = 6,
    }

    public class TelescopeEventArgs : EventArgs {
        public TelescopeAction Action { get; set; }

        public TelescopeEventArgs(TelescopeAction action) {
            Action = action;
        }
    }

    public class EquipmentManager {
        private ICameraMediator camera;
        private IDomeMediator dome;
        private ITelescopeMediator telescope;
        private IApplicationStatusMediator statusMediator;
        private static CancellationTokenSource domeTokenSource;
        private static CancellationTokenSource telescopeTokenSource;
        public event EventHandler<CameraEventArgs> CameraUpdated;
        public event EventHandler<DomeEventArgs> DomeUpdated;
        public event EventHandler<TelescopeEventArgs> TelescopeUpdated;


        public EquipmentManager(IApplicationStatusMediator statusMediator, ICameraMediator camera, IDomeMediator dome, ITelescopeMediator telescope) {
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
            this.telescope = telescope;
            this.telescope.Connected += (sender, e) => {
                TelescopeUpdated?.Invoke(this, new TelescopeEventArgs(TelescopeAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.telescope.Disconnected += (sender, e) => {
                TelescopeUpdated?.Invoke(this, new TelescopeEventArgs(TelescopeAction.DISCONNECTED));
                return Task.CompletedTask;
            };
            this.telescope.Parked += (sender, e) => {
                TelescopeUpdated?.Invoke(this, new TelescopeEventArgs(TelescopeAction.PARKED));
                return Task.CompletedTask;
            };
            this.telescope.Homed += (sender, e) => {
                TelescopeUpdated?.Invoke(this, new TelescopeEventArgs(TelescopeAction.HOMED));
                return Task.CompletedTask;
            };
            this.telescope.Slewed += (sender, e) => {
                TelescopeUpdated?.Invoke(this, new TelescopeEventArgs(TelescopeAction.SLEWED));
                return Task.CompletedTask;
            };
            this.telescope.Unparked += (sender, e) => {
                TelescopeUpdated?.Invoke(this, new TelescopeEventArgs(TelescopeAction.UNPARKED));
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

        public async Task TelescopeConnect() {
            if (!telescope.GetInfo().Connected) {
                await telescope.Rescan();
                await telescope.Connect();
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

        public async Task TelescopeDisconnect() {
            if (telescope.GetInfo().Connected) {
                await telescope.Disconnect();
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

        public TelescopeInfo TelescopeInfo() {
            return telescope.GetInfo();
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

        public async Task TelescopePark() {
            var progress = new Progress<ApplicationStatus>(status => {
                Logger.Info($"TelescopePark: {status.Progress}");
            });
            if (telescope.GetInfo().Connected && !telescope.GetInfo().AtPark) {
                if (telescope.GetInfo().Slewing) {
                    telescope.StopSlew();
                }
                telescopeTokenSource?.Cancel();
                telescopeTokenSource = new CancellationTokenSource();
                await telescope.ParkTelescope(progress, telescopeTokenSource.Token);
            }
        }

        public async Task TelescopeUnpark() {
            var progress = new Progress<ApplicationStatus>(status => {
                Logger.Info($"TelescopeUnpark: {status.Progress}");
            });
            if (telescope.GetInfo().Connected && telescope.GetInfo().AtPark) {
                telescopeTokenSource?.Cancel();
                telescopeTokenSource = new CancellationTokenSource();
                await telescope.UnparkTelescope(progress, telescopeTokenSource.Token);
            }
        }

        public async Task DomeFindHome() {
            if (dome.GetInfo().Connected) {
                domeTokenSource?.Cancel();
                domeTokenSource = new CancellationTokenSource();
                await dome.FindHome(domeTokenSource.Token);
            }
        }
    }

}
