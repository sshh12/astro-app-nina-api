## REST API Documentation

#### Camera Endpoints

<details>
 <summary><code>GET</code> <code><b>/api/v1/camera/</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Battery": "...", "Connected": "...", "Offset": "...", "Gain": "...", "DefaultGain": "...", "Temperature": "...", "TemperatureSetPoint": "...", "AtTargetTemp": "...", "TargetTemp": "...", "CoolerOn": "...", "CoolerPower": "...", "HasDewHeater": "...", "BinX": "...", "BinY": "...", "USBLimit": "...", "XSize": "...", "YSize": "...", "PixelSize": "...", "SensorType": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>PATCH</code> <code><b>/api/v1/camera/binning</b></code></summary>

##### Parameters

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | X | required | short | N/A  |
> | Y | required | short | N/A  |

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Battery": "...", "Connected": "...", "Offset": "...", "Gain": "...", "DefaultGain": "...", "Temperature": "...", "TemperatureSetPoint": "...", "AtTargetTemp": "...", "TargetTemp": "...", "CoolerOn": "...", "CoolerPower": "...", "HasDewHeater": "...", "BinX": "...", "BinY": "...", "USBLimit": "...", "XSize": "...", "YSize": "...", "PixelSize": "...", "SensorType": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/camera/capture</b></code></summary>

##### Parameters

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | ExposureTime | required | double | N/A  |

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Battery": "...", "Connected": "...", "Offset": "...", "Gain": "...", "DefaultGain": "...", "Temperature": "...", "TemperatureSetPoint": "...", "AtTargetTemp": "...", "TargetTemp": "...", "CoolerOn": "...", "CoolerPower": "...", "HasDewHeater": "...", "BinX": "...", "BinY": "...", "USBLimit": "...", "XSize": "...", "YSize": "...", "PixelSize": "...", "SensorType": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/camera/connect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Battery": "...", "Connected": "...", "Offset": "...", "Gain": "...", "DefaultGain": "...", "Temperature": "...", "TemperatureSetPoint": "...", "AtTargetTemp": "...", "TargetTemp": "...", "CoolerOn": "...", "CoolerPower": "...", "HasDewHeater": "...", "BinX": "...", "BinY": "...", "USBLimit": "...", "XSize": "...", "YSize": "...", "PixelSize": "...", "SensorType": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/camera/disconnect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Battery": "...", "Connected": "...", "Offset": "...", "Gain": "...", "DefaultGain": "...", "Temperature": "...", "TemperatureSetPoint": "...", "AtTargetTemp": "...", "TargetTemp": "...", "CoolerOn": "...", "CoolerPower": "...", "HasDewHeater": "...", "BinX": "...", "BinY": "...", "USBLimit": "...", "XSize": "...", "YSize": "...", "PixelSize": "...", "SensorType": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

#### Dome Endpoints

<details>
 <summary><code>GET</code> <code><b>/api/v1/dome/</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "AtHome": "...", "AtPark": "...", "DriverFollowing": "...", "ShutterStatus": "...", "Azimuth": "...", "Slewing": "...", "IsFollowingScope": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/dome/close</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "AtHome": "...", "AtPark": "...", "DriverFollowing": "...", "ShutterStatus": "...", "Azimuth": "...", "Slewing": "...", "IsFollowingScope": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/dome/connect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "AtHome": "...", "AtPark": "...", "DriverFollowing": "...", "ShutterStatus": "...", "Azimuth": "...", "Slewing": "...", "IsFollowingScope": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/dome/disconnect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "AtHome": "...", "AtPark": "...", "DriverFollowing": "...", "ShutterStatus": "...", "Azimuth": "...", "Slewing": "...", "IsFollowingScope": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>PATCH</code> <code><b>/api/v1/dome/following</b></code></summary>

##### Parameters

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Enabled | required | bool | N/A  |

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "AtHome": "...", "AtPark": "...", "DriverFollowing": "...", "ShutterStatus": "...", "Azimuth": "...", "Slewing": "...", "IsFollowingScope": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/dome/home</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "AtHome": "...", "AtPark": "...", "DriverFollowing": "...", "ShutterStatus": "...", "Azimuth": "...", "Slewing": "...", "IsFollowingScope": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/dome/open</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "AtHome": "...", "AtPark": "...", "DriverFollowing": "...", "ShutterStatus": "...", "Azimuth": "...", "Slewing": "...", "IsFollowingScope": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/dome/park</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "AtHome": "...", "AtPark": "...", "DriverFollowing": "...", "ShutterStatus": "...", "Azimuth": "...", "Slewing": "...", "IsFollowingScope": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/dome/rotate</b></code></summary>

##### Parameters

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Azimuth | required | double | N/A  |

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "AtHome": "...", "AtPark": "...", "DriverFollowing": "...", "ShutterStatus": "...", "Azimuth": "...", "Slewing": "...", "IsFollowingScope": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/dome/sync</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "AtHome": "...", "AtPark": "...", "DriverFollowing": "...", "ShutterStatus": "...", "Azimuth": "...", "Slewing": "...", "IsFollowingScope": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

#### Mount Endpoints

<details>
 <summary><code>GET</code> <code><b>/api/v1/mount/</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "UTCDate": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Slewing": "...", "TrackingEnabled": "...", "TrackingMode": "...", "AtHome": "...", "AtPark": "...", "RightAscension": "...", "Declination": "...", "Azimuth": "...", "Altitude": "...", "SideOfPier": "...", "SiteLatitude": "...", "SiteLongitude": "...", "SiteElevation": "...", "AlignmentMode": "...", "IsPulseGuiding": "...", "SiderealTime": "...", "GuideRateDeclination": "...", "GuideRateRightAscension": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/mount/connect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "UTCDate": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Slewing": "...", "TrackingEnabled": "...", "TrackingMode": "...", "AtHome": "...", "AtPark": "...", "RightAscension": "...", "Declination": "...", "Azimuth": "...", "Altitude": "...", "SideOfPier": "...", "SiteLatitude": "...", "SiteLongitude": "...", "SiteElevation": "...", "AlignmentMode": "...", "IsPulseGuiding": "...", "SiderealTime": "...", "GuideRateDeclination": "...", "GuideRateRightAscension": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/mount/disconnect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "UTCDate": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Slewing": "...", "TrackingEnabled": "...", "TrackingMode": "...", "AtHome": "...", "AtPark": "...", "RightAscension": "...", "Declination": "...", "Azimuth": "...", "Altitude": "...", "SideOfPier": "...", "SiteLatitude": "...", "SiteLongitude": "...", "SiteElevation": "...", "AlignmentMode": "...", "IsPulseGuiding": "...", "SiderealTime": "...", "GuideRateDeclination": "...", "GuideRateRightAscension": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/mount/park</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "UTCDate": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Slewing": "...", "TrackingEnabled": "...", "TrackingMode": "...", "AtHome": "...", "AtPark": "...", "RightAscension": "...", "Declination": "...", "Azimuth": "...", "Altitude": "...", "SideOfPier": "...", "SiteLatitude": "...", "SiteLongitude": "...", "SiteElevation": "...", "AlignmentMode": "...", "IsPulseGuiding": "...", "SiderealTime": "...", "GuideRateDeclination": "...", "GuideRateRightAscension": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/mount/slew</b></code></summary>

##### Parameters

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | RightAscension | required | double | N/A  |
> | Declination | required | double | N/A  |

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "UTCDate": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Slewing": "...", "TrackingEnabled": "...", "TrackingMode": "...", "AtHome": "...", "AtPark": "...", "RightAscension": "...", "Declination": "...", "Azimuth": "...", "Altitude": "...", "SideOfPier": "...", "SiteLatitude": "...", "SiteLongitude": "...", "SiteElevation": "...", "AlignmentMode": "...", "IsPulseGuiding": "...", "SiderealTime": "...", "GuideRateDeclination": "...", "GuideRateRightAscension": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>PATCH</code> <code><b>/api/v1/mount/trackingmode</b></code></summary>

##### Parameters

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | TrackingMode | required | string | N/A  |

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "UTCDate": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Slewing": "...", "TrackingEnabled": "...", "TrackingMode": "...", "AtHome": "...", "AtPark": "...", "RightAscension": "...", "Declination": "...", "Azimuth": "...", "Altitude": "...", "SideOfPier": "...", "SiteLatitude": "...", "SiteLongitude": "...", "SiteElevation": "...", "AlignmentMode": "...", "IsPulseGuiding": "...", "SiderealTime": "...", "GuideRateDeclination": "...", "GuideRateRightAscension": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/mount/unpark</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "UTCDate": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Slewing": "...", "TrackingEnabled": "...", "TrackingMode": "...", "AtHome": "...", "AtPark": "...", "RightAscension": "...", "Declination": "...", "Azimuth": "...", "Altitude": "...", "SideOfPier": "...", "SiteLatitude": "...", "SiteLongitude": "...", "SiteElevation": "...", "AlignmentMode": "...", "IsPulseGuiding": "...", "SiderealTime": "...", "GuideRateDeclination": "...", "GuideRateRightAscension": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

#### Rotator Endpoints

<details>
 <summary><code>GET</code> <code><b>/api/v1/rotator/</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Position": "...", "IsMoving": "...", "StepSize": "...", "MechanicalPosition": "...", "Reverse": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/rotator/connect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Position": "...", "IsMoving": "...", "StepSize": "...", "MechanicalPosition": "...", "Reverse": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/rotator/disconnect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Position": "...", "IsMoving": "...", "StepSize": "...", "MechanicalPosition": "...", "Reverse": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

#### Switch Endpoints

<details>
 <summary><code>GET</code> <code><b>/api/v1/switch/</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/switch/connect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/switch/disconnect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

#### Focuser Endpoints

<details>
 <summary><code>GET</code> <code><b>/api/v1/focuser/</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Position": "...", "TempComp": "...", "TempCompAvailable": "...", "Temperature": "...", "StepSize": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/focuser/connect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Position": "...", "TempComp": "...", "TempCompAvailable": "...", "Temperature": "...", "StepSize": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/focuser/disconnect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Position": "...", "TempComp": "...", "TempCompAvailable": "...", "Temperature": "...", "StepSize": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>PATCH</code> <code><b>/api/v1/focuser/position</b></code></summary>

##### Parameters

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Position | required | int | N/A  |

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Position": "...", "TempComp": "...", "TempCompAvailable": "...", "Temperature": "...", "StepSize": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

#### Filter Wheel Endpoints

<details>
 <summary><code>GET</code> <code><b>/api/v1/filterwheel/</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "SelectedFilter": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/filterwheel/connect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "SelectedFilter": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/filterwheel/disconnect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "SelectedFilter": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>PATCH</code> <code><b>/api/v1/filterwheel/filter</b></code></summary>

##### Parameters

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Position | required | short | N/A  |

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "SelectedFilter": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

#### Flat Device Endpoints

<details>
 <summary><code>GET</code> <code><b>/api/v1/flatdevice/</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Brightness": "...", "MaxBrightness": "...", "MinBrightness": "...", "CoverState": "...", "LightOn": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/flatdevice/connect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Brightness": "...", "MaxBrightness": "...", "MinBrightness": "...", "CoverState": "...", "LightOn": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/flatdevice/disconnect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Brightness": "...", "MaxBrightness": "...", "MinBrightness": "...", "CoverState": "...", "LightOn": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

#### Safety Monitor Controller Endpoints

<details>
 <summary><code>GET</code> <code><b>/api/v1/safetymonitor/</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "IsSafe": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/safetymonitor/connect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "IsSafe": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/safetymonitor/disconnect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "IsSafe": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

#### Weather Endpoints

<details>
 <summary><code>GET</code> <code><b>/api/v1/weather/</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Temperature": "...", "Humidity": "...", "DewPoint": "...", "WindSpeed": "...", "WindDirection": "...", "Pressure": "...", "SkyQuality": "...", "SkyBrightness": "...", "RainRate": "...", "CloudCover": "...", "StarFWHM": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/weather/connect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Temperature": "...", "Humidity": "...", "DewPoint": "...", "WindSpeed": "...", "WindDirection": "...", "Pressure": "...", "SkyQuality": "...", "SkyBrightness": "...", "RainRate": "...", "CloudCover": "...", "StarFWHM": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

<details>
 <summary><code>POST</code> <code><b>/api/v1/weather/disconnect</b></code></summary>

##### Parameters

> None

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                | `{"Name": "...", "Description": "...", "DeviceId": "...", "Connected": "...", "Temperature": "...", "Humidity": "...", "DewPoint": "...", "WindSpeed": "...", "WindDirection": "...", "Pressure": "...", "SkyQuality": "...", "SkyBrightness": "...", "RainRate": "...", "CloudCover": "...", "StarFWHM": "...", "Action": "..."}`                                             |
</details>

------------------------------------------------------------------------------------------

## Websocket Events Documentation

#### Camera Events

<details>
 <summary><code>Event</code> <code><b>CameraStatusResponse</b></code></summary>

##### Data

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Name | required | string | N/A  |
> | Description | required | string | N/A  |
> | DeviceId | required | string | N/A  |
> | Battery | required | int? | N/A  |
> | Connected | required | bool | N/A  |
> | Offset | required | int | N/A  |
> | Gain | required | int? | N/A  |
> | DefaultGain | required | int? | N/A  |
> | Temperature | required | double? | N/A  |
> | TemperatureSetPoint | required | double? | N/A  |
> | AtTargetTemp | required | bool | N/A  |
> | TargetTemp | required | double? | N/A  |
> | CoolerOn | required | bool | N/A  |
> | CoolerPower | required | double? | N/A  |
> | HasDewHeater | required | bool | N/A  |
> | BinX | required | short | N/A  |
> | BinY | required | short | N/A  |
> | USBLimit | required | int | N/A  |
> | XSize | required | int | N/A  |
> | YSize | required | int | N/A  |
> | PixelSize | required | double? | N/A  |
> | SensorType | required | string | N/A  |
> | Action | required | string | N/A  |
</details>

------------------------------------------------------------------------------------------

#### Dome Events

<details>
 <summary><code>Event</code> <code><b>DomeStatusResponse</b></code></summary>

##### Data

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Name | required | string | N/A  |
> | Description | required | string | N/A  |
> | DeviceId | required | string | N/A  |
> | Connected | required | bool | N/A  |
> | AtHome | required | bool | N/A  |
> | AtPark | required | bool | N/A  |
> | DriverFollowing | required | bool | N/A  |
> | ShutterStatus | required | string | N/A  |
> | Azimuth | required | double | N/A  |
> | Slewing | required | bool | N/A  |
> | IsFollowingScope | required | bool | N/A  |
> | Action | required | string | N/A  |
</details>

------------------------------------------------------------------------------------------

#### Mount Events

<details>
 <summary><code>Event</code> <code><b>MountStatusResponse</b></code></summary>

##### Data

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Name | required | string | N/A  |
> | UTCDate | required | string | N/A  |
> | Description | required | string | N/A  |
> | DeviceId | required | string | N/A  |
> | Connected | required | bool | N/A  |
> | Slewing | required | bool | N/A  |
> | TrackingEnabled | required | bool | N/A  |
> | TrackingMode | required | string | N/A  |
> | AtHome | required | bool | N/A  |
> | AtPark | required | bool | N/A  |
> | RightAscension | required | double | N/A  |
> | Declination | required | double | N/A  |
> | Azimuth | required | double | N/A  |
> | Altitude | required | double | N/A  |
> | SideOfPier | required | string | N/A  |
> | SiteLatitude | required | double | N/A  |
> | SiteLongitude | required | double | N/A  |
> | SiteElevation | required | double | N/A  |
> | AlignmentMode | required | string | N/A  |
> | IsPulseGuiding | required | bool | N/A  |
> | SiderealTime | required | string | N/A  |
> | GuideRateDeclination | required | double? | N/A  |
> | GuideRateRightAscension | required | double? | N/A  |
> | Action | required | string | N/A  |
</details>

------------------------------------------------------------------------------------------

#### Rotator Events

<details>
 <summary><code>Event</code> <code><b>RotatorStatusResponse</b></code></summary>

##### Data

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Name | required | string | N/A  |
> | Description | required | string | N/A  |
> | DeviceId | required | string | N/A  |
> | Connected | required | bool | N/A  |
> | Position | required | double? | N/A  |
> | IsMoving | required | bool | N/A  |
> | StepSize | required | double? | N/A  |
> | MechanicalPosition | required | float? | N/A  |
> | Reverse | required | bool | N/A  |
> | Action | required | string | N/A  |
</details>

------------------------------------------------------------------------------------------

#### Switch Events

<details>
 <summary><code>Event</code> <code><b>SwitchStatusResponse</b></code></summary>

##### Data

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Name | required | string | N/A  |
> | Description | required | string | N/A  |
> | DeviceId | required | string | N/A  |
> | Connected | required | bool | N/A  |
> | Action | required | string | N/A  |
</details>

------------------------------------------------------------------------------------------

#### Focuser Events

<details>
 <summary><code>Event</code> <code><b>FocuserStatusResponse</b></code></summary>

##### Data

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Name | required | string | N/A  |
> | Description | required | string | N/A  |
> | DeviceId | required | string | N/A  |
> | Connected | required | bool | N/A  |
> | Position | required | int | N/A  |
> | TempComp | required | bool | N/A  |
> | TempCompAvailable | required | bool | N/A  |
> | Temperature | required | double? | N/A  |
> | StepSize | required | double? | N/A  |
> | Action | required | string | N/A  |
</details>

------------------------------------------------------------------------------------------

#### Filter Wheel Events

<details>
 <summary><code>Event</code> <code><b>FilterWheelStatusResponse</b></code></summary>

##### Data

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Name | required | string | N/A  |
> | Description | required | string | N/A  |
> | DeviceId | required | string | N/A  |
> | Connected | required | bool | N/A  |
> | SelectedFilter | required | Filter | N/A  |
> | Action | required | string | N/A  |
</details>

------------------------------------------------------------------------------------------

#### Flat Device Events

<details>
 <summary><code>Event</code> <code><b>FlatDeviceStatusResponse</b></code></summary>

##### Data

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Name | required | string | N/A  |
> | Description | required | string | N/A  |
> | DeviceId | required | string | N/A  |
> | Connected | required | bool | N/A  |
> | Brightness | required | int | N/A  |
> | MaxBrightness | required | int | N/A  |
> | MinBrightness | required | int | N/A  |
> | CoverState | required | string | N/A  |
> | LightOn | required | bool | N/A  |
> | Action | required | string | N/A  |
</details>

------------------------------------------------------------------------------------------

#### Safety Monitor Controller Events

<details>
 <summary><code>Event</code> <code><b>SafetyMonitorStatusResponse</b></code></summary>

##### Data

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Name | required | string | N/A  |
> | Description | required | string | N/A  |
> | DeviceId | required | string | N/A  |
> | Connected | required | bool | N/A  |
> | IsSafe | required | bool | N/A  |
> | Action | required | string | N/A  |
</details>

------------------------------------------------------------------------------------------

#### Weather Events

<details>
 <summary><code>Event</code> <code><b>WeatherStatusResponse</b></code></summary>

##### Data

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | Name | required | string | N/A  |
> | Description | required | string | N/A  |
> | DeviceId | required | string | N/A  |
> | Connected | required | bool | N/A  |
> | Temperature | required | double? | N/A  |
> | Humidity | required | double? | N/A  |
> | DewPoint | required | double? | N/A  |
> | WindSpeed | required | double? | N/A  |
> | WindDirection | required | double? | N/A  |
> | Pressure | required | double? | N/A  |
> | SkyQuality | required | double? | N/A  |
> | SkyBrightness | required | double? | N/A  |
> | RainRate | required | double? | N/A  |
> | CloudCover | required | double? | N/A  |
> | StarFWHM | required | double? | N/A  |
> | Action | required | string | N/A  |
</details>

------------------------------------------------------------------------------------------

