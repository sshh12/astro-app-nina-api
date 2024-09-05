using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using NINA.Equipment.Equipment.MyFilterWheel;
using System.Threading.Tasks;
using NINA.Core.Model.Equipment;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

    public class Filter {
        public string Name { get; set; }
        public short Position { get; set; }

        public static Filter FromFilterInfo(FilterInfo info) {
            return new Filter {
                Name = info.Name,
                Position = info.Position,
            };
        }

        public static Filter Empty() {
            return new Filter {
                Name = "Unknown",
                Position = 0,
            };
        }
    }

    public class FilterWheelStatusResponse {
        public string Type = "FilterWheelStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public Filter SelectedFilter { get; set; }
        public string Action { get; set; }

        public static FilterWheelStatusResponse FromFilterWheelInfo(FilterWheelInfo info, FilterWheelAction action) {
            return new FilterWheelStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                SelectedFilter = info.SelectedFilter == null ? Filter.Empty() : Filter.FromFilterInfo(info.SelectedFilter),
                Action = action.ToString(),
            };
        }
    }

    public class FilterWheelFilterRequest {
        public short Position { get; set; }
    }

    public class FilterWheelRouteController : DeviceRouteController {

        public FilterWheelRouteController(IHttpContext context, EquipmentManager equipmentManager) : base(context, equipmentManager) { }

        private async Task respondWithInfo(FilterWheelAction action) {
            var info = equipmentManager.FilterWheelInfo();
            var response = FilterWheelStatusResponse.FromFilterWheelInfo(info, action);
            await RespondWithJSON(response);
        }

        [Route(HttpVerbs.Get, "/")]
        public async Task FilterWheelStatus() {
            await respondWithInfo(FilterWheelAction.NONE);
        }

        [Route(HttpVerbs.Post, "/connect")]
        public async Task FilterWheelConnect() {
            await equipmentManager.FilterWheelConnect();
            await respondWithInfo(FilterWheelAction.CONNECTED);
        }

        [Route(HttpVerbs.Post, "/disconnect")]
        public async Task FilterWheelDisconnect() {
            await equipmentManager.FilterWheelDisconnect();
            await respondWithInfo(FilterWheelAction.DISCONNECTED);
        }

        [Route(HttpVerbs.Patch, "/filter")]
        public async Task FilterWheelSetFilter([JsonData] FilterWheelFilterRequest request) {
            await equipmentManager.FilterWheelSetFilter(request.Position);
            await respondWithInfo(FilterWheelAction.NONE);
        }


    }
}
