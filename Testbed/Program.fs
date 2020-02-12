// Learn more about F# at http://fsharp.org

open System

let fa = [
    (0, 0)
    (1, 1)
    (2, 4)
    (3, 9)
]

let lookup = fun n -> List.find (fst >> (=) n)

let f g a b = (a + b)**(g a)

let curry = fun f a b -> f (a, b)

let uncurry = fun f (a, b) -> f a b

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    0 // return an integer exit code
