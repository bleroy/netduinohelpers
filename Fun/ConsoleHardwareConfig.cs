using System;
using netduino.helpers.Hardware;
using netduino.helpers.Fun;
using netduino.helpers.Helpers;
using SecretLabs.NETMF.Hardware;

namespace netduino.helpers.Fun {
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
    public class ConsoleHardwareConfig {

        public int Version { get; private set; }

        public AnalogJoystick JoystickLeft { get; set; }
        public AnalogJoystick JoystickRight { get; set; }
        public Max72197221 Matrix { get; set; }
        public PWM Speaker { get; set; }
        public SDResourceLoader Resources { get; set; }
        public PushButton LeftButton { get; set; }
        public PushButton RightButton { get; set; }

        public ConsoleHardwareConfig(object[] args) {
            Version = (int)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.Version];

            if (100 == Version) {
                JoystickLeft = (AnalogJoystick)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.JoystickLeft];
                JoystickRight = (AnalogJoystick)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.JoystickRight];
                Matrix = (Max72197221)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.Matrix];
                Speaker = (PWM)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.Speaker];
                Resources = (SDResourceLoader)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.SDResourceLoader];
                LeftButton = (PushButton)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.ButtonLeft];
                RightButton = (PushButton)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.ButtonRight];
            }
            else {
                throw new ArgumentException("args");
            }
        }
    }
}
