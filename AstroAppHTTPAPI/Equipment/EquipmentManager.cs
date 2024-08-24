using NINA.Equipment.Interfaces.Mediator;
using System;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Equipment {

    public class EquipmentManager {
        public ICameraMediator Camera;
        public event EventHandler CameraUpdated;
        public async Task CameraConnect() {
            if (!Camera.GetInfo().Connected) {
                await Camera.Rescan();
                await Camera.Connect();
            }
            CameraUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

}
