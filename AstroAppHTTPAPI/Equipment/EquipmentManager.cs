using NINA.Equipment.Equipment.MyCamera;
using NINA.Equipment.Interfaces.Mediator;
using System;
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

    public class EquipmentManager {
        private ICameraMediator camera;
        public event EventHandler<CameraEventArgs> CameraUpdated;

        public EquipmentManager(ICameraMediator camera) {
            this.camera = camera;
            this.camera.Connected += (sender, e) => {
                CameraUpdated?.Invoke(this, new CameraEventArgs(CameraAction.CONNECTED));
                return Task.CompletedTask;
            };
            this.camera.Disconnected += (sender, e) => {
                CameraUpdated?.Invoke(this, new CameraEventArgs(CameraAction.DISCONNECTED));
                return Task.CompletedTask;
            };
        }

        public async Task CameraConnect() {
            if (!camera.GetInfo().Connected) {
                await camera.Rescan();
                await camera.Connect();
            }
        }

        public async Task CameraDisconnect() {
            if (camera.GetInfo().Connected) {
                await camera.Disconnect();
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
    }

}
