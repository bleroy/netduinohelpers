using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;

namespace netduino.helpers.Hardware {
    /// <summary>
    /// netduino ST7735 driver (based on AdaFruit's library http://github.com/adafruit/ST7735-Library)
    /// </summary>
    public class AdaFruitST7735 : IDisposable {

        public const byte Width = 128;
        public const byte Height = 160;

        public enum LcdCommand {
            NOP = 0x0,
            SWRESET = 0x01,
            RDDID = 0x04,
            RDDST = 0x09,
            SLPIN = 0x10,
            SLPOUT = 0x11,
            PTLON = 0x12,
            NORON = 0x13,
            INVOFF = 0x20,
            INVON = 0x21,
            DISPOFF = 0x28,
            DISPON = 0x29,
            CASET = 0x2A,
            RASET = 0x2B,
            RAMWR = 0x2C,
            RAMRD = 0x2E,
            COLMOD = 0x3A,
            MADCTL = 0x36,
            FRMCTR1 = 0xB1,
            FRMCTR2 = 0xB2,
            FRMCTR3 = 0xB3,
            INVCTR = 0xB4,
            DISSET5 = 0xB6,
            PWCTR1 = 0xC0,
            PWCTR2 = 0xC1,
            PWCTR3 = 0xC2,
            PWCTR4 = 0xC3,
            PWCTR5 = 0xC4,
            VMCTR1 = 0xC5,
            RDID1 = 0xDA,
            RDID2 = 0xDB,
            RDID3 = 0xDC,
            RDID4 = 0xDD,
            PWCTR6 = 0xFC,
            GMCTRP1 = 0xE0,
            GMCTRN1 = 0xE1
        }

        public enum Colors {
            Black = 0x0000,
            Blue = 0x001F,
            Red = 0xF800,
            Green = 0x07E0,
            Cyan = 0x07FF,
            Magenta = 0xF81F,
            Yellow = 0xFFE0,
            White = 0xFFFF
        }

        public AdaFruitST7735(Cpu.Pin chipSelect, Cpu.Pin dc, Cpu.Pin reset, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = (uint)9500) {
            DataCommand = new OutputPort(dc, false);
            Reset = new OutputPort(reset, true);

            var extendedSpiConfig = new ExtendedSpiConfiguration(
                SPI_mod: spiModule,
                ChipSelect_Port: chipSelect,
                ChipSelect_ActiveState: false,
                ChipSelect_SetupTime: 0,
                ChipSelect_HoldTime: 0,
                Clock_IdleState: false,
                Clock_Edge: true,
                Clock_RateKHz: speedKHz,
                BitsPerTransfer: 8);

            Spi = new SPI(extendedSpiConfig);

            Initialize();
        }

        private void Initialize() {
            Reset.Write(true);
            Thread.Sleep(50);
            Reset.Write(false);
            Thread.Sleep(50);
            Reset.Write(true);
            Thread.Sleep(50);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.SWRESET); // software reset
            Thread.Sleep(150);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.SLPOUT);  // out of sleep mode
            Thread.Sleep(150);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.FRMCTR1);  // frame rate control - normal mode
            DataCommand.Write(Data);
            Write(0x01);  // frame rate = fosc / (1 x 2 + 40) * (LINE + 2C + 2D)
            Write(0x2C);
            Write(0x2D);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.FRMCTR2);  // frame rate control - idle mode
            DataCommand.Write(Data);
            Write(0x01);  // frame rate = fosc / (1 x 2 + 40) * (LINE + 2C + 2D)
            Write(0x2C);
            Write(0x2D);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.FRMCTR3);  // frame rate control - partial mode
            DataCommand.Write(Data);
            Write(0x01); // dot inversion mode
            Write(0x2C);
            Write(0x2D);
            Write(0x01); // line inversion mode
            Write(0x2C);
            Write(0x2D);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.INVCTR);  // display inversion control
            DataCommand.Write(Data);
            Write(0x07);  // no inversion

            DataCommand.Write(Command);
            Write((byte)LcdCommand.PWCTR1);  // power control
            DataCommand.Write(Data);
            Write(0xA2);
            Write(0x02);      // -4.6V
            Write(0x84);      // AUTO mode

            DataCommand.Write(Command);
            Write((byte)LcdCommand.PWCTR2);  // power control
            DataCommand.Write(Data);
            Write(0xC5);      // VGH25 = 2.4C VGSEL = -10 VGH = 3 * AVDD

            DataCommand.Write(Command);
            Write((byte)LcdCommand.PWCTR3);  // power control
            DataCommand.Write(Data);
            Write(0x0A);      // Opamp current small 
            Write(0x00);      // Boost frequency

            DataCommand.Write(Command);
            Write((byte)LcdCommand.PWCTR4);  // power control
            DataCommand.Write(Data);
            Write(0x8A);      // BCLK/2, Opamp current small & Medium low
            Write(0x2A);

            Write((byte)LcdCommand.PWCTR5);  // power control
            DataCommand.Write(Data);
            Write(0x8A);
            Write(0xEE);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.VMCTR1);  // power control
            DataCommand.Write(Data);
            Write(0x0E);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.INVOFF);    // don't invert display

            DataCommand.Write(Command);
            Write((byte)LcdCommand.MADCTL);  // memory access control (directions)
            DataCommand.Write(Data);
            Write(0xC8);  // row address/col address, bottom to top refresh

            DataCommand.Write(Command);
            Write((byte)LcdCommand.COLMOD);  // set color mode
            DataCommand.Write(Data);
            Write(0x05);        // 16-bit color

            DataCommand.Write(Command);
            Write((byte)LcdCommand.CASET);  // column addr set
            DataCommand.Write(Data);
            Write(0x00);
            Write(0x00);   // XSTART = 0
            Write(0x00);
            Write(0x7F);   // XEND = 127

            DataCommand.Write(Command);
            Write((byte)LcdCommand.RASET);  // row addr set
            DataCommand.Write(Data);
            Write(0x00);
            Write(0x00);    // XSTART = 0
            Write(0x00);
            Write(0x9F);    // XEND = 159

            DataCommand.Write(Command);
            Write((byte)LcdCommand.GMCTRP1);
            DataCommand.Write(Data);
            Write(0x02);
            Write(0x1c);
            Write(0x07);
            Write(0x12);
            Write(0x37);
            Write(0x32);
            Write(0x29);
            Write(0x2d);
            Write(0x29);
            Write(0x25);
            Write(0x2B);
            Write(0x39);
            Write(0x00);
            Write(0x01);
            Write(0x03);
            Write(0x10);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.GMCTRN1);
            DataCommand.Write(Data);
            Write(0x03);
            Write(0x1d);
            Write(0x07);
            Write(0x06);
            Write(0x2E);
            Write(0x2C);
            Write(0x29);
            Write(0x2D);
            Write(0x2E);
            Write(0x2E);
            Write(0x37);
            Write(0x3F);
            Write(0x00);
            Write(0x00);
            Write(0x02);
            Write(0x10);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.DISPON);
            Thread.Sleep(50);

            DataCommand.Write(Command);
            Write((byte)LcdCommand.NORON);  // normal display on
            Thread.Sleep(10);

            SetAddressWindow(0, 0, Width - 1, Height - 1);

            DataCommand.Write(Data);
        }

        protected void SetAddressWindow(byte x0, byte y0, byte x1, byte y1)
        {
            DataCommand.Write(Command);
            Write((byte)LcdCommand.CASET);  // column addr set
            DataCommand.Write(Data);
            Write(0x00);
            Write((byte) (x0 + 2));   // XSTART 
            Write(0x00);
            Write((byte) (x1 + 2));   // XEND

            DataCommand.Write(Command);
            Write((byte)LcdCommand.RASET);  // row addr set
            DataCommand.Write(Data);
            Write(0x00);
            Write((byte) (y0 + 1));    // YSTART
            Write(0x00);
            Write((byte) (y1 + 1));    // YEND

            DataCommand.Write(Command);
            Write((byte)LcdCommand.RAMWR);  // write to RAM
        }

        public void DrawPixel(byte x, byte y, ushort color) {
          if ((x >= Width) || (y >= Height)) return;
        }

        public void FillScreen(ushort color) {
            for(ushort I = 0; I < SpiBuffer.Length; I+=2){
                SpiBuffer[I] = (byte) (color >> 8);
                SpiBuffer[I+1] = (byte)(color);
            }
            Spi.Write(SpiBuffer);
        }

        unsafe public void SetPixel(byte x, byte y, ushort color) {
            fixed (byte* pSpiBuffer = SpiBuffer)
            {
                ushort* pSpiShortBuffer = (ushort*)pSpiBuffer + (((y * Width) + x) * sizeof(ushort));
                *pSpiShortBuffer = color;
            }
        }

        private const bool Data = true;
        private const bool Command = false;

        protected void Write(byte Byte) {
            SpiBOneByteBuffer[0] = Byte;
            Spi.Write(SpiBOneByteBuffer);
        }

        public void Dispose() {
            Spi.Dispose();
            Spi = null;
            DataCommand = null;
            Reset = null;
        }

        public readonly byte[] SpiBuffer = new byte[Width*Height*sizeof(ushort)];
        protected readonly byte[] SpiBOneByteBuffer = new byte[1];
        protected OutputPort DataCommand;
        protected OutputPort Reset;
        protected SPI Spi;

    }
}
