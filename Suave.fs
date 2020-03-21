namespace Suave

module Http =
  type RequestType =
    | GET
    | POST

  type Request =
    { Route: string
      Type: RequestType }

  type Response =
    { Content: string
      StatusCode: int }

  type Context =
    { Request: Request
      Response: Response }

  type WebPart = Context -> Async<Context option>

module Successful =
  open Http

  let OK content context =
    { context with
        Response =
          { Content = content
            StatusCode = 200 } }
    |> Some
    |> async.Return

module Console =
  open Http

  let parseRequest (input: string) =
    let parts = input.Split ';'
    let rawType = parts.[0]
    let route = parts.[1]
    match rawType with
    | "GET" ->
        { Type = GET
          Route = route }
    | "POST" ->
        { Type = POST
          Route = route }
    | _ -> failwith "Invalid Request"

  let execute inputContext webPart =
    async {
      let! outputContext = webPart inputContext
      printfn "---"
      match outputContext with
      | Some context ->
          printfn "Code: %d" context.Response.StatusCode
          printfn "Output: %s" context.Response.Content
      | None ->
          printfn "No Output"
      printfn "---"
    }
    |> Async.RunSynchronously

  let executeInLoop inputContext webPart =
    let mutable continueLooping = true
    while continueLooping do
      printf "Enter Request: "
      let input = System.Console.ReadLine()
      try
        if input = "exit" then
          continueLooping <- false
        else
          let context = { inputContext with Request = parseRequest input }
          execute context webPart
      with ex -> printfn "Error: %s" ex.Message

module Combinators =
  let compose first second context =
    async {
      let! firstContext = first context
      match firstContext with
      | None -> return None
      | Some context ->
          let! secondContext = second context
          return secondContext
    }

  let (>=>) = compose

module Filters =
  open Http

  let iif condition context =
    if condition context then
      context
      |> Some
      |> async.Return
    else
      None |> async.Return
      
  let GET = iif (fun context -> context.Request.Type = GET)
  let POST = iif (fun context -> context.Request.Type = POST)
  let Path path = iif (fun context -> context.Request.Route = path)
