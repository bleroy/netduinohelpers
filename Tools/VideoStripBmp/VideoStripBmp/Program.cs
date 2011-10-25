using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace netduino.helpers.tools {
    class Program {
        static void Main(string[] args) {
            var list = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.bmp");
            foreach(var file in list) {
                ProcessBitmap(file);
            }
        }

        const int BytesPerPixel = 3;
        private static byte[] pixelBuffer = new byte[BytesPerPixel];
        const int GreenByte = 0;
        const int RedByte = 1;
        const int BlueByte = 2;

        public static void ProcessBitmap(string path) {
            using(var bmp = new Bitmap(path)) {
                if(bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb) {
                    Console.WriteLine("Please provide a 24-bit depth bitmap to convert.");
                    return;
                }
                var periodPosition = path.LastIndexOf('.');
                var filename = path.Substring(0, periodPosition);
                FileStream bin = new FileStream((string)(filename + ".vsbin"), FileMode.Create, FileAccess.Write);
                for(int row = 0; row < bmp.Height; row++) {
                    for(int column = 0; column < bmp.Width; column++) {
                        var pixel = bmp.GetPixel(column, row);
                        pixelBuffer[GreenByte] = (byte)(pixel.G | 0x80);
                        pixelBuffer[RedByte] = (byte)(pixel.R | 0x80);
                        pixelBuffer[BlueByte] = (byte)(pixel.B | 0x80);
                        bin.Write(pixelBuffer, 0, BytesPerPixel);
                        Console.Write("0x{0:x},0x{1:x},0x{2:x},", pixelBuffer[GreenByte], pixelBuffer[RedByte], pixelBuffer[BlueByte]);
                    }
                    Console.Write("\r\n");
                }
                bin.Close();
                Console.WriteLine("");
            }
        }
    }
}