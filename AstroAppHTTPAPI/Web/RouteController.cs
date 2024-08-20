﻿using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class RouteController : WebApiController {

        private EquipmentManager equipmentManager;

        public RouteController(IHttpContext context, EquipmentManager equipmentManager) {
            this.equipmentManager = equipmentManager;
        }

        [Route(HttpVerbs.Get, "/")]
        public string Index() {
            return "https://github.com/sshh12/astro-app-nina-api2";
        }

        [Route(HttpVerbs.Get, "/connect")]
        public string ConnectCamera() {
            equipmentManager.Camera.Connect();
            return "done";
        }

        [Route(HttpVerbs.Get, "/camera")]
        public string CameraStatus() {
            return equipmentManager.Camera.GetInfo().Connected.ToString();
        }
    }
}
