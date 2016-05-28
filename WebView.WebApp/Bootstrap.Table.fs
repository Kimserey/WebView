namespace WebView.WebApp.Bootstrap

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.Resources

[<JavaScript; AutoOpen>]
module Table =
    
    type Table = {
        Headers: string list option
        Body: TableBody
        Style: TableStyle list
    } with
        static member Empty = 
            { Headers = None
              Body = TableBody.Empty
              Style = [] }

        static member AddHeaders headers x =
            { x with Headers = Some headers }

        static member OnAfterRenderBody action (x: Table) =
            { x with Body = { x.Body with OnAfterRenderAction = Some action } }

        static member AddRow row (x: Table) =
            { x with Body = x.Body |> TableBody.AddRow row }

        static member AddRows rows (x: Table) =
            (x, rows) ||> List.fold (fun table row -> table |> Table.AddRow row)

        static member SetStyle style (x:Table) =
            { x with Style = style }

        static member Render (x: Table) =
            divAttr 
                [ attr.``class`` "table-responsive" ]
                [ tableAttr [ attr.``class`` ([ "table" ] @ (List.map TableStyle.ToCssClass x.Style) |> String.concat " ") ]
                            [ yield! match x.Headers |> Option.map (fun hs -> [ thead  [ tr (hs |> List.map (fun h -> th [ text h ] :> Doc)) ] :> Doc ]) with Some x -> x | None -> []
                              yield x.Body |> TableBody.Render :> Doc ] :> Doc ]
    and TableBody = 
        { Rows: TableRow list
          OnAfterRenderAction: Option<Dom.Element -> unit> }
          with
            static member Empty =
                { Rows = []
                  OnAfterRenderAction = None }

            static member AddRow row (x: TableBody) =
                { x with Rows = x.Rows @ [ row ] }

            static member Render (x: TableBody) =
                tbodyAttr (match x.OnAfterRenderAction |> Option.map (fun action -> [ on.afterRender action ])  with Some x -> x | None -> [])
                          (x.Rows |> List.map TableRow.Render |> Seq.cast)
    and TableRow = {
        Status: TableRowStatus
        Data: Doc list
        OnClickAction: (unit -> unit) option
    } with
        static member Create data =
            { Status = TableRowStatus.Normal
              Data = data
              OnClickAction = None }
        
        static member OnClick action (x: TableRow) =
            { x with OnClickAction = Some action } 

        static member SetStatus status (x: TableRow) =
            { x with Status = status }

        static member Render (x: TableRow) =
            trAttr [ yield attr.``class`` (TableRowStatus.ToCssClass x.Status) 
                     yield! match x.OnClickAction with Some action -> [ on.click(fun _ _ -> action()); attr.style "cursor:pointer;" ] | None -> [] ]
                   (x.Data |> List.map (fun d -> td [ d ] :> Doc))

    and TableStyle = Bordered | Striped | Hover
        with static member ToCssClass = function Bordered -> "table-bordered" | Striped  -> "table-striped" | Hover    -> "table-hover"

    and TableRowStatus = Normal | Active | Success | Warning | Danger | Info
        with static member ToCssClass = function  Normal -> "" | Active -> "active" | Success -> "success" | Warning -> "warning" | Danger -> "danger" | Info -> "info" 
