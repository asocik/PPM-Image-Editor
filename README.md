#The PPM Image Editor
The PPM Image Editor can perform various operations on PPM formatted images. The editor was written with a C# front-end GUI and F# image processing back-end. The operations that the editor can preform on images include:

* Grayscale - average out RGB colors for each pixel
* Invert - invert each pixel by applying (newValue = MaxColorDepth - currentValue) on each one
* Flip image horizontally - flips left and right pixels
* Flip image vertically - flips the top and bottom rows of pixels
* Rotate right 90 degrees - transposes the matrix of pixels

The easiest way to run the program is to just clone it and open up the Visual Studio solution.
