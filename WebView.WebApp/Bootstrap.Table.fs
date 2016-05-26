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

        static member AddRowDoc rowData (x: Table) =
            { x with Body = x.Body |> TableBody.AddRowDoc rowData }

        static member AddRowText rowData (x: Table) =
            { x with Body = x.Body |> TableBody.AddRowText rowData }

        static member AddRowTextWithStatus rowData status (x: Table) =
            { x with Body = x.Body |> TableBody.AddRowTextWithStatus rowData status }
        
        static member AddStyle style (x:Table) =
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

            static member AddRowDoc rowData (x: TableBody) =
                { x with Rows = x.Rows @ [ { Status = Normal; Data = rowData } ] }
            
            static member AddRowText rowData (x: TableBody) =
                x |> TableBody.AddRowDoc (rowData |> List.map (fun i -> text i))

            static member AddRowTextWithStatus rowData status x =
                { x with Rows = x.Rows @ [ { Status = status;  Data = rowData |> List.map (fun i -> text i) } ] }

            static member Render (x: TableBody) =
                tbodyAttr (match x.OnAfterRenderAction |> Option.map (fun action -> [ on.afterRender action ])  with Some x -> x | None -> [])
                          (x.Rows |> List.map TableRow.Render |> Seq.cast)

    and TableStyle =
    | Bordered
    | Striped
    | Hover
        with
            static member ToCssClass =
                function
                | Bordered -> "table-bordered"
                | Striped  -> "table-striped"
                | Hover    -> "table-hover"

    and TableRow = {
        Status: TableRowStatus
        Data: Doc list
    } with
        static member Empty =
            { Status = TableRowStatus.Normal
              Data = [] }

        static member AddData data (x: TableRow) =
            { x with Data = data }

        static member SetStatus status (x: TableRow) =
            { x with Status = status }

        static member Render (x: TableRow) =
            trAttr [ attr.``class`` (string x.Status) ] 
                   (x.Data |> List.map (fun d -> td [ d ] :> Doc))

    and TableRowStatus =
        | Normal
        | Active
        | Success
        | Warning
        | Danger
        | Info
        with 
            override x.ToString() = 
                match x with 
                | Normal -> "" 
                | Active -> "active" 
                | Success -> "success" 
                | Warning -> "warning" 
                | Danger -> "danger" 
                | Info -> "info" 
