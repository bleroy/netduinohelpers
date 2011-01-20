using Microsoft.SPOT.Hardware;

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
    /// Abstracts a 74HC595 shift register accessed through SPI
    /// </summary>
    public class ShiftRegister74HC595 {
        protected SPI Spi;

        /// <summary>
        /// By default, SPI expects the clock signal (74HC595 SRCLK) on pin 13 and data (74HC595 SER) on the on pin 11. The latch pin is arbitrary.
        /// By default SPI1 is used on the netduino
        /// </summary>
        /// <param name="latchPin">Pin connected to register latch (called ST_CP or RCLK) of the 74HC595</param>
        public ShiftRegister74HC595(Cpu.Pin latchPin)
            : this(latchPin, SPI.SPI_module.SPI1) {
        }

        /// <summary>
        /// Code for using a 74HC595 Shift Register
        /// </summary>
        /// <param name="latchPin">Pin connected to register latch on the 74HC595</param>
        /// <param name="spiModule">SPI module being used to send data to the shift register</param>
        public ShiftRegister74HC595(Cpu.Pin latchPin, SPI.SPI_module spiModule) {
            var spiConfig = new SPI.Configuration(
                SPI_mod: spiModule,
                ChipSelect_Port: latchPin,
                ChipSelect_ActiveState: false,
                ChipSelect_SetupTime: 0,
                ChipSelect_HoldTime: 0,
                Clock_IdleState: false,
                Clock_Edge: true,
                Clock_RateKHz: 1000
                );
            Spi = new SPI(spiConfig);
        }
        /// <summary>
        /// Sends 8 bits to the shift register
        /// </summary>
        /// <param name="buffer"></param>
        public void Write(byte buffer) {
            Spi.Write(new[] {buffer});
        }

        /// <summary>
        /// Reverse the bits of the byte
        /// </summary>
        /// <param name="val">A byte value to be reversed</param>
        /// <returns>The byte with the reversed bits</returns>
        public byte FlipBits(byte val)
        {
            byte reversed = 0;

            int bit = 0;

            while (true) {
                if ((val & 1) == 1) {
                    reversed |= 1;
                }
                else {
                    reversed |= 0;
                }

                bit++;

                if (bit < 8) {
                    reversed <<= 1;
                    val >>= 1;
                }
                else {
                    break;
                }
            }

            return reversed;
        }
    }
}