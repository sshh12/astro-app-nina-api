using System.Reflection;
using System.Runtime.InteropServices;

// [MANDATORY] The following GUID is used as a unique identifier of the plugin. Generate a fresh one for your plugin!
[assembly: Guid("e733ee91-25e0-443d-821a-e16aec9f5757")]

// [MANDATORY] The assembly versioning
//Should be incremented for each new release build of a plugin
[assembly: AssemblyVersion("1.0.0.10")]
[assembly: AssemblyFileVersion("1.0.0.10")]

// [MANDATORY] The name of your plugin
[assembly: AssemblyTitle("Astro HTTP API")]
// [MANDATORY] A short description of your plugin
[assembly: AssemblyDescription("A general purpose HTTP API for NINA designed for use with astroapp.io")]

// The following attributes are not required for the plugin per se, but are required by the official manifest meta data

// Your name
[assembly: AssemblyCompany("Shrivu Shankar (sshh12)")]
// The product name that this plugin is part of
[assembly: AssemblyProduct("Astro HTTP API")]
[assembly: AssemblyCopyright("Copyright © 2024 Shrivu Shankar")]

// The minimum Version of N.I.N.A. that this plugin is compatible with
[assembly: AssemblyMetadata("MinimumApplicationVersion", "3.0.0.2017")]

// The license your plugin code is using
[assembly: AssemblyMetadata("License", "MPL-2.0")]
// The url to the license
[assembly: AssemblyMetadata("LicenseURL", "https://www.mozilla.org/en-US/MPL/2.0/")]
// The repository where your pluggin is hosted
[assembly: AssemblyMetadata("Repository", "https://github.com/sshh12/astro-app-nina-api")]

// The following attributes are optional for the official manifest meta data

//[Optional] Your plugin homepage URL - omit if not applicaple
[assembly: AssemblyMetadata("Homepage", "https://github.com/sshh12/astro-app-nina-api")]

//[Optional] Common tags that quickly describe your plugin
[assembly: AssemblyMetadata("Tags", "HTTP,REST,API,Websockets")]

//[Optional] A link that will show a log of all changes in between your plugin's versions
[assembly: AssemblyMetadata("ChangelogURL", "https://github.com/sshh12/astro-app-nina-api/commits/main/")]

//[Optional] The url to a featured logo that will be displayed in the plugin list next to the name
[assembly: AssemblyMetadata("FeaturedImageURL", "https://raw.githubusercontent.com/sshh12/astro-app-nina-api/main/AstroAppHTTPAPI/logo512.png")]
//[Optional] A url to an example screenshot of your plugin in action
[assembly: AssemblyMetadata("ScreenshotURL", "")]
//[Optional] An additional url to an example example screenshot of your plugin in action
[assembly: AssemblyMetadata("AltScreenshotURL", "")]
//[Optional] An in-depth description of your plugin
[assembly: AssemblyMetadata("LongDescription", @"This plugin provides a general purpose HTTP (and websocket) server you can use to control N.I.N.A. remotely.

* NOTE: This plugin is experimental and may not yet support all N.I.N.A features, in some cases it may unexpectedly crash N.I.N.A. Use at your own risk.

# Features #

* Control all standard device types (mount, focuser, camera, filter wheel, rotator, dome)
* Subscribe to live events from N.I.N.A. via websockets
* Use over the internet with tools like ngrok or cloudflare tunnels
* Native support for astroapp.io

# Quick Start #

For using an existing UI, visit [astroapp.io](https://astroapp.io/image/nina).

For using the API directly, see the [quick start guide](https://github.com/sshh12/astro-app-nina-api).

See API docs here: [API Docs](https://github.com/sshh12/astro-app-nina-api/blob/main/API.md).

# Getting Help #

If you have questions, submit an issue on [GitHub](https://github.com/sshh12/astro-app-nina-api/issues) or this [form](https://forms.gle/hiaKh3HRfVBYZxiu6).
* Astro HTTP API is provided 'as is' under the terms of the [Mozilla Public License 2.0](https://github.com/sshh12/astro-app-nina-api/blob/main/LICENSE)
* Source code for this plugin is available at this plugin's [repository](https://github.com/sshh12/astro-app-nina-api)
")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
// [Unused]
[assembly: AssemblyConfiguration("")]
// [Unused]
[assembly: AssemblyTrademark("")]
// [Unused]
[assembly: AssemblyCulture("")]