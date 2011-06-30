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
        
        public bool IsLeftButtonClicked { get; set; }
        public bool IsRightButtonClicked { get; set; }

        public abstract void Loop();

        protected Game(ConsoleHardwareConfig hardwareConfig) {
            Hardware = hardwareConfig;           
            DisplayDelay = 80;
        }

        protected bool ScrollStopFunction() {
            if (IsLeftButtonClicked || IsRightButtonClicked) {
                return true;
            }
            return false;
        }

        protected void ResetButtonClicks() {
            IsLeftButtonClicked = false;
            IsRightButtonClicked = false;
        }

        protected virtual void OnLeftButtonClick(UInt32 port, UInt32 state, DateTime time) {
            IsLeftButtonClicked = true;
        }

        protected virtual void OnRightButtonClick(UInt32 port, UInt32 state, DateTime time) {
            IsRightButtonClicked = true;
        }

        public virtual void Dispose() {
            Stop();
        }

        public Thread Run() {
            IsSpinning = true; 
            _thread = new Thread(LoopOverLoop);
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

        public delegate bool ScrollStop();

        public void ScrollMessage(string message, int delayMS, ScrollStop stopFunction) {
            ResetButtonClicks();

            var charSet = new CharSet();
            var splashScreen = charSet.StringToBitmap(message);
            var exit = false;

            while (!exit) {
                var x = 0;
                for (; x < splashScreen.Width; x++) {
                    Hardware.Matrix.Display(splashScreen.GetFrame(x, 0));
                    exit = stopFunction();
                    if (exit) {
                        break;
                    }
                    Thread.Sleep(delayMS);
                }
                for (; x != 0; x--) {
                    exit = stopFunction();
                    if (exit) {
                        break;
                    }
                    Hardware.Matrix.Display(splashScreen.GetFrame(x, 0));
                    Thread.Sleep(delayMS);
                }
            }
        }

        protected virtual void OnGameStart() {
        }

        protected virtual void OnGameOver() {
        }

        private void LoopOverLoop() {
            Hardware.LeftButton.Input.DisableInterrupt();
            Hardware.RightButton.Input.DisableInterrupt();

            Hardware.LeftButton.Input.OnInterrupt += OnLeftButtonClick;
            Hardware.RightButton.Input.OnInterrupt += OnRightButtonClick;

            Hardware.LeftButton.Input.EnableInterrupt();
            Hardware.RightButton.Input.EnableInterrupt();

            OnGameStart();

            while (IsSpinning) {
                Loop();
                Thread.Sleep(DisplayDelay);
            }

            OnGameOver();

            Hardware.LeftButton.Input.DisableInterrupt();
            Hardware.RightButton.Input.DisableInterrupt();
            Hardware.LeftButton.Input.OnInterrupt -= OnLeftButtonClick;
            Hardware.RightButton.Input.OnInterrupt -= OnRightButtonClick;
        }
    }
}
