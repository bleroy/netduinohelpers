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
            tft.FillScreen((ushort)AdaFruitST7735.Colors.Red);
        }
    }
}
