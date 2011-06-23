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
        public bool IsSpinning { get; set; }

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
            IsSpinning = true;
            _thread.Start();
            return _thread;
        }

        public void Stop() {
            IsSpinning = false;
        }

        private void LoopOverLoop() {
            while (IsSpinning) {
                Loop();
                Thread.Sleep(DisplayDelay);
            }
        }
    }
}
