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
                // TODO: All together now
                var comp = new Composition(new byte[]  {
                                                     0, 1, 2, 4, 4, 2, 1, 0,
                                                     1, 2, 4, 8, 8, 4, 2, 1,
                                                     2, 4, 8, 16, 16, 8, 4, 2,
                                                     4, 8, 16, 32, 32, 16, 8, 4,
                                                     4, 8, 16, 32, 32, 16, 8, 4,
                                                     2, 4, 8, 16, 16, 8, 4, 2,
                                                     1, 2, 4, 8, 8, 4, 2, 1,
                                                     0, 1, 2, 4, 4, 2, 1, 0,
                                                 }, 8, 8);
                var player = new PlayerMissile("player", 0, 0);
                var missile = new PlayerMissile("missile", 7, 7);
                comp.AddMissile(player);
                comp.AddMissile(missile);
                while (true) {
                    for (var size = 0; size < 32; size++) {
                        player.X = size/4;
                        player.Y = size/8;
                        missile.X = 7 - size/8;
                        missile.Y = 7 - size/4;
                        var frame = comp.GetFrame((Math.Sin(size*20))/500, (Math.Cos(size*20))/500);
                        var cutoff = new byte[8];
                        for (var i = 0; i < 8; i++) {
                            for (var j = 0; j < 8; j++) {
                                if (frame[i*8 + j] > size) {
                                    cutoff[j] |= (byte) (1 << i);
                                }
                            }
                        }
                        matrix.Set(cutoff);
                        Thread.Sleep(10);
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
