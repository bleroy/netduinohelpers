using Microsoft.SPOT;
using netduino.helpers.Fun;
using netduino.helpers.Hardware;
using netduino.helpers.Helpers;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Pong {
    /*
    Copyright (C) 2011 by Bertrand Le Roy & Fabien Royer

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
    /// The project is a simple game of Pong.
    /// It is intended to demonstrate how to build a game with the netduino.helpers and how to turn your game into a 'cartridge' on an SD card.
    /// To use the game as a cartridge, deploy the \Sample\ConsoleBootLoader to your netduino first. It will bootstrap the game from an SD card.
    /// Place the .pe file (\Samples\Pong\Pong\bin\Debug\le\Pong.pe or \Samples\Pong\Pong\bin\Release\le\Pong.pe) at the root of an SD card.
    /// Place a text file named 'cartridge.txt' at the root of the SD card.
    /// Inside of 'cartridge.txt', write the following line of text:
    /// assembly:file=Pong.pe;name=Pong;version=1.0.0.0;class=Pong.Program;method=Run
    /// The ConsoleBootLoader will use the content of 'cartridge.txt' to find the entry point of the game and it will start it.
    /// </summary>
    public class Program {
        /// <summary>
        /// During development, Main() acts as the ConsoleBootLoader, making it easy to debug the game.
        /// When game development is complete, comment out the content Main() to remove the overhead
        /// </summary>
        public static void Main() {
            /*
            AnalogJoystick JoystickLeft = new AnalogJoystick(Pins.GPIO_PIN_A0, Pins.GPIO_PIN_A1);
            AnalogJoystick JoystickRight = new AnalogJoystick(Pins.GPIO_PIN_A2, Pins.GPIO_PIN_A3);
            Max72197221 Matrix = new Max72197221(chipSelect: Pins.GPIO_PIN_D8);
            PWM Speaker = new PWM(Pins.GPIO_PIN_D5);
            SDResourceLoader ResourceLoader = new SDResourceLoader();
            object[] args = new object[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.Size];

            int index = 0;
            args[index++] = CartridgeVersionInfo.CurrentVersion;
            args[index++] = JoystickLeft;
            args[index++] = JoystickRight;
            args[index++] = Matrix;
            args[index++] = Speaker;
            args[index++] = ResourceLoader;

            Matrix.Shutdown(Max72197221.ShutdownRegister.NormalOperation);
            Matrix.SetDecodeMode(Max72197221.DecodeModeRegister.NoDecodeMode);
            Matrix.SetDigitScanLimit(7);
            Matrix.SetIntensity(8);

            Run(args);
            */
        }

        /// <summary>
        /// Entry point called by the ConsoleBootLoader project
        /// </summary>
        /// <param name="args">Array of object references to the hardware features</param>
        public static void Run(object[] args) {
            var thread = new GameOfPong(new ConsoleHardwareConfig(args)).Run();
            thread.Join();
        }
    }
}