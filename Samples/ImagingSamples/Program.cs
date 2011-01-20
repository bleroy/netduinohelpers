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
                // All together now
                var composition = new Composition();
                composition.AddBitmap(
                    name: "scenery",
                    bitmap: new Bitmap(16, 16,
                        new byte[] {
                                       0x01, 0x80,
                                       0x02, 0x40,
                                       0x04, 0x20,
                                       0x08, 0x10,
                                       0x10, 0x08,
                                       0x20, 0x04,
                                       0x40, 0x02,
                                       0x80, 0x01,
                                       0x80, 0x01,
                                       0x40, 0x02,
                                       0x20, 0x04,
                                       0x10, 0x08,
                                       0x08, 0x10,
                                       0x04, 0x20,
                                       0x02, 0x40,
                                       0x01, 0x80,
                                   }));
                //composition.AddBitmap(
                //    name: "sad",
                //    bitmap: new Bitmap(8, 8, sad),
                //    offsetX: 0, offsetY: 4
                //);
                //composition.AddBitmap(
                //    name: "smile",
                //    bitmap: new Bitmap(8, 8, smile),
                //    offsetX: 4, offsetY: 8
                //);
                byte[] frame = null;
                const int delay = 0;
                for (var i = 0; i <= 8; i++) {
                    frame = composition.GetFrame(i, 0, true);
                    matrix.Set(frame);
                    Thread.Sleep(delay);
                }
                for (var i = 0; i <= 8; i++) {
                    frame = composition.GetFrame(8, i, true);
                    matrix.Set(frame);
                    Thread.Sleep(delay);
                }
                for (var i = 0; i <= 8; i++) {
                    frame = composition.GetFrame(8 - i, 8, true);
                    matrix.Set(frame);
                    Thread.Sleep(delay);
                }
                for (var i = 0; i <= 8; i++) {
                    frame = composition.GetFrame(0, 8 - i, true);
                    matrix.Set(frame);
                    Thread.Sleep(delay);
                }
                DisplayAndWait(frame, matrix, button);
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
