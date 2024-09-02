namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class CameraBinningRequest {
        public short X { get; set; }
        public short Y { get; set; }
    }

    public class DomeSlewRequest {
        public double Azimuth { get; set; }
    }

    public class DomeFollowingRequest {
        public bool Enabled { get; set; }
    }

    public class FilterWheelFilterRequest {
        public short Position { get; set; }
    }

    public class FocuserPositionRequest {
        public int Position { get; set; }
    }

    public class WebSocketAuthRequest {
        public string ApiKey { get; set; }
    }

}
