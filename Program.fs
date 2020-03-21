// Learn more about F# at http://fsharp.org

open Suave.Combinators
open Suave.Console
open Suave.Http
open Suave.Successful

[<EntryPoint>]
let main argv =
  let request =
    { Route = ""
      Type = Suave.Http.GET }

  let response =
    { Content = ""
      StatusCode = 200 }

  let context =
    { Request = request
      Response = response }

  executeInLoop context ((OK "hello") >=> (OK "world") >=> (OK "foo"))
  0
