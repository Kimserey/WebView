namespace WebView.WebApp

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.Resources
open Bootstrap.Hyperlink
open Bootstrap.NavTabs
open Bootstrap.Table
open WebSharper.Data
open FSharp.Data

[<JavaScript>]
module Main =

    type Endpoint =
        | Shop of string
        | Expense of string
        | Listing

    let main =

        let route = 
            RouteMap.Create 
                (function 
                 | Shop name    -> [ "shops"; name ] 
                 | Expense name -> [ "expenses"; name ] 
                 | Listing      -> [ "listing" ]) 
                (function
                 | [ "shops"; name ]    -> Shop name
                 | [ "expenses"; name ] -> Expense name
                 | _                    -> Listing)
            |> RouteMap.Install

        let shopsTable() =
            Table.Empty
            |> Table.SetStyle [ TableStyle.Hover; TableStyle.Striped ]
            |> Table.AddHeaders [ "#"; "Name"; "Location"; "Category" ]
            |> Table.AddRow (TableRow.Create [ text "1"; text "Waitrose"; text "London"; text "Supermarket" ] 
                             |> TableRow.OnClick (fun () -> route.Value <- Shop "Waitrose"))
            |> Table.AddRow (TableRow.Create [ text "2"; text "Aldi"; text "London"; text "Supermarket" ]
                             |> TableRow.OnClick (fun () -> route.Value <- Shop "Aldi"))
            |> Table.AddRow (TableRow.Create [ text "3"; text "Currys"; text "London"; text "Electronic" ]
                             |> TableRow.OnClick (fun () -> route.Value <- Shop "Currys"))
            |> Table.Render
            
        let shopsTab =
            NavTab.Create("shops", "Shops")
            |> NavTab.AddContent(shopsTable())
            |> NavTab.SetState NavTabState.Active
            
        let expensesTable() =
            Table.Empty
            |> Table.SetStyle [ TableStyle.Hover; TableStyle.Striped ]
            |> Table.AddRow (TableRow.Create [ text "#"; text "Name"; text "Location"; text "Category"; text "Price" ])
            |> Table.AddRow (TableRow.Create [ text "1"; text "Bread"; text "London"; text "Supermarket"; text "$1" ]
                             |> TableRow.OnClick (fun () -> route.Value <- Expense "Bread"))
            |> Table.AddRow (TableRow.Create [ text "2"; text "Coffee"; text "London"; text "Supermarket"; text "$1" ])
            |> Table.Render

        let expensesTab =
            NavTab.Create("expenses", "Expenses")
            |> NavTab.AddContent(expensesTable())

        let (nav, content) =
            NavTabs.Create [ shopsTab; expensesTab ]
            |> NavTabs.Render
        
        route.View
        |> Doc.BindView(fun endpoint ->
            match endpoint with
            | Shop name -> h1 [ text ("Shop " + name) ]
            | Expense name -> h1 [ text ("Expense " + name) ]
            | Listing -> div [ nav; content ])
        |> Doc.RunById "main"
