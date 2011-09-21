using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;

namespace netduino.helpers.Hardware {
    public delegate void VoltageTriggerCallback(int averagedDistance, float mappedVoltage);

    // Datasheet: http://sharp-world.com/products/device/lineup/data/pdf/datasheet/gp2y0a21yk_e.pdf
    public class SharpGP2Y0A21YK0F : IDisposable {
        
        protected AnalogInput DistanceSensor;
        protected VoltageTriggerCallback VoltageTriggerProcedure;
        protected Thread DistanceSampler;
        protected bool DistanceSamplerStop;

        public float VoltageThreshold { get; set; }
        public int AverageMeasurementCount { get; set; }

        public SharpGP2Y0A21YK0F(Cpu.Pin analogPin) {
            DistanceSensor = new AnalogInput(analogPin);
            DistanceSensor.SetRange(70, 970);
            VoltageThreshold = 0;
            AverageMeasurementCount = 10;
        }

        public void Start(float voltageThreshold, VoltageTriggerCallback callback) {
            if (DistanceSampler == null) {
                VoltageThreshold = voltageThreshold;
                VoltageTriggerProcedure = callback;
                DistanceSamplerStop = false;
                DistanceSampler = new Thread(DistanceSampling);
                DistanceSampler.Start();
            }
        }

        public void Stop() {
            if (DistanceSampler != null) {
                DistanceSamplerStop = true;
                DistanceSampler.Join();
                DistanceSampler = null;
            }
        }

        protected void DistanceSampling() {
            while (!DistanceSamplerStop) {
                var averagedDistance = ReadAverageDistance();
                var mappedVoltage = MapRange(970f, 70f, 3.3f, 0.4f, averagedDistance);
                if (mappedVoltage >= VoltageThreshold) VoltageTriggerProcedure(averagedDistance, mappedVoltage);
            }
        }

        public int ReadAverageDistance() {
            var count = AverageMeasurementCount;
            var total = 0;
            while (--count >= 0) {
                total += DistanceSensor.Read();
            }
            return total / AverageMeasurementCount;
        }

        // Maps a range of values to another http://rosettacode.org/wiki/Map_range#C
        public float MapRange(float a1, float a2, float b1, float b2, float s) {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        public void Dispose() {
            Stop();
            DistanceSensor = null;
            VoltageTriggerProcedure = null;
        }
    }
}
