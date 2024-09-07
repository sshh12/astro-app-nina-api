using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using NINA.Equipment.Equipment.MyDome;
using System.Threading.Tasks;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class DomeStatusResponse {
        public string Type = "DomeStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }

        public bool AtHome { get; set; }
        public bool AtPark { get; set; }
        public bool DriverFollowing { get; set; }
        public string ShutterStatus { get; set; }
        public double Azimuth { get; set; }
        public bool Slewing { get; set; }
        public bool IsFollowingScope { get; set; }
        public string Action { get; set; }

        public static DomeStatusResponse FromDomeInfo(DomeInfo info, bool following, DomeAction action) {
            return new DomeStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                AtHome = info.AtHome,
                AtPark = info.AtPark,
                IsFollowingScope = following,
                DriverFollowing = info.DriverFollowing,
                ShutterStatus = info.ShutterStatus.ToString(),
                Azimuth = info.Azimuth,
                Slewing = info.Slewing,
                Action = action.ToString(),
            };
        }
    }

    public class DomeSlewRequest {
        public double Azimuth { get; set; }
    }

    public class DomeFollowingRequest {
        public bool Enabled { get; set; }
    }

    public class DomeRouteController : DeviceRouteController {

        public DomeRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        private async Task respondWithInfo(DomeAction action) {
            var info = equipmentManager.DomeInfo();
            var following = equipmentManager.DomeIsFollowing();
            var response = DomeStatusResponse.FromDomeInfo(info, following, action);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Get, "/")]
        public async Task DomeStatus() {
            await respondWithInfo(DomeAction.NONE);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task DomeConnect() {
            await equipmentManager.DomeConnect();
            await respondWithInfo(DomeAction.CONNECTED);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task DomeDisconnect() {
            await equipmentManager.DomeDisconnect();
            await respondWithInfo(DomeAction.DISCONNECTED);
        }

        [Route(HttpVerbs.Post, "/open")]
        public async Task DomeOpen() {
            await equipmentManager.DomeSetShutterOpen(true);
            await respondWithInfo(DomeAction.OPENED);
        }

        [Route(HttpVerbs.Post, "/close")]
        public async Task DomeClose() {
            await equipmentManager.DomeSetShutterOpen(false);
            await respondWithInfo(DomeAction.CLOSED);
        }

        [Route(HttpVerbs.Post, "/rotate")]
        public async Task DomeSlew([JsonData] DomeSlewRequest request) {
            await equipmentManager.DomeSlew(request.Azimuth);
            await respondWithInfo(DomeAction.SLEWED);
        }

        [Route(HttpVerbs.Post, "/park")]
        public async Task DomePark() {
            await equipmentManager.DomePark();
            await respondWithInfo(DomeAction.PARKED);
        }

        [Route(HttpVerbs.Post, "/home")]
        public async Task DomeHome() {
            await equipmentManager.DomeFindHome();
            await respondWithInfo(DomeAction.HOMED);
        }

        [Route(HttpVerbs.Post, "/sync")]
        public async Task DomeSync() {
            await equipmentManager.DomeSyncToMount();
            await respondWithInfo(DomeAction.SYNCED);
        }

        [Route(HttpVerbs.Patch, "/following")]
        public async Task DomeSetFollowing([JsonData] DomeFollowingRequest request) {
            await equipmentManager.DomeSetFollowing(request.Enabled);
            await respondWithInfo(DomeAction.FOLLOWING_UPDATED);
        }
    }
}