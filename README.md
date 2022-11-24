# ByteMap
A Simple tool to generate byte arrays from bitmaps. To use with Adafruit GFX library on Arduino boards and SSD1306 display controllers.
![2022-11-24 15_05_40-ByteMap](https://user-images.githubusercontent.com/617600/203808235-14eadcc0-98d4-4404-8401-5daa86ae3504.png)
Curently the image processing is using the System.Drawing.Common library, that is not supported on platforms other than Windows.

## Installation
No need to install anything, just build and run.

## Included in release
The project includes 2 tools:
- A WinForms GUI app where you can preview the result
- And a command line tool for the confident people.

## Usage
### Command line

There are 2 use cases:

For color images you can use
`bm image.png logo 192`
This will create a file named *logo.h* from *image.png* where all the pixels with RGB average below *192* are black

For already monochrome images use `bm image.png logo`
This is equivalent to the other use case with a threshold value of *128*
 
### GUI
Open an image.

For color images You can adjust the slider to set the threshold while looking at the histogram below.
The preview window shows the actual output.

_***Note:*** The preview window only shows the output's top left 128x64 pixel portion. Do not worry, the actual image is processed as a whole, so no data is lost._


## Results
The resulting c/c++/Arduino code is generated as a binary text, so (at least for small images) it is visible what the end result is: 
 ```
// Generated monochrome bitmap
// Size: 16x16
static const unsigned char PROGMEM bitmap[] = {
  0b11111111,0b11111111,
  0b11111000,0b00011111,
  0b11100000,0b00000111,
  0b11000000,0b00000011,
  0b11000000,0b00000011,
  0b10000000,0b00000001,
  0b10000011,0b11000001,
  0b00000011,0b11000000,
  0b00000011,0b11000000,
  0b10000011,0b11000001,
  0b10000001,0b10000001,
  0b10000001,0b10000001,
  0b11000011,0b11000011,
  0b11100011,0b11000111,
  0b11110011,0b11001111,
  0b11111111,0b11111111
};
// 32 bytes
 ```
<sub>If you have a hard time seeing the pattern, just hit _Ctrl+F_ and type *"1"*</sub>

