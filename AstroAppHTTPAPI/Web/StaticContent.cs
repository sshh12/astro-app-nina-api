using System.Text;

namespace Plugin.NINA.AstroAppHTTPAPI.Web {
    public static class StaticContent {
        public static string GetIndexHtml() {
            return @"<!DOCTYPE html>
<html>
<head>
    <title>NINA AstroApp HTTP API</title>
    <style>
        body {
            font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, ""Helvetica Neue"", Arial, sans-serif;
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
            background: #f5f5f5;
        }
        .container {
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        pre {
            background: #f8f9fa;
            padding: 15px;
            border-radius: 4px;
            overflow-x: auto;
        }
        .docs-frame {
            width: 100%;
            height: 500px;
            border: 1px solid #ddd;
            border-radius: 4px;
            margin: 20px 0;
        }
        h2 {
            margin-top: 30px;
            color: #333;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <h1>NINA AstroApp HTTP API</h1>
        
        <h2>Documentation</h2>
        <p>View the <a href=""https://github.com/sshh12/astro-app-nina-api/blob/main/API.md"" target=""_blank"">API documentation on GitHub</a></p>

        <h2>Example Code</h2>
        
        <h3>Python</h3>
        <pre>
import requests

status = requests.post(
    ""http://localhost:8181/api/v1/camera/capture"", 
    auth=(""user"", ""#password#""), 
    json={""ExposureTime"": 1.0}
).json()
print(status)

import websockets
import asyncio
import json

async def connect():
    async with websockets.connect(""ws://localhost:8181/events/v1"") as websocket:
        await asyncio.sleep(0.1)
        await websocket.send(json.dumps({""ApiKey"": ""#password#""}))
        while True:
            try:
                event = json.loads(await websocket.recv())
                print(event)
            except websockets.ConnectionClosed:
                break

asyncio.run(connect())</pre>

        <h3>JavaScript</h3>
        <pre>
fetch(""http://localhost:8181/api/v1/camera/capture"", {
    method: ""POST"",
    headers: {
        ""Authorization"": ""Basic "" + btoa(""user:#password#""),
        ""Content-Type"": ""application/json""
    },
    body: JSON.stringify({ExposureTime: 1.0})
}).then(res => res.json()).then(console.log)

const ws = new WebSocket(""ws://localhost:8181/events/v1"")

ws.onopen = () => ws.send(JSON.stringify({ApiKey: ""#password#""}))
ws.onmessage = event => console.log(JSON.parse(event.data))</pre>
    </div>
</body>
</html>";
        }
    }
} 