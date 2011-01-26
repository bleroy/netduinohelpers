using System.Threading;
using Microsoft.SPOT.Hardware;
using netduino.helpers.Hardware;
using netduino.helpers.Imaging;
using SecretLabs.NETMF.Hardware.Netduino;

namespace ImagingSamples {
    public class Program {
        public static void Main() {
            var button = new InputPort(Pins.ONBOARD_SW1, false, Port.ResistorMode.Disabled);
            using (var matrix = new LedMS88SR74HC595().Initialize()) {
                // Oh, prototype is so sad!
                var sad = new byte[] {0x66, 0x24, 0x00, 0x18, 0x00, 0x3C, 0x42, 0x81};
                DisplayAndWait(sad, matrix, button);
                // Let's make it smile!
                var smile = new byte[] {0x42, 0x18, 0x18, 0x81, 0x7E, 0x3C, 0x18, 0x00};
                DisplayAndWait(smile, matrix, button);
                // TODO: All together now
            }
        }

        private static void DisplayAndWait(byte[] smile, LedMatrix matrix, InputPort button) {
            matrix.Set(smile);
            while (button.Read()) {
                Thread.Sleep(10);
            }
            while (!button.Read()) {
                Thread.Sleep(10);
            }
        }
    }
}
