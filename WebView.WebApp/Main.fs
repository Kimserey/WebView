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
open Bootstrap.NavBar
open Bootstrap.Table
open WebSharper.Data
open FSharp.Data

[<JavaScript>]
module Main =

    type Endpoint =
        | Shop of string
        | Expense of string
        | Listing

    type Nav = NavBar<Endpoint>
    type NavMenu = NavBarMenu<Endpoint>

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

        let navBar() =
            let leftMenu =
                NavMenu.Create Left 
                |> NavMenu.AddLinks
                    [ Hyperlink.Create(Action(fun () -> route.Value <- Listing), "Shops / Expenses")
                      |> Hyperlink.SetId "listing"
                      Hyperlink.Create(Action(fun () -> route.Value <- Shop "Waitrose"), "Waitrose")
                      |> Hyperlink.SetId "shop-waitrose" ]
                |> NavMenu.IsActive (fun link endpoint -> 
                    match link.Id, endpoint with
                    | Some "listing", Listing
                    | Some "shop-waitrose", Shop "Waitrose"-> true
                    | _ -> false)

            Nav.Create (NavBrand.Create ignore (text "WebView test"))
            |> Nav.SetLeftMenu leftMenu
            |> Nav.Render route.View

        let shopsTable() =
            Table.Empty
            |> Table.SetStyle [ TableStyle.Hover; TableStyle.Striped; TableStyle.Bordered ]
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
            |> Table.SetStyle [ TableStyle.Hover; TableStyle.Striped; TableStyle.Bordered ]
            |> Table.AddHeaders [ "#"; "Name"; "Location"; "Category"; "Price" ]
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
        
        navBar()
        |> Doc.RunById "nav"

        route.View
        |> Doc.BindView(fun endpoint ->
            match endpoint with
            | Shop name -> h1 [ text ("Shop " + name) ]
            | Expense name -> h1 [ text ("Expense " + name) ]
            | Listing -> div [ nav; content ])
        |> Doc.RunById "main"
