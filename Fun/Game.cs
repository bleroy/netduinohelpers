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

            Hardware.LeftButton.Input.DisableInterrupt();
            Hardware.RightButton.Input.DisableInterrupt();
            Hardware.LeftButton.Input.OnInterrupt += OnLeftButtonClick;
            Hardware.RightButton.Input.OnInterrupt += OnRightButtonClick;
            Hardware.LeftButton.Input.EnableInterrupt();
            Hardware.RightButton.Input.EnableInterrupt(); 
            
            DisplayDelay = 80;
        }

        protected virtual void OnLeftButtonClick(UInt32 port, UInt32 state, DateTime time) {
        }

        protected virtual void OnRightButtonClick(UInt32 port, UInt32 state, DateTime time) {
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

        public void Beep(uint frequency, int delayMS) {
            var period = 1000000 / frequency;
            Hardware.Speaker.SetPulse(period, period / 2);
            Thread.Sleep(delayMS);
            Hardware.Speaker.SetPulse(0, 0);
        }

        private void LoopOverLoop() {
            while (IsSpinning) {
                Loop();
                Thread.Sleep(DisplayDelay);
            }
        }
    }
}
