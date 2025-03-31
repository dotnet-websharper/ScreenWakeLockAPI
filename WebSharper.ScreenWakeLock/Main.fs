namespace WebSharper.ScreenWakeLock

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let WakeLockSentinel =
        Class "WakeLockSentinel"
        |=> Inherits T<Dom.EventTarget>
        |+> Instance [
            "released" =? T<bool>
            "type" =? T<string>

            "release" => T<unit> ^-> T<Promise<unit>>

            "onrelease" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnRelease instead"
            "onrelease" =@ T<Dom.Event> ^-> T<unit>
            |> WithSourceName "OnRelease"
        ]

    let WakeLock =
        Class "WakeLock"
        |+> Instance [
            "request" => T<string>?``type`` ^-> T<Promise<_>>[WakeLockSentinel]
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.ScreenWakeLock" [
                WakeLockSentinel
                WakeLock
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
