using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using netduino.helpers.Hardware;

namespace SharpDistanceSensorTest {
    public class Program {
        public static SharpGP2Y0A21YK0F sensor = new SharpGP2Y0A21YK0F(Pins.GPIO_PIN_A0);
        public static void Main() {
            sensor.Start(1.0f, VoltageTrigger);

            uint seconds = 60;

            Debug.Print("Started monitoring distance");

            while (--seconds > 0) {
                Thread.Sleep(1000);
            }

            sensor.Stop();

            Debug.Print("Finished monitoring distance");
        }

        public static void VoltageTrigger(int averagedDistance, float mappedVoltage) {
            Debug.Print("Avg. dist: " + averagedDistance.ToString() + ", voltage: " + mappedVoltage.ToString());
        }
    }
}
