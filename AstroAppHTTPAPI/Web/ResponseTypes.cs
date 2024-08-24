using NINA.Equipment.Equipment.MyCamera;
using NINA.Equipment.Equipment.MyDome;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class CameraConnectedResponse {
        public string Type = "CameraConnected";
        public bool Success {
            get; set;
        }
        public string Name { get; set; }
        public string DeviceId { get; set; }
    }

    public class DomeConnectedResponse {
        public string Type = "DomeConnected";
        public bool Success {
            get; set;
        }
        public string Name { get; set; }
        public string DeviceId { get; set; }
    }

    public class CameraDisconnectedResponse {
        public string Type = "CameraDisconnected";
        public bool Success {
            get; set;
        }
    }

    public class DomeDisconnectedResponse {
        public string Type = "DomeDisconnected";
        public bool Success {
            get; set;
        }
    }

    public class CameraStatusResponse {
        public string Type = "CameraStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public int? Battery { get; set; }
        public bool Connected { get; set; }
        public int Offset { get; set; }
        public int? Gain { get; set; }
        public int? DefaultGain { get; set; }
        public double? Temperature { get; set; }
        public short BinX { get; set; }
        public short BinY { get; set; }
        public int XSize { get; set; }
        public int YSize { get; set; }
        public string SensorType { get; set; }
        public string Action { get; set; }



        public static CameraStatusResponse FromCameraInfo(CameraInfo info, CameraAction action) {
            return new CameraStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Battery = info.Battery < 0 ? null : info.Battery,
                Connected = info.Connected,
                Offset = info.Offset,
                Gain = info.Gain < 0 ? null : info.Gain,
                DefaultGain = info.DefaultGain < 0 ? null : info.DefaultGain,
                Temperature = Double.IsNaN(info.Temperature) ? null : info.Temperature,
                BinX = info.BinX,
                BinY = info.BinY,
                XSize = info.XSize,
                YSize = info.YSize,
                SensorType = info.SensorType.ToString(),
                Action = action.ToString(),
            };
        }

    }

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
        public string Action { get; set; }

        public static DomeStatusResponse FromDomeInfo(DomeInfo info, DomeAction action) {
            return new DomeStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                AtHome = info.AtHome,
                AtPark = info.AtPark,
                DriverFollowing = info.DriverFollowing,
                ShutterStatus = info.ShutterStatus.ToString(),
                Azimuth = info.Azimuth,
                Action = action.ToString(),
            };
        }
    }


    public class WebSocketConnectedResponse {
        public string Type = "WebSocketConnected";
    }

    public class WebSocketAuthenticatedResponse {
        public string Type = "WebSocketAuthenticated";
        public bool Success {
            get; set;
        }

    }

}