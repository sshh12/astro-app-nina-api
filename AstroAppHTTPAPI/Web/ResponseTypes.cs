using NINA.Equipment.Equipment.MyCamera;
using NINA.Equipment.Equipment.MyDome;
using NINA.Equipment.Equipment.MyTelescope;
using NINA.Equipment.Equipment.MyFilterWheel;
using NINA.Equipment.Equipment.MyFocuser;
using NINA.Equipment.Equipment.MyRotator;
using NINA.Equipment.Equipment.MyGuider;
using NINA.Equipment.Equipment.MySwitch;
using NINA.Equipment.Equipment.MyFlatDevice;
using NINA.Equipment.Equipment.MyWeatherData;
using NINA.Equipment.Equipment.MySafetyMonitor;
using Plugin.NINA.AstroAppHTTPAPI.Equipment;
using System;
using NINA.Core.Model.Equipment;
using NINA.Equipment.Interfaces;
using System.Collections.Immutable;
using System.Linq;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {

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
        public double? PixelSize { get; set; }
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
                PixelSize = Double.IsNaN(info.PixelSize) ? null : info.PixelSize,
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

    public class FocuserStatusResponse {
        public string Type = "FocuserStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public int Position { get; set; }
        public bool TempComp { get; set; }
        public bool TempCompAvailable { get; set; }
        public double? Temperature { get; set; }
        public double? StepSize { get; set; }
        public string Action { get; set; }

        public static FocuserStatusResponse FromFocuserInfo(FocuserInfo info, FocuserAction action) {
            return new FocuserStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Position = info.Position,
                TempComp = info.TempComp,
                TempCompAvailable = info.TempCompAvailable,
                Temperature = Double.IsNaN(info.Temperature) ? null : info.Temperature,
                StepSize = Double.IsNaN(info.StepSize) ? null : info.StepSize,
                Action = action.ToString(),
            };
        }
    }

    public class RotatorStatusResponse {
        public string Type = "RotatorStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public double? Position { get; set; }
        public bool IsMoving { get; set; }
        public double? StepSize { get; set; }
        public float? MechanicalPosition { get; set; }
        public bool Reverse { get; set; }
        public string Action { get; set; }

        public static RotatorStatusResponse FromRotatorInfo(RotatorInfo info, RotatorAction action) {
            return new RotatorStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Position = Double.IsNaN(info.Position) ? null : info.Position,
                IsMoving = info.IsMoving,
                StepSize = Double.IsNaN(info.StepSize) ? null : info.StepSize,
                MechanicalPosition = float.IsNaN(info.MechanicalPosition) ? null : info.MechanicalPosition,
                Reverse = info.Reverse,
                Action = action.ToString(),
            };
        }
    }

    public class GuiderStatusResponse {
        public string Type = "GuiderStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public double? PixelScale { get; set; }
        public string Action { get; set; }

        public static GuiderStatusResponse FromGuiderInfo(GuiderInfo info, GuiderAction action) {
            return new GuiderStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                PixelScale = Double.IsNaN(info.PixelScale) ? null : info.PixelScale,
                Action = action.ToString(),
            };
        }
    }

    public class Gauge {
        public string Name { get; set; }
        public string Description { get; set; }
        public double? Value { get; set; }

        public static Gauge FromSwitch(ISwitch info) {
            return new Gauge {
                Name = info.Name,
                Description = info.Description,
                Value = Double.IsNaN(info.Value) ? null : info.Value,
            };
        }
    }

    public class SwitchStatusResponse {
        public string Type = "SwitchStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public Gauge[] Gauges { get; set; }
        public string Action { get; set; }

        public static SwitchStatusResponse FromSwitchInfo(SwitchInfo info, SwitchAction action) {
            return new SwitchStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Gauges = info.ReadonlySwitches?.ToImmutableList().Select(s => Gauge.FromSwitch(s)).ToArray(),
                Action = action.ToString(),
            };
        }
    }

    public class FlatDeviceStatusResponse {
        public string Type = "FlatDeviceStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public int Brightness { get; set; }
        public int MaxBrightness { get; set; }
        public int MinBrightness { get; set; }
        public string CoverState { get; set; }
        public bool LightOn { get; set; }
        public string Action { get; set; }

        public static FlatDeviceStatusResponse FromFlatDeviceInfo(FlatDeviceInfo info, FlatDeviceAction action) {
            return new FlatDeviceStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Brightness = info.Brightness,
                MaxBrightness = info.MaxBrightness,
                MinBrightness = info.MinBrightness,
                CoverState = info.CoverState.ToString(),
                LightOn = info.LightOn,
                Action = action.ToString(),
            };
        }
    }

    public class WeatherStatusResponse {
        public string Type = "WeatherStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? DewPoint { get; set; }
        public double? WindSpeed { get; set; }
        public double? WindDirection { get; set; }
        public double? Pressure { get; set; }
        public double? SkyQuality { get; set; }
        public double? SkyBrightness { get; set; }
        public double? RainRate { get; set; }
        public double? CloudCover { get; set; }
        public double? StarFWHM { get; set; }
        public string Action { get; set; }

        public static WeatherStatusResponse FromWeatherInfo(WeatherDataInfo info, WeatherAction action) {
            return new WeatherStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                Temperature = Double.IsNaN(info.Temperature) ? null : info.Temperature,
                Humidity = Double.IsNaN(info.Humidity) ? null : info.Humidity,
                DewPoint = Double.IsNaN(info.DewPoint) ? null : info.DewPoint,
                WindSpeed = Double.IsNaN(info.WindSpeed) ? null : info.WindSpeed,
                WindDirection = Double.IsNaN(info.WindDirection) ? null : info.WindDirection,
                Pressure = Double.IsNaN(info.Pressure) ? null : info.Pressure,
                SkyQuality = Double.IsNaN(info.SkyQuality) ? null : info.SkyQuality,
                SkyBrightness = Double.IsNaN(info.SkyBrightness) ? null : info.SkyBrightness,
                RainRate = Double.IsNaN(info.RainRate) ? null : info.RainRate,
                CloudCover = Double.IsNaN(info.CloudCover) ? null : info.CloudCover,
                StarFWHM = Double.IsNaN(info.StarFWHM) ? null : info.StarFWHM,
                Action = action.ToString(),
            };
        }
    }

    public class SafetyMonitorStatusResponse {
        public string Type = "SafetyMonitorStatus";
        public string Name { get; set; }
        public string Description { get; set; }
        public string DeviceId { get; set; }
        public bool Connected { get; set; }
        public bool IsSafe { get; set; }
        public string Action { get; set; }

        public static SafetyMonitorStatusResponse FromSafetyMonitorInfo(SafetyMonitorInfo info, SafetyMonitorAction action) {
            return new SafetyMonitorStatusResponse {
                Name = info.Name,
                Description = info.Description,
                DeviceId = info.DeviceId,
                Connected = info.Connected,
                IsSafe = info.IsSafe,
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