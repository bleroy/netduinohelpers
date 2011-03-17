using System;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.IO;
using SecretLabs.NETMF.Hardware.Netduino;

namespace netduino.helpers.Hardware {
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
    /// MAX7219 / 7221 LED display driver
    /// http://datasheets.maxim-ic.com/en/ds/MAX7219-MAX7221.pdf
    /// </summary>
    public class Max72197221 : IDisposable {
        public enum RegisterAddressMap {
            NoOp,
            Digit0,
            Digit1,
            Digit2,
            Digit3,
            Digit4,
            Digit5,
            Digit6,
            Digit7,
            DecodeMode,
            Intensity,
            ScanLimit,
            Shutdown,
            DisplayTest = 0x0F
        }

        public enum ShutdownRegister {
            ShutdownMode,
            NormalOperation
        }

        /// <summary>
        /// Logic OR the values of the DecodeModeRegister together or use DecodeDigitAll to decode all digits
        /// </summary>
        [Flags]
        public enum DecodeModeRegister {
            NoDecodeMode,
            DecodeDigit0,
            DecodeDigit1,
            DecodeDigit2,
            DecodeDigit3,
            DecodeDigit4,
            DecodeDigit5,
            DecodeDigit6,
            DecodeDigit7,
            DecodeDigitAll = 255
        }

        public enum CodeBFont {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Dash,
            E,
            H,
            L,
            P,
            Blank = 0x0F
        }

        public enum CodeBDecimalPoint {
            OFF,
            ON = 0x80
        }

        public enum DisplayTestRegister {
            NormalOperation,
            DisplayTestMode
        }

        /// <summary>
        /// Instantiates a Max7219/7221 LED driver, using the netduino's hard SPI interface by default:
        /// If multiple Max7219/7221 chips are chained together, the CS pin must be controlled by the caller instead of the netduino handling it.
        /// CLK = pin 13
        /// MOSI = pin 11
        /// CS = pin 10
        /// </summary>
        /// <param name="latchPin">SPI Hardware pin 10 by default. Any other pin if controlling multiple LED drivers.</param>
        /// <param name="spiModule">SPI module, SPI 1 is used by default.</param>
        /// <param name="speedKHz">Speed of the SPI bus in kHz. Set @ 2MHz by default.</param>
        public Max72197221(Cpu.Pin latchPin = Pins.GPIO_PIN_D10, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = (uint)2000) {
            var extendedSpiConfig = new ExtendedSpiConfiguration(
                SPI_mod: spiModule,
                ChipSelect_Port: latchPin,
                ChipSelect_ActiveState: false,
                ChipSelect_SetupTime: 0,
                ChipSelect_HoldTime: 0,
                Clock_IdleState: false,
                Clock_Edge: true,
                Clock_RateKHz: speedKHz,
                BitsPerTransfer: 16);

            Spi = new SPI(extendedSpiConfig);

            DigitScanLimitSafety = true;

            SpiBuffer = new ushort[1];
        }

        public void SetIntensity(byte value) {
            if (value < 0 || value > 15) {
                throw new ArgumentOutOfRangeException("value");
            }
            Write((byte) RegisterAddressMap.Intensity, value);
        }

        public bool DigitScanLimitSafety { get; set; }

        public void SetDigitScanLimit(byte value) {
            if (value < 0 || value > 7) {
                throw new ArgumentOutOfRangeException("value");
            }

            if (DigitScanLimitSafety && value < 3) {
                throw new ArgumentException("SetDigitScanLimitSafety value should not be set too low in order to keep within datasheet limits and protect your matrix or digits from burning out.");
            }
            Write((byte) RegisterAddressMap.ScanLimit, value);
        }

        public void SetDecodeMode(DecodeModeRegister value) {
            if (value < DecodeModeRegister.NoDecodeMode || value > DecodeModeRegister.DecodeDigitAll) {
                throw new ArgumentOutOfRangeException("value");
            }
            Write((byte) RegisterAddressMap.DecodeMode, (byte) value);
        }

        public void Shutdown(ShutdownRegister value = ShutdownRegister.ShutdownMode) {
            if (value != ShutdownRegister.NormalOperation && value != ShutdownRegister.ShutdownMode) {
                throw new ArgumentOutOfRangeException("value");
            }
            Write((byte) RegisterAddressMap.Shutdown, (byte) value);
        }

        public void SetDisplayTest(DisplayTestRegister value) {
            if (value != DisplayTestRegister.DisplayTestMode && value != DisplayTestRegister.NormalOperation) {
                throw new ArgumentOutOfRangeException("value");
            }
            Write((byte) RegisterAddressMap.DisplayTest, (byte) value);
        }

        /// <summary>
        /// Send an 8x8 matrix pattern to the LED driver.
        /// The LED driver requires DecodeMode = DecodeModeRegister.NoDecodeMode first.
        /// </summary>
        /// <param name="matrix">8x8 bitmap to be displayed.</param>
        public void Display(byte[] matrix) {
            if (matrix.Length != 8) {
                throw new ArgumentOutOfRangeException("matrix");
            }
            var rowNumber = (byte)RegisterAddressMap.Digit0;
            foreach (var rowData in matrix) {
                Write(rowNumber, rowData);
                rowNumber++;
            }
        }

        /// <summary>
        /// Translate a string into CodeBFont values and displays it.
        /// Each character followed by a '.' will be displayed with a decimal point.
        /// Unrecognized characters will be replaced by a 'blank' CodeBFont value.
        /// The string is scanned from the right to the left (LSB first).
        /// The LED driver requires DecodeMode != DecodeModeRegister.NoDecodeMode first.
        /// </summary>
        /// <param name="digits">A string containing characters from 0-9, '-', ' ', 'E', 'H', 'L', 'P' or '.'</param>
        public void Display(string digits) {
            var length = digits.Length;
            var decimalPoint = CodeBDecimalPoint.OFF;
            var digitPosition = RegisterAddressMap.Digit0;
            while (length != 0) {
                var c = digits[--length];                
                if (c == '.') {
                    decimalPoint = CodeBDecimalPoint.ON;
                    continue;
                }
                CodeBFont data;
                if (c >= '0' && c <= '9') {
                    data = (CodeBFont)(c - '0');
                } else {
                    switch (c) {
                        case '-':
                            data = CodeBFont.Dash;
                            break;
                        case 'E':
                            data = CodeBFont.E;
                            break;
                        case 'H':
                            data = CodeBFont.H;
                            break;
                        case 'L':
                            data = CodeBFont.L;
                            break;
                        case 'P':
                            data = CodeBFont.P;
                            break;
                        default:
                            data = CodeBFont.Blank;
                            break;
                    }
                }

                Display(digitPosition, data, decimalPoint);

                decimalPoint = CodeBDecimalPoint.OFF; 
                
                digitPosition++;
                if (digitPosition > RegisterAddressMap.Digit7) {
                    break;
                }
            }
        }

        /// Send a CodeBFont pattern to a specific digit register of the LED driver. No-op is acceptable.
        /// The LED driver requires DecodeMode != DecodeModeRegister.NoDecodeMode first.
        public void Display(RegisterAddressMap register, CodeBFont codeBFont, CodeBDecimalPoint decimalPoint) {
            if (register < RegisterAddressMap.NoOp || register > RegisterAddressMap.Digit7) {
                throw new ArgumentOutOfRangeException("register");
            }
            var data = (byte)codeBFont;
            data |= (byte)decimalPoint;
            Write((byte)register, data);
        }

        protected void Write(byte register, byte value) {
            SpiBuffer[0] = register;
            SpiBuffer[0] <<= 8;
            SpiBuffer[0] |= value;
            Spi.Write(SpiBuffer);
        }

        public void Dispose() {
            Spi.Dispose();
            Spi = null;
            SpiBuffer = null;
        }

        protected SPI Spi;
        protected ushort[] SpiBuffer;
    }
}
