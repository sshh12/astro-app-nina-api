# Astro HTTP API

> Astro HTTP API is a general purpose HTTP API for [N.I.N.A](https://nighttime-imaging.eu/) designed for use with [AstroApp.io](https://astroapp.io/)

## Local Network Setup

1. Install this plugin either directly from N.I.N.A. or by downloading the latest release from the [releases page](https://github.com/sshh12/astro-app-nina-api/releases) and extracting it into `%localappdata%\NINA\Plugins`
2. On the plugin page, set `Web Server Enabled`
3. Find your local IP address by running `ipconfig` in Command Prompt (typically `192.168.x.x` or `10.x.x.x`)
4. Open your browser and navigate to `http://<local-ip>:<port>` where `<local-ip>` is your local IP address and `<port>` is the port you set in the plugin settings (default is `8181`). It should prompt for a login and you'll see the user/password in the plugin settings.

By default, you'll only be able to access the API from your local network (e.g. same WiFi network) and will need to setup SSL if you want to use it with astroapp.io.

<details>
<summary>SSL Setup Instructions (Windows)</summary>

To generate a self-signed SSL certificate using PowerShell:

1. Open PowerShell as Administrator and run:
```powershell
$cert = New-SelfSignedCertificate -DnsName "localhost" -CertStoreLocation "cert:\LocalMachine\My" -NotAfter (Get-Date).AddYears(10) -KeySpec KeyExchange

$pwd = ConvertTo-SecureString -String "YourPasswordHere" -Force -AsPlainText # Change this to your desired password

$path = "C:\Users\Shriv\Desktop\certificate.pfx" # Change this to the path you want to save the certificate to

Export-PfxCertificate -Cert $cert -FilePath $path -Password $pwd
```

2. Update the plugin settings with:
   - SSL Certificate Path: Path to the generated .pfx file (e.g., `C:\Users\Shriv\Desktop\certificate.pfx`)
   - SSL Certificate Password: The password you used in the PowerShell command (e.g., `YourPasswordHere`)

3. If you want to use this with astroapp.io, in Chrome, enable `chrome://flags/#allow-insecure-localhost` and restart.

Note: Self-signed certificates will show security warnings in browsers.

</details>

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
