// Learn more about F# at http://fsharp.org

open Suave.Combinators
open Suave.Console
open Suave.Filters
open Suave.Http
open Suave.Successful

[<EntryPoint>]
let main argv =
  let request =
    { Route = ""
      Type = GET }

  let response =
    { Content = ""
      StatusCode = 200 }

  let context =
    { Request = request
      Response = response }

  let app =
    Choose
      [ Get >=> Path "/hello" >=> OK "Hello GET"
        Post >=> Path "/hello" >=> OK "Hello POST"
        Path "/foo" >=> Choose
                          [ Get >=> OK "Foo GET"
                            Post >=> OK "Foo POST" ] ]

  executeInLoop context app
  0
