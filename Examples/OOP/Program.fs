// namespace Church
module OOP

open System

type FirstName = string
type LastName = string
type Name = string
type Email = string
type Phone = string

type ContactMethod =
    | Email of Email
    | Phone of Phone
    | EmailAndPhone of Email * Phone

type ContactInfo = {
    FirstName : FirstName
    MiddleName : Name option
    LastName : LastName
    Contact : ContactMethod
}

type MyContact
    ( firstName : FirstName,
      lastName : LastName,
      contact : ContactMethod,
      ?middleName : Name) = // optional argument to constuctor

    // private property
    let middleOrDefault = Option.defaultValue "" middleName

    // private mutable prop (backing store)
    let mutable dont = true

    // do stuff upon construction
    do printfn "hello %s" firstName

    // alternative, overloaded constructor
    // must call primary constructor as final statement
    new () =
        MyContact( "Reodor", "Felgen", Phone "1234321" )

    // initialze immutable properties, with implicit getters
    member this.FirstName = firstName

    member this.LastName = lastName

    member this.MiddleName = middleOrDefault

    member this.Contact = contact

    // public property with getter and setter
    member this.Dont
        with get () = dont
        and set x = dont <- x

    // automatic (mutable) property with getter and setter
    member val Life = 42 with get, set

    // static member, without "this"
    static member Hello x = "Hello " + x

    // abstract member
    abstract member ShowContact : unit -> string

    // default implmentation. can be overridden
    default this.ShowContact () =
        match this.Contact with
        | Email e -> e
        | Phone nr -> nr
        | EmailAndPhone (e, nr)-> e + " (" + nr + ")"

    // override default ToString from obj
    override this.ToString () =
        sprintf """
           %s%s %s %A
        """ this.FirstName (
            match middleName with
            | Some x -> " x"
            | None -> ""
            ) this.LastName this.Contact

// type/class extension
type MyContact with
    member this.Cat x = x

let prettyPrint =
    function
    | Email e -> string e
    | Phone nr -> string nr
    | EmailAndPhone (e, nr)-> sprintf "e-mail: <%s>, phone: %s" e nr

// inheritance
type PrettyContact
    ( firstName : FirstName,
      lastName : LastName,
      contact : ContactMethod) =
    inherit MyContact(firstName, lastName, contact)

    override this.ShowContact () =
        prettyPrint this.Contact

// interface definititon. note the "missing" () -> no constructor
type IContactInfo =
    abstract ShowContact : unit -> string

// ordinary record/union types can have members and implement interfaces
type OtherContact =
    {
        contact : ContactMethod
    }
    // interface impementation
    interface IContactInfo with
        member this.ShowContact () =
            prettyPrint this.contact

// implementation of an interface using a _object expression_
let prettyContact c =
    { new IContactInfo
      with member this.ShowContact () = prettyPrint c
    }

// empty type with overloaded members A.
type MoreStuff =
    static member A (x: int) = x
    static member A (x: string) = x

    // member B is generic with a type _constraint_
    static member B<'T when 'T :> IContactInfo> (x: 'T) = x.ShowContact ()

[<EntryPoint>]
let main argv =
    let x = MyContact()
    printfn "%A" x
    printfn "%s" x.FirstName
    let y = { contact = EmailAndPhone ("hello@world.io", "123") }
    let y' = y :> IContactInfo
    printfn "%s" <| y'.ShowContact ()
    printfn "%s" <| MoreStuff.B y

    0 // return an integer exit code

