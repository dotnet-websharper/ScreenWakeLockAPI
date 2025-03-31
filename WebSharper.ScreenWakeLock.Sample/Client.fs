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
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let statusMessage = Var.Create "Click the button to keep the screen awake."

    let requestWakeLock() = promise {
        try
            let! wakelock = JS.Window.Navigator.WakeLock.Request("screen")

            statusMessage := "Screen Wake Lock is active!"

            wakelock.AddEventListener("release", fun (evt: Dom.Event) -> statusMessage := "Wake Lock released!")
        with error ->
            statusMessage := $"Wake Lock Error: {error.Message}"
    }

    [<SPAEntryPoint>]
    let Main () =

        IndexTemplate.Main()
            .Status(statusMessage.V)
            .wakeButton(fun _ -> 
                async {
                    do! requestWakeLock().AsAsync()
                }
                |> Async.Start
            )
            .Doc()
        |> Doc.RunById "main"
