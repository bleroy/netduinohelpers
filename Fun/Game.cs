using System;
using System.Threading;
using netduino.helpers.Imaging;
using netduino.helpers.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace netduino.helpers.Fun {
    /*
    Copyright (C) 2011 by Bertrand Le Roy & Fabien Royer

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
    public abstract class Game : IDisposable {
        private Thread _thread;
        private readonly Random _random = new Random();

        public Composition World { get; set; }
        public int DisplayDelay { get; set; }
        public Random Random { get { return _random; } }
        public ConsoleHardwareConfig Hardware { get; private set; }

        public abstract void Loop();

        protected Game(ConsoleHardwareConfig hardwareConfig) {
            Hardware = hardwareConfig;
            DisplayDelay = 80;
        }

        public virtual void Dispose() {
            Stop();
        }

        public Thread Run() {
            _thread = new Thread(LoopOverLoop);
            _thread.Start();
            return _thread;
        }

        public void Stop() {
            _thread.Abort();
            _thread = null;
        }

        private void LoopOverLoop() {
            while (true) {
                Loop();
                Hardware.Matrix.Display(World.GetFrame(0, 0));
                Thread.Sleep(DisplayDelay);
            }
        }
    }
}
