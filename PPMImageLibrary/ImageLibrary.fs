//
// F# based PPM image library. 
//
// Adam Socik
//
module PPMImageLibrary
#light

//
// DebugOutput:
//
// Outputs to console, which appears in the "Output" window pane of
// Visual Studio when you run with debugging (F5).
//
let rec private OutputImage(image:int list list) = 
  match image with
  | [ ] -> printfn "**END**"
  |  _  -> printfn "%A" image.Head
           OutputImage(image.Tail)
           
let DebugOutput(width:int, height:int, depth:int, image:int list list) =
  printfn "**HEADER**"
  printfn "W=%A, H=%A, D=%A" width height depth
  printfn "**IMAGE**"
  OutputImage(image)

//
// TransformFirstRowWhite:
//
// An example transformation: replaces the first row of the given image
// with a row of all white pixels.
//
let rec BuildRowOfWhite cols white = 
  match cols with
  | 0 -> []
  | _ -> // 1 pixel, i.e. RGB value, followed by remaining pixels:
         white :: white :: white :: BuildRowOfWhite (cols-1) white

let TransformFirstRowWhite(width:int, height:int, depth:int, image:int list list) = 
  // first row all white :: followed by rest of original image
  BuildRowOfWhite width depth :: image.Tail

//---------------------------------------------------------------------------------------

// Converts a list to a string separated by spaces
// 
// Used for help:
// http://stackoverflow.com/questions/19469252/convert-integer-list-to-a-string
let listToString L = 
    L |> List.map (sprintf "%A") |> String.concat " " 

//
// WriteP3Image:
//
// Writes the given image out to a text file, in "P3" format.  Returns true if successful,
// false if not.
//
let WriteP3Image(filepath:string, width:int, height:int, depth:int, image:int list list) = 
    let body = List.map (listToString) image 
    let file = "P3" :: (string) width :: (string) height :: (string) depth :: body

    System.IO.File.WriteAllLines(filepath, file)
    true  // success

let rec BuildRowOfGrayScale (L: int list) =
    match L with
    | [] -> []
    | _  -> // Average the RGB values for each pixel
        let averageColor = ((L.Head + L.Tail.Head + L.Tail.Tail.Head) / 3)
        averageColor :: averageColor :: averageColor :: BuildRowOfGrayScale L.Tail.Tail.Tail

let TransformGrayscale(width:int, height:int, depth:int, image:int list list) = 
    List.map(BuildRowOfGrayScale) image

let rec BuildRowOfInvert depth (L: int list) =
    match L with
    | [] -> []
    | _  -> // Calculate the inverted value for each pixel color
        let newR = depth - L.Head
        let newG = depth - L.Tail.Head
        let newB = depth - L.Tail.Tail.Head
        newR :: newG :: newB :: BuildRowOfInvert depth L.Tail.Tail.Tail

let TransformInvert(width:int, height:int, depth:int, image:int list list) = 
    List.map(BuildRowOfInvert depth) image

let rec BuildRowOfHFlip (L: int list) =
    match L with
    | [] -> []
    | _  -> (BuildRowOfHFlip L.Tail.Tail.Tail) @ [L.Head; L.Tail.Head; L.Tail.Tail.Head]

let TransformFlipHorizontal(width:int, height:int, depth:int, image:int list list) = 
    List.map(BuildRowOfHFlip) image

// Flips an image vertically
let TransformFlipVertical(width:int, height:int, depth:int, image:int list list) =
    List.rev image

// Takes in a list of lists and returns a list of all the first pixels
// from each row.
let rec getHeadPixel (L: int list list) (result: int list) = 
    match L with
    | [] -> result
    | _  -> getHeadPixel L.Tail (L.Head.Head :: L.Head.Tail.Head :: L.Head.Tail.Tail.Head :: result)

let getNextPixel (L: int list) = 
    L.Tail.Tail.Tail

// This function rotates an image by 90 degrees. You just transpose the
// matrix of pixels.
//
// Used for help:
// http://stackoverflow.com/questions/3016139/help-me-to-explain-the-f-matrix-transpose-function
let rec rotate = function
    | (_::_)::_ as M -> (getHeadPixel M []) :: rotate (List.map getNextPixel M)
    | _ -> []

let RotateRight90(width:int, height:int, depth:int, image:int list list) =
    rotate image 