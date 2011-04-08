#define NETDUINO

using System.IO;
using System.Threading;
using SecretLabs.NETMF.Hardware;
using netduino.helpers.Hardware;
using netduino.helpers.Helpers;
using netduino.helpers.Fun;

#if NETDUINO_MINI
using SecretLabs.NETMF.Hardware.NetduinoMini;
#else
using SecretLabs.NETMF.Hardware.Netduino;
#endif

namespace ConsoleBootLoader {
    public class Program {

#if NETDUINO_MINI
        // Use this document to see the pin map of the mini: http://www.netduino.com/netduinomini/schematic.pdf
        public static AnalogJoystick JoystickLeft = new AnalogJoystick(Pins.GPIO_PIN_5, Pins.GPIO_PIN_6);
        public static AnalogJoystick JoystickRight = new AnalogJoystick(Pins.GPIO_PIN_7, Pins.GPIO_PIN_8);
        public static Max72197221 Matrix = new Max72197221(chipSelect: Pins.GPIO_PIN_17);
        public static PWM Speaker = new PWM(Pins.GPIO_PIN_18);
#else
        public static AnalogJoystick JoystickLeft = new AnalogJoystick(Pins.GPIO_PIN_A0, Pins.GPIO_PIN_A1);
        public static AnalogJoystick JoystickRight = new AnalogJoystick(Pins.GPIO_PIN_A2, Pins.GPIO_PIN_A3);
        public static Max72197221 Matrix = new Max72197221(chipSelect: Pins.GPIO_PIN_D8);
        public static PWM Speaker = new PWM(Pins.GPIO_PIN_D5);
#endif

        public static SDResourceLoader ResourceLoader = new SDResourceLoader();

        public static object[] args = new object[(int) CartridgeVersionInfo.LoaderArgumentsVersion100.Size];

        public static void Main() {
            try {
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

#if NETDUINO_MINI
                ResourceLoader.Load(Pins.GPIO_PIN_13, args: args);
#else
                ResourceLoader.Load(Pins.GPIO_PIN_D10, resourceManifest: "cartridge.txt", args: args);
#endif
            } catch (IOException) {
                Matrix.Display(new byte[] { 0x7e, 0x42, 0x42, 0x42, 0x42, 0x42, 0x22, 0x1e });
                while (true) {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
