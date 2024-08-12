﻿using NINA.Core.Utility;
using NINA.Equipment.Interfaces;
using NINA.Equipment.Interfaces.ViewModel;
using NINA.Profile.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.AstroAppHttpApiDrivers {
    /// <summary>
    /// This Class shows the basic principle on how to add a new Device driver to N.I.N.A. via the plugin interface
    /// When the application scans for equipment the "GetEquipment" method of a device provider is called.
    /// This method should then return the specific List of Devices that you can connect to
    /// </summary>
    [Export(typeof(IEquipmentProvider))]
    public class AstroAppHttpApiWeatherProvider : IEquipmentProvider<IWeatherData> {
        private IProfileService profileService;

        [ImportingConstructor]
        public AstroAppHttpApiWeatherProvider(IProfileService profileService) {
            this.profileService = profileService;
        }

        public string Name => "AstroAppHttpApi";

        public IList<IWeatherData> GetEquipment() {
            var devices = new List<IWeatherData>();
            devices.Add(new AstroAppHttpApiWeatherDriver("MyUniqueId", "MyDeviceId"));

            return devices;
        }
    }
}
