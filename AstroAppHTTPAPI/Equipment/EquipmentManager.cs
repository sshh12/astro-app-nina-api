using NINA.Equipment.Equipment.MyCamera;
using NINA.Equipment.Interfaces.Mediator;
using System;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Equipment {

    public class EquipmentManager {
        private ICameraMediator camera;
        public event EventHandler CameraUpdated;

        public EquipmentManager(ICameraMediator camera) {
            this.camera = camera;
            this.camera.Connected += (sender, e) => {
                CameraUpdated?.Invoke(this, EventArgs.Empty);
                return Task.CompletedTask;
            };
            this.camera.Disconnected += (sender, e) => {
                CameraUpdated?.Invoke(this, EventArgs.Empty);
                return Task.CompletedTask;
            };
        }

        public async Task CameraConnect() {
            if (!camera.GetInfo().Connected) {
                await camera.Rescan();
                await camera.Connect();
            }
            CameraUpdated?.Invoke(this, EventArgs.Empty);
        }

        public async Task CameraDisconnect() {
            if (camera.GetInfo().Connected) {
                await camera.Disconnect();
            }
            CameraUpdated?.Invoke(this, EventArgs.Empty);
        }

        public async Task CameraSetBinning(short x, short y) {
            camera.SetBinning(x, y);
            CameraUpdated?.Invoke(this, EventArgs.Empty);
            await Task.CompletedTask;
        }

        public CameraInfo CameraInfo() {
            return camera.GetInfo();
        }
    }

}
