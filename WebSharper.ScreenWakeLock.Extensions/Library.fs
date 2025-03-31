namespace WebSharper.ScreenWakeLock

open WebSharper
open WebSharper.JavaScript

[<JavaScript; AutoOpen>]
module Extensions =

    type Navigator with
        [<Inline "$this.wakeLock">]
        member this.WakeLock with get(): WakeLock = X<WakeLock>
