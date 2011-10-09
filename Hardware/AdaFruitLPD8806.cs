using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace netduino.helpers.Hardware {
    public class AdaFruitLPD8806 : IDisposable {
        int PixelCount { get; set; }
        public AdaFruitLPD8806(int pixels, Cpu.Pin chipSelect, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = 1000) {
            PixelCount = pixels;
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

            pixelBuffer = new byte[PixelCount * 3];
        }

        protected SPI spi;
        protected byte[] pixelBuffer;
        protected byte[] attentionSequence = new byte[] { 0, 0, 0, 0 };
        protected byte[] latchSequence = new byte[] { 0, 0, 0 };

        public void Reset() {
            for (var i = 0; i < pixelBuffer.Length; i++) {
                pixelBuffer[i] = 0;
            }
        }

        public void Refresh() {
            spi.Write(attentionSequence);
            spi.Write(pixelBuffer);
            var ledLatchCount = PixelCount * 2;
            for (var i = 0; i < ledLatchCount; i++) {
                spi.Write(latchSequence);
            }
            Thread.Sleep(10);
        }

        UInt32 RgbToColor(byte r, byte g, byte b)
        {
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

        public void SetPixel(int pixelIndex, UInt32 color) {
            if (pixelIndex >= PixelCount) throw new ArgumentOutOfRangeException("pixelIndex");
            pixelBuffer[pixelIndex * 3] = (byte)((color >> 16) | 0x80);
            pixelBuffer[pixelIndex * 3 + 1] = (byte)((color >> 8) | 0x80);
            pixelBuffer[pixelIndex * 3 + 2] = (byte)(color | 0x80);
        }

        public void Dispose() {
            pixelBuffer = null;
            attentionSequence = null;
            latchSequence = null;
            spi = null;
        }
    }
}
