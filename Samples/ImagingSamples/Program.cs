using System.Threading;
using Microsoft.SPOT;
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
                //var sad = new byte[] {0x66, 0x24, 0x00, 0x18, 0x00, 0x3C, 0x42, 0x81};
                //DisplayAndWait(sad, matrix, button);
                //// Let's make it smile!
                //var smile = new byte[] {0x42, 0x18, 0x18, 0x81, 0x7E, 0x3C, 0x18, 0x00};
                //DisplayAndWait(smile, matrix, button);
                var comp = new Composition(new byte[]  {
                    0x00, 0x00,
                    0x00, 0x00,
                    0x00, 0x00,
                    0x00, 0x00,
                    0x03, 0xC0,
                    0x07, 0xE0,
                    0x0F, 0xF0,
                    0x0F, 0xF0,
                    0x0F, 0xF0,
                    0x0F, 0xF0,
                    0x07, 0xE0,
                    0x03, 0xC0,
                    0x00, 0x00,
                    0x00, 0x00,
                    0x00, 0x00,
                    0x00, 0x00,
                }, 16, 16);
                var player = new PlayerMissile("player", 0, 0);
                var missile = new PlayerMissile("missile", 0, 0);
                comp.AddMissile(player);
                comp.AddMissile(missile);
                while (true) {
                    for (var angle = 0; angle < 360; angle++) {
                        player.X = 8 + Math.Sin(angle * 2)/160;
                        player.Y = 8 + Math.Cos(angle * 2)/160;
                        missile.X = 8 + Math.Sin(angle) / 160;
                        missile.Y = 8 + Math.Cos(angle) / 160;
                        var frame = comp.GetFrame(Math.Sin(angle*20)/250 + 4, Math.Cos(angle*20)/250 + 4);
                        matrix.Set(frame);
                        Thread.Sleep(50);
                    }
                }
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
