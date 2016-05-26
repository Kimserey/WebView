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

[<JavaScript>]
module Main =

    let main =
        
        let shopsTable() =
            Table.Empty
            |> Table.AddHeaders [ "#"; "Name"; "Location"; "Category" ]
            |> Table.AddRowText [ "1"; "Waitrose"; "London"; "Supermarket" ]
            |> Table.AddRowText [ "2"; "Waitrose"; "London"; "Supermarket" ]
            |> Table.AddRowText [ "3"; "Waitrose"; "London"; "Supermarket" ]
            |> Table.AddRowText [ "4"; "Waitrose"; "London"; "Supermarket" ]
            |> Table.Render

        let shopsTab =
            NavTab.Create("shops", "Shops")
            |> NavTab.AddContent(shopsTable())
            |> NavTab.SetState NavTabState.Active
            
        let expensesTable() =
            Table.Empty
            |> Table.AddHeaders [ "#"; "Name"; "Location"; "Category"; "Price" ]
            |> Table.AddRowText [ "1"; "Bread"; "London"; "Supermarket"; "$1" ]
            |> Table.AddRowText [ "2"; "Bread"; "London"; "Supermarket"; "$1" ]
            |> Table.AddRowText [ "3"; "Bread"; "London"; "Supermarket"; "$1" ]
            |> Table.AddRowText [ "4"; "Bread"; "London"; "Supermarket"; "$1" ]
            |> Table.Render

        let expensesTab =
            NavTab.Create("expenses", "Expenses")
            |> NavTab.AddContent(expensesTable())

        let (nav, content) =
            NavTabs.Create [ shopsTab; expensesTab ]
            |> NavTabs.Render
        
        div [ nav; content ]
        |> Doc.RunById "main"
