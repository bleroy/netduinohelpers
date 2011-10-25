using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace netduino.helpers.Hardware {
    // Based on https://github.com/adafruit/LPD8806
    public class AdaFruitLPD8806 : IDisposable {

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int PixelCount { get; private set; }
        public int FrameSize { get; private set; }
        public const int BytesPerPixel = 3;
        protected int PixelBufferEnd;

        public AdaFruitLPD8806(int width, int height, Cpu.Pin chipSelect, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = 1000) {
            
            Width = width;
            Height = height;
            PixelCount = Width * Height;
            PixelBufferEnd = (PixelCount - 1) * BytesPerPixel;
            FrameSize = Width * Height * BytesPerPixel;

            var spiConfig = new SPI.Configuration(
                SPI_mod: spiModule,
                ChipSelect_Port: chipSelect,
                ChipSelect_ActiveState: false,
                ChipSelect_SetupTime: 0,
                ChipSelect_HoldTime: 0,
                Clock_IdleState: false,
                Clock_Edge: true,
                Clock_RateKHz: speedKHz
                );

            spi = new SPI(spiConfig);

            pixelBuffer = new byte[PixelCount * BytesPerPixel];
        }

        protected SPI spi;
        protected byte[] pixelBuffer;
        protected byte[] attentionSequence = new byte[] { 0, 0, 0, 0 };
        protected byte[] latchSequence = new byte[] { 0, 0, 0 };

        public void Reset() {
            for (var i = 0; i < pixelBuffer.Length; i++) {
                pixelBuffer[i] = 0x80;
            }
        }

        public void SetStripColor(UInt32 color) {
            for (var pixel = 0; pixel < PixelCount; pixel++) {
                SetPixel(pixel, color);
            }
        }

        public void Refresh(int delayMS = 4) {
            spi.Write(attentionSequence);
            spi.Write(pixelBuffer);
            var ledLatchCount = PixelCount * 2;
            for (var i = 0; i < ledLatchCount; i++) {
                spi.Write(latchSequence);
            }
            Thread.Sleep(delayMS);
        }

        public UInt32 RgbToColor(byte r, byte g, byte b) {
          // Take the lowest 7 bits of each value and append them end to end
          // We have the top bit set high (its a 'parity-like' bit in the protocol and must be set!
          UInt32 color;
          color = (UInt32)(g | 0x80);
          color <<= 8;
          color |= (UInt32)(r | 0x80);
          color <<= 8;
          color |= (UInt32)(b | 0x80);
          return color;
        }

        public void SetPixel(int pixelIndex, byte r, byte g, byte b) {
            if (pixelIndex >= PixelCount) throw new ArgumentOutOfRangeException("pixelIndex");
            pixelBuffer[pixelIndex * 3] = (byte)(g | 0x80);
            pixelBuffer[pixelIndex * 3 + 1] = (byte)(r | 0x80);
            pixelBuffer[pixelIndex * 3 + 2] = (byte)(b | 0x80);
        }

        // The color parameter needs to come from RgbToColor()
        public void SetPixel(int pixelIndex, UInt32 color) {
            if (pixelIndex >= PixelCount) throw new ArgumentOutOfRangeException("pixelIndex");
            pixelBuffer[pixelIndex * 3] = (byte)(color >> 16);
            pixelBuffer[pixelIndex * 3 + 1] = (byte)(color >> 8);
            pixelBuffer[pixelIndex * 3 + 2] = (byte)(color);
        }

        public void SetPixel(int x, int y, UInt32 color) {
            SetPixel((y * Width) + x, color);
        }

        protected byte[] singlePixelBuffer = new byte[BytesPerPixel];

        public void ShiftRight(bool circular = false) {
            if (circular) {
                Array.Copy(pixelBuffer, PixelBufferEnd, singlePixelBuffer, 0, BytesPerPixel);
                Array.Copy(pixelBuffer, 0, pixelBuffer, BytesPerPixel, PixelBufferEnd);
                Array.Copy(singlePixelBuffer, pixelBuffer, BytesPerPixel);
            } else {
                singlePixelBuffer[0] = 0x80;
                singlePixelBuffer[1] = 0x80;
                singlePixelBuffer[2] = 0x80;
                Array.Copy(pixelBuffer, 0, pixelBuffer, BytesPerPixel, PixelBufferEnd);
                Array.Copy(singlePixelBuffer, pixelBuffer, BytesPerPixel);
            }
        }

        public void ShiftLeft(bool circular = false) {
            if (circular) {
                Array.Copy(pixelBuffer, 0, singlePixelBuffer, 0, BytesPerPixel);
                Array.Copy(pixelBuffer, 3, pixelBuffer, 0, PixelBufferEnd);
                Array.Copy(singlePixelBuffer, 0, pixelBuffer, PixelBufferEnd, BytesPerPixel);
            } else {
                singlePixelBuffer[0] = 0x80;
                singlePixelBuffer[1] = 0x80;
                singlePixelBuffer[2] = 0x80;
                Array.Copy(pixelBuffer, 3, pixelBuffer, 0, PixelBufferEnd);
                Array.Copy(singlePixelBuffer, 0, pixelBuffer, PixelBufferEnd, BytesPerPixel);
            }
        }

        public void Gradient(int startRed, int startGreen, int startBlue, int endRed, int endGreen, int endBlue, int pixelStart, int pixelEnd) {
            var pixelRange = pixelEnd - pixelStart;
            for (var pixel = pixelStart; pixel <= pixelEnd; pixel++) {
                var ratio = (float)pixel / (float)pixelRange;
                SetPixel(pixel,
                    (byte)(endRed * ratio + startRed * (1 - ratio)),
                    (byte)(endGreen * ratio + startGreen * (1 - ratio)),
                    (byte)(endBlue * ratio + startBlue * (1 - ratio))
                    );
            }
        }

        public void CopyVideoStripBitmap(byte[] bitmap, int sourceBufferOffset = 0) {
            var length = Width * BytesPerPixel;
            for (var row = 0; row < Height; row += 2) {
                var offSet = Width * row * BytesPerPixel;
                Array.Copy(bitmap, offSet + sourceBufferOffset, pixelBuffer, offSet, length);
            }
            for (var row = 1; row < Height; row += 2) {
                var offSet = Width * row * BytesPerPixel;
                var targetCount = length - BytesPerPixel;
                var sourceCount = 0;
                while (targetCount >= 0) {
                    Array.Copy(bitmap, sourceBufferOffset + offSet + sourceCount, pixelBuffer, offSet + targetCount, BytesPerPixel);
                    targetCount -= BytesPerPixel;
                    sourceCount += BytesPerPixel;
                }
            }
        }

        public void Dispose() {
            singlePixelBuffer = null;
            pixelBuffer = null;
            attentionSequence = null;
            latchSequence = null;
            spi = null;
        }
    }
}
