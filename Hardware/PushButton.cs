using System;
using Microsoft.SPOT.Hardware;

namespace netduino.helpers.Hardware {
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
    /// This class implements an interrupt-driven push-button driver.
    /// It can be used in 2 modes:
    /// 1. derive your own class from PushButton and implement the OnButtonStateChange() method to get notified when the button is pushed.
    /// 2. don't derive from PushButton and provide a NativeEventHandler() instance instead.
    /// </summary>
    public class PushButton : IDisposable
    {
        private bool _state;

        /// <summary>
        /// The interrupt fires on a high edge when the button is pressed by default.
        /// If using a button other than the built-in one, you should connect pull-down resistors to the switch.
        /// Lady Ada has an excellent tutorial on this subject: http://www.ladyada.net/learn/arduino/lesson5.html
        /// </summary>
        /// <param name="pin">A digital pin connected to the actual push-button.</param>
        /// <param name="intMode">Defines the type of edge-change triggering the interrupt.</param>
        /// <param name="target">The event handler called when an interrupt occurs.</param>
        /// <param name="resistorMode">Internal pullup resistor configuration</param>
        /// <param name="glitchFilter">Input debouncing filter</param>
        public PushButton(
            Cpu.Pin pin,
            Port.InterruptMode intMode = Port.InterruptMode.InterruptEdgeBoth,
            NativeEventHandler target = null,
            Port.ResistorMode resistorMode = Port.ResistorMode.Disabled,
            bool glitchFilter = true
            ) {
                Input = new InterruptPort(pin, glitchFilter, resistorMode, intMode);

            if (target == null) {
                Input.OnInterrupt += InternalInterruptHandler;
            }
            else {
                Input.OnInterrupt += target;
            }

            Input.EnableInterrupt();
        }

        public InterruptPort Input { get; set; }

        public bool IsPressed {
            get { return _state; }
        }

        /// <summary>
        /// Internal interrupt handler used when no user-defined handler is provided in the constructor.
        /// Handles disabling / enabling the interrupt and calls the OnButtonStateChange() method.
        /// </summary>
        protected void InternalInterruptHandler(UInt32 port, UInt32 state, DateTime time) {
            Input.DisableInterrupt();
            _state = state != 0;
            OnButtonStateChange(port, state, time);
            Input.ClearInterrupt();
            Input.EnableInterrupt();
        }

        /// <summary>
        /// User-defined method used to receive notifications when deriving from PushButton.
        /// </summary>
        /// <param name="port">CPU port number that triggered the interrupt</param>
        /// <param name="state">The state of the edge that triggered the interrupt</param>
        /// <param name="time">Timestamp when the interrupt occured</param>
        protected virtual void OnButtonStateChange(UInt32 port, UInt32 state, DateTime time) {
        }

        public void Dispose() {
            Input.DisableInterrupt();
            Input.Dispose();
            Input = null;
        }
    }
}
