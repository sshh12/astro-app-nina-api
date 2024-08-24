namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public class CameraBinningRequest {
        public short X { get; set; }
        public short Y { get; set; }
    }

    public class WebSocketAuthRequest {
        public string ApiKey { get; set; }
    }
}
