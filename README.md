# Astro HTTP API

> Astro HTTP API is a general purpose HTTP API for NINA designed for use with astroapp.io

## Local Network Setup

1. Install this plugin either directly from N.I.N.A. or by downloading the latest release from the [releases page](https://github.com/sshh12/astro-app-nina-api/releases) and extracting it into `%localappdata%\NINA\Plugins`
2. On the plugin page, set `Web Server Enabled`
3. Find your local IP address by running `ipconfig` in Command Prompt (typically `192.168.x.x` or `10.x.x.x`)
4. Open your browser and navigate to `http://<local-ip>:<port>` where `<local-ip>` is your local IP address and `<port>` is the port you set in the plugin settings (default is `8181`). It should prompt for a login and you'll see the user/password in the plugin settings.

By default, you'll only be able to access the API from your local network (e.g. same WiFi network).

## Remote Network Setup

There are a ton of options for exposing this server to the internet and allowing N.I.N.A. to be controlled from anywhere. See [awesome-tunneling](https://github.com/anderspitman/awesome-tunneling) for list of options.

The typical best option is cloudflare tunnels. It's free and easy to setup. You can find a guide [here](https://developers.cloudflare.com/cloudflare-one/connections/connect-networks/get-started/create-remote-tunnel/).

Keep in mind that exposing your N.I.N.A. server to the internet can be a security risk. It's recommended to not run this server for long periods of time (24/7) if you have sensitive data on the same system that N.I.N.A. is running on.

## Example Usage

> For full endpoint documentation see the [API Documentation](https://github.com/sshh12/astro-app-nina-api/blob/main/API.md).

### Python

```python
import requests

status = requests.post(
    "http://localhost:8181/api/v1/camera/capture", 
    auth=("user", "#password#"), 
    json={"ExposureTime": 1.0}
).json()
print(status)
```

```python
import websockets
import asyncio
import json

async def connect():
    async with websockets.connect("ws://localhost:8181/events/v1") as websocket:
        await asyncio.sleep(0.1)
        await websocket.send(json.dumps({'ApiKey': '#password#'}))
        while True:
            try:
                event = json.loads(await websocket.recv())
                print(event)
            except websockets.ConnectionClosed:
                break

asyncio.run(connect())
```

### JavaScript

```javascript
fetch("http://localhost:8181/api/v1/camera/capture", {
    method: "POST",
    headers: {
        "Authorization": "Basic " + btoa("user:#password#"),
        "Content-Type": "application/json"
    },
    body: JSON.stringify({ExposureTime: 1.0})
}).then(res => res.json()).then(console.log)
```

```javascript
const ws = new WebSocket("ws://localhost:8181/events/v1")

ws.onopen = () => ws.send(JSON.stringify({ApiKey: "#password#"}))
ws.onmessage = event => console.log(JSON.parse(event.data))
```

## Help & Feature Requests

There are plenty of N.I.N.A. features that are not yet implemented in this API just because I wasn't using them. If you need a feature that is not yet implemented, please let me know and I can add it.

Submit an issue on [GitHub](https://github.com/sshh12/astro-app-nina-api/issues) or this [form](https://forms.gle/hiaKh3HRfVBYZxiu6).

## Acknowledgements

Shout out to https://github.com/christian-photo/ninaAPI which was used as a reference for this project.