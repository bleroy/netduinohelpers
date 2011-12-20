using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using netduino.helpers.Hardware;

namespace AdaFruitILI932xTftTest {
    public class Program {
        public static ShiftRegister74HC595 shiftRegister = new ShiftRegister74HC595(Pins.GPIO_PIN_D9);
        public static AdaFruitILI932x tft = new AdaFruitILI932x(
                                    shiftRegister,
                                    tftChipSelect: Pins.GPIO_PIN_D8,
                                    tftCommandData: Pins.GPIO_PIN_D7,
                                    tftWrite: Pins.GPIO_PIN_D6,
                                    tftRead: Pins.GPIO_NONE,
                                    tftReset: Pins.GPIO_PIN_D5);
        public static void Main() {
            tft.Initialize();
            tft.DrawPixel(160, 120, 0xF800);
            tft.DrawPixel(161, 120, 0xF800);
            tft.DrawPixel(162, 120, 0xF800);
            tft.DrawPixel(163, 120, 0xF800);
            tft.DrawPixel(164, 120, 0xF800);
        }
    }
}
