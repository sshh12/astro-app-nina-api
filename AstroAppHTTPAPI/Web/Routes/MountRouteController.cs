using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using NINA.Equipment.Equipment.MyTelescope;
using System.Threading.Tasks;
using System;
using NINA.Equipment.Interfaces;
using NINA.Astrometry;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class MountStatusResponse {
        public string Type = "MountStatus";
        public string Name { get; set; }
        public string UTCDate { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public bool Slewing { get; set; }
        public bool TrackingEnabled { get; set; }
        public string TrackingMode { get; set; }
        public string[] TrackingModes { get; set; }
        public bool AtHome { get; set; }
        public bool AtPark { get; set; }
        public double RightAscension { get; set; }
        public double Declination { get; set; }
        public double Azimuth { get; set; }
        public double Altitude { get; set; }
        public string SideOfPier { get; set; }
        public double SiteLatitude { get; set; }
        public double SiteLongitude { get; set; }
        public double SiteElevation { get; set; }
        public string AlignmentMode { get; set; }
        public bool IsPulseGuiding { get; set; }
        public string SiderealTime { get; set; }
        public double? GuideRateDeclination { get; set; }
        public double? GuideRateRightAscension { get; set; }
        public string Action { get; set; }

        public static MountStatusResponse FromMountInfo(TelescopeInfo info, MountAction action) {
            return new MountStatusResponse {
                Name = info.Name,
                UTCDate = info.UTCDate.ToString(),
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Slewing = info.Slewing,
                TrackingEnabled = info.TrackingEnabled,
                TrackingMode = info.TrackingRate.TrackingMode.ToString(),
                TrackingModes = info.TrackingRate.TrackingMode.GetType().GetEnumNames(),
                AtHome = info.AtHome,
                AtPark = info.AtPark,
                RightAscension = info.RightAscension,
                Declination = info.Declination,
                Azimuth = info.Azimuth,
                Altitude = info.Altitude,
                SideOfPier = info.SideOfPier.ToString(),
                SiteLatitude = info.SiteLatitude,
                SiteLongitude = info.SiteLongitude,
                SiteElevation = info.SiteElevation,
                AlignmentMode = info.AlignmentMode.ToString(),
                IsPulseGuiding = info.IsPulseGuiding,
                SiderealTime = info.SiderealTimeString,
                GuideRateDeclination = Double.IsNaN(info.GuideRateDeclinationArcsecPerSec) ? null : info.GuideRateDeclinationArcsecPerSec,
                GuideRateRightAscension = Double.IsNaN(info.GuideRateRightAscensionArcsecPerSec) ? null : info.GuideRateRightAscensionArcsecPerSec,
                Action = action.ToString(),
            };
        }

    }

    public class MountTrackingModeRequest {
        public string TrackingMode { get; set; }

        public TrackingMode ToTrackingMode() {
            return (TrackingMode)Enum.Parse(typeof(TrackingMode), TrackingMode);
        }
    }

    public class MountSlewRequest {
        public double RightAscension { get; set; }
        public double Declination { get; set; }
    }

    public class MountRouteController : DeviceRouteController {

        public MountRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        [Route(HttpVerbs.Get, "/")]
        public async Task MountStatus() {
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.NONE);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task MountConnect() {
            await equipmentManager.MountConnect();
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.CONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task MountDisconnect() {
            await equipmentManager.MountDisconnect();
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.DISCONNECTED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/park")]
        public async Task MountPark() {
            await equipmentManager.MountPark();
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.PARKED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/unpark")]
        public async Task MountUnpark() {
            await equipmentManager.MountUnpark();
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.UNPARKED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Patch, "/trackingmode")]
        public async Task MountSetTrackingMode([JsonData] MountTrackingModeRequest request) {
            await equipmentManager.MountSetTrackingMode(request.ToTrackingMode());
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.TRACKING_MODE_UPDATED);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Post, "/slew")]
        public async Task MountSlew([JsonData] MountSlewRequest request) {
            await equipmentManager.MountSlew(request.RightAscension, request.Declination, Epoch.JNOW, Coordinates.RAType.Hours);
            var info = equipmentManager.MountInfo();
            var response = MountStatusResponse.FromMountInfo(info, MountAction.NONE);
            await RespondWithJSON(response);
        }
    }
}
