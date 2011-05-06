#define NETDUINO

using System.IO;
using System.Threading;
using Microsoft.SPOT.Hardware;
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
        public static PushButton ButtonLeft = new PushButton(Pins.GPIO_PIN_19, Port.InterruptMode.InterruptEdgeLevelLow, null, Port.ResistorMode.PullUp);
        public static PushButton ButtonRight = new PushButton(Pins.GPIO_PIN_20, Port.InterruptMode.InterruptEdgeLevelLow, null, Port.ResistorMode.PullUp);
#else
        public static AnalogJoystick JoystickLeft = new AnalogJoystick(Pins.GPIO_PIN_A0, Pins.GPIO_PIN_A1);
        public static AnalogJoystick JoystickRight = new AnalogJoystick(Pins.GPIO_PIN_A2, Pins.GPIO_PIN_A3);
        public static Max72197221 Matrix = new Max72197221(chipSelect: Pins.GPIO_PIN_D8);
        public static PWM Speaker = new PWM(Pins.GPIO_PIN_D5);
        public static PushButton ButtonLeft = new PushButton(Pins.GPIO_PIN_D0, Port.InterruptMode.InterruptEdgeLevelLow, null, Port.ResistorMode.PullUp);
        public static PushButton ButtonRight = new PushButton(Pins.GPIO_PIN_D1, Port.InterruptMode.InterruptEdgeLevelLow, null, Port.ResistorMode.PullUp);
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
                args[index++] = ButtonLeft;
                args[index] = ButtonRight;

                Matrix.Shutdown(Max72197221.ShutdownRegister.NormalOperation);
                Matrix.SetDecodeMode(Max72197221.DecodeModeRegister.NoDecodeMode);
                Matrix.SetDigitScanLimit(7);
                Matrix.SetIntensity(8);

                Matrix.Display(new byte[] { 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55, 0xAA, 0x55 });

#if NETDUINO_MINI
                ResourceLoader.Load(Pins.GPIO_PIN_13, resourceManifest: "cartridge.txt", args: new object[] {args});
#else
                ResourceLoader.Load(Pins.GPIO_PIN_D10, resourceManifest: "cartridge.txt", args: new object[] {args});
#endif
                DisplayAndWait(new byte[] { 0x7e, 0x42, 0x42, 0x42, 0x42, 0x42, 0x22, 0x1e });

            } catch (IOException) {
                DisplayAndWait(new byte[] { 0x81, 0x42, 0x3c, 0x5a, 0x7e, 0x24, 0x5a, 0x81 });
            }
        }

        private static void DisplayAndWait(byte[] bitmap) {
            Matrix.Display(bitmap);
            while (true) {
                Thread.Sleep(1000);
            }
        }
    }
}
