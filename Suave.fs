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
