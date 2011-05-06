using System;
using System.Threading;
using netduino.helpers.Imaging;

namespace netduino.helpers.Fun {
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
                Thread.Sleep(DisplayDelay);
            }
        }
    }
}
