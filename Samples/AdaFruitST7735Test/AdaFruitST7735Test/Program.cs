using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using netduino.helpers.Hardware;

namespace AdaFruitST7735Test {
    public class Program {
        public static void Main() {
            var tft = new AdaFruitST7735(Pins.GPIO_PIN_D9, Pins.GPIO_PIN_D7, Pins.GPIO_PIN_D8);

            Debug.EnableGCMessages(true);
            Debug.GC(true);

            tft.ClearScreen();
            tft.Refresh();

            tft.DrawLine(0, 0, AdaFruitST7735.Width, 0, (ushort)AdaFruitST7735.Colors.White);
            tft.DrawLine(0, 0, 0, AdaFruitST7735.Height, (ushort)AdaFruitST7735.Colors.White);
            tft.DrawLine(0, AdaFruitST7735.Height - 1, AdaFruitST7735.Width - 1, AdaFruitST7735.Height - 1, (ushort)AdaFruitST7735.Colors.White);
            tft.DrawLine(AdaFruitST7735.Width - 1, 0, AdaFruitST7735.Width - 1, AdaFruitST7735.Height - 1, (ushort)AdaFruitST7735.Colors.White);

            tft.Refresh();

            var x = AdaFruitST7735.Width / 2;
            var y = AdaFruitST7735.Height / 2;

            tft.DrawPixel(x, y, (ushort)AdaFruitST7735.Colors.White);
            tft.Refresh();

            for (var r = 1; r < x; r += 3) {
                tft.DrawCircle(x, y, r, (ushort)AdaFruitST7735.Colors.White);
                tft.Refresh();
            }


            Debug.GC(true);
        }
    }
}
