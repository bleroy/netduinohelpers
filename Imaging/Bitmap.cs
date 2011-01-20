using System;

namespace netduino.helpers.Imaging {
    /*
    Copyright (C) 2011 by Fabien Royer

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
    */
    /// <summary>
    /// Abstracts a 1 bit depth bitmap expressed as a flat array of 8 bit values
    /// </summary>
    public class Bitmap {
        public static readonly int FrameSize = 8;
        /// <summary>
        /// Bitmap data in hex.
        /// </summary>
        protected byte[] BitmapData = {0x0, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8};

        public Bitmap() {
            HeightModuloSize = 1;
            WidthModuloSize = 1;
        }

        public Bitmap(int heightInPixels, int widthInPixels, byte[] data) {
            if ((heightInPixels % 8) != 0 || (widthInPixels % 8) != 0) {
                throw new InvalidOperationException("height and width must be multiples of 8");
            }

            Redefine(heightInPixels, widthInPixels, data);
        }

        protected int HeightModuloSize { get; set; }
        protected int WidthModuloSize { get; set; }

        /// <summary>
        /// Width of the bitmap in pixels
        /// </summary>
        public int Width {
            get {
                return WidthModuloSize << 3;
            }
        }

        /// <summary>
        /// Height of the bitmap in pixels
        /// </summary>
        public int Height {
            get {
                return HeightModuloSize << 3;
            }
        }
        /// <summary>
        /// Allows the bitmap to be redefined with a new one at runtime
        /// </summary>
        /// <param name="heightInPixels">Height of the new bitmap in pixels</param>
        /// <param name="widthInPixels">Width of the new bitmap in pixels</param>
        /// <param name="data">The new bitmap</param>
        protected void Redefine(int heightInPixels, int widthInPixels, byte[] data)
        {
            if ((heightInPixels % 8) != 0 || (widthInPixels % 8) != 0)
            {
                throw new InvalidOperationException("height and width must be multiples of 8");
            }

            HeightModuloSize = heightInPixels >> 3;
            WidthModuloSize = widthInPixels >> 3;
            BitmapData = data;
        }

        /// <summary>
        /// Takes x and y coordinates in pixels and returns a corresponding 8*8 frame
        /// </summary>
        /// <returns>An 8*8 frame, whose upper left corner is x and y</returns>
        public byte[] this[int x, int y] {
            get {
                if (x < 0 || x > Width) {
                    throw new ArgumentOutOfRangeException("x");
                }

                if (y < 0 || y > Height) {
                    throw new ArgumentOutOfRangeException("y");
                }

                var bitmapX = x / FrameSize; // Divide x by frameSize to determine where the x coordinate lands in the bitmap
                var xOffset = x % FrameSize; // Determine the amount of horizontal scrolling required to show the frame at this position

                var frame = new byte[FrameSize]; // Create the final frame

                var endLine = (y + FrameSize); // determine the ending line in the bitmap to create the final frame

                if (endLine > Height) // don't get out of bounds!
                {
                    endLine = Height;
                }

                for (int line = y, frameLine = 0; line < endLine; line++, frameLine++) // Build the frame one line at a time
                {
                    var index = line * WidthModuloSize + bitmapX; // determine the location of the graphics in the bitmap matrix

                    if (xOffset == 0) // if no scrolling is required, stored the graphics as-is
                    {
                        frame[frameLine] = BitmapData[index];
                    }
                    else // we need to merge / scroll two graphics to make one line
                    {
                        var merged = BitmapData[index];
                        merged <<= (byte)(xOffset);
                        var neighbor = BitmapData[index + 1];
                        neighbor >>= (byte)(FrameSize - xOffset);
                        merged |= neighbor;
                        frame[frameLine] = merged;
                    }
                }

                return frame;
            }
        }
    }
}
