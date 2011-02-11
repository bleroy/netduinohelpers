using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;

namespace netduino.helpers.Servo
{
    /*
    Copyright (C) 2011 by Fabien Royer

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
    */

    /// <summary>
    /// Driver for the HiTec HS6635HB servo.
    /// http://www.hitecrcd.com/products/digital/digital-sport/hs-6635hb.html
    /// </summary>
    public class HS6635HBServo : IDisposable {
        private PWM _servo;

        public uint MinRangePulse { get; set; }
        public uint CenterRangePulse { get; set; }
        public uint MaxRangePulse { get; set; }
        public uint PulseRefreshRateMs { get; set; }

        public HS6635HBServo(Cpu.Pin pwmPin, uint minPulse = 900, uint centerPulse = 1500, uint maxPulse = 2100) {
            _servo = new PWM((Cpu.Pin)pwmPin);
            _servo.SetDutyCycle(0);
            MinRangePulse = minPulse;
            CenterRangePulse = centerPulse;
            MaxRangePulse = maxPulse;
            PulseRefreshRateMs = 20;
        }

        // Slowly moves the servo from a position to another
        public void Move(uint startDegree, uint endDegree, int delay = 80) {
            if (delay <= 1) {
                delay = 10;
            }

            if (startDegree < endDegree) {
                for (var degree = startDegree; degree <= endDegree; degree++) {
                    Degree = degree;
                    Thread.Sleep(delay);
                }
            } else {
                for (var degree = startDegree; degree > endDegree; degree--) {
                    Degree = degree;
                    Thread.Sleep(delay);
                }
            }

            Release();
        }

        // Positions the servo to its center position
        public void Center() {
            Pulse = CenterRangePulse;
        }

        /// <summary>
        /// Positions the servo from 0 to 180 degrees
        /// </summary>
        public uint Degree {
            set {
                if (value > 180) {
                    value = 180;
                } else {
                    if (value < 0) {
                        value = 0;
                    }
                }
                var pulse = (uint) MapRange(
                    (double)0, 
                    (double)180,
                    (double)MinRangePulse,
                    (double)MaxRangePulse,
                    (double)value
                    );
                
                Debug.Print("Degree: " + value.ToString() + " = " + pulse.ToString());
                
                Pulse = pulse;
            }
        }

        // Positions the servo using an absolute pulse
        public uint Pulse {
            set {
                if (value > MaxRangePulse) {
                    value = MaxRangePulse;
                } else {
                    if (value < MinRangePulse) {
                        value = MinRangePulse;
                    }
                }
                _servo.SetPulse(PulseRefreshRateMs * 1000, value);
            }
        }

        // Releases the servo from tracking its position
        public void Release() {
            _servo.SetDutyCycle(0);
        }

        /// <summary>
        /// Maps a range of values to another
        /// http://rosettacode.org/wiki/Map_range#C
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private double MapRange(double a1, double a2, double b1, double b2, double s) {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        // Disposes of the servo resources
        public void Dispose() {
            Release();
            _servo.Dispose();
            _servo = null;
        }
    }
}
