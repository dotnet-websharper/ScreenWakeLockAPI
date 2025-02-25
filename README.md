# WebSharper Screen Wake Lock API Binding

This repository provides an F# [WebSharper](https://websharper.com/) binding for the [Screen Wake Lock API](https://developer.mozilla.org/en-US/docs/Web/API/Screen_Wake_Lock_API), enabling WebSharper applications to prevent the screen from dimming or locking while in use.

## Repository Structure

The repository consists of two main projects:

1. **Binding Project**:

   - Contains the F# WebSharper binding for the Screen Wake Lock API.

2. **Sample Project**:
   - Demonstrates how to use the Screen Wake Lock API with WebSharper syntax.
   - Includes a GitHub Pages demo: [View Demo](https://dotnet-websharper.github.io/ScreenWakeLockAPI/).

## Installation

To use this package in your WebSharper project, add the NuGet package:

```bash
   dotnet add package WebSharper.ScreenWakeLock
```

## Building

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/dotnet-websharper/ScreenWakeLock.git
   cd ScreenWakeLock
   ```

2. Build the Binding Project:

   ```bash
   dotnet build WebSharper.ScreenWakeLock/WebSharper.ScreenWakeLock.fsproj
   ```

3. Build and Run the Sample Project:

   ```bash
   cd WebSharper.ScreenWakeLock.Sample
   dotnet build
   dotnet run
   ```

4. Open the hosted demo to see the Sample project in action:
   [https://dotnet-websharper.github.io/ScreenWakeLockAPI/](https://dotnet-websharper.github.io/ScreenWakeLockAPI/)

## Example Usage

Below is an example of how to use the Screen Wake Lock API in a WebSharper project:

```fsharp
namespace WebSharper.ScreenWakeLock.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Notation
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.ScreenWakeLock

[<JavaScript>]
module Client =
    // The templates are loaded from the DOM, so you can edit index.html
    // and refresh your browser without recompiling unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    // Variable to store the wake lock status message
    let statusMessage = Var.Create "Click the button to keep the screen awake."

    let requestWakeLock() = promise {
        try
            // Request a wake lock to prevent the screen from dimming or locking
            let! wakelock = As<Navigator>(JS.Window.Navigator).WakeLock.Request("screen")

            // Update status message when wake lock is active
            statusMessage := "Screen Wake Lock is active!"

            // Listen for the release event and update status
            wakelock.AddEventListener("release", fun (evt: Dom.Event) ->
                statusMessage := "Wake Lock released!"
            )
        with error ->
            // Handle errors and display the failure message
            statusMessage := $"Wake Lock Error: {error.Message}"
    }

    [<SPAEntryPoint>]
    let Main () =

        IndexTemplate.Main()
            // Bind the status message to the UI
            .Status(statusMessage.V)
            // Attach event handler to the wake lock button
            .wakeButton(fun _ ->
                async {
                    do! requestWakeLock().AsAsync()
                }
                |> Async.Start
            )
            .Doc()
        |> Doc.RunById "main"
```

This example demonstrates how to request and handle the Screen Wake Lock API using WebSharper.

## Important Considerations

- **User Interaction Required**: Most browsers require user interaction before enabling wake locks.
- **Limited Browser Support**: Some browsers do not fully support the Screen Wake Lock API; check [MDN Screen Wake Lock API](https://developer.mozilla.org/en-US/docs/Web/API/Screen_Wake_Lock_API) for the latest compatibility information.
- **Battery Consumption**: Using wake locks can impact battery life, so they should be released when no longer needed.
