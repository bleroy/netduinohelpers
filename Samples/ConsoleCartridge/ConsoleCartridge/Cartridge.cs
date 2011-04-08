using System;
using Microsoft.SPOT;
using netduino.helpers.Hardware;
using netduino.helpers.Fun;
using netduino.helpers.Helpers;
using SecretLabs.NETMF.Hardware;

namespace ConsoleCartridge {
    public class Cartridge {
        public void Run(object[] args) {
            if (100 == (int)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.Version]) {
                AnalogJoystick JoystickLeft = (AnalogJoystick)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.JoystickLeft];
                AnalogJoystick JoystickRight = (AnalogJoystick)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.JoystickRight];
                Max72197221 Matrix = (Max72197221)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.Matrix];
                PWM Speaker = (PWM)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.Speaker];
                SDResourceLoader Resources = (SDResourceLoader)args[(int)CartridgeVersionInfo.LoaderArgumentsVersion100.SDResourceLoader];

                Debug.Print(JoystickLeft.GetType().FullName);
                Debug.Print(JoystickRight.GetType().FullName);
                Debug.Print(Matrix.GetType().FullName);
                Debug.Print(Speaker.GetType().FullName);
                Debug.Print(Resources.GetType().FullName);

                Debug.EnableGCMessages(true);
                Debug.GC(true);
            } else {
                throw new ArgumentException("args");
            }
        }
    }
}
