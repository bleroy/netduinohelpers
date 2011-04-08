using System;
using System.Threading;
using netduino.helpers.Imaging;
using netduino.helpers.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace netduino.helpers.Fun {
    public abstract class Game : IDisposable {
        private Thread _thread;
        private readonly Random _random = new Random();
        private readonly Max72197221 _matrix;

        public Composition World { get; set; }
        public int DisplayDelay { get; set; }
        public Random Random { get { return _random; } }

        public abstract void Loop();

        protected Game() {
            _matrix = new Max72197221(Pins.GPIO_PIN_D8);
            _matrix.Shutdown(Max72197221.ShutdownRegister.NormalOperation);
            _matrix.SetDigitScanLimit(7);
            _matrix.SetIntensity(6);
            DisplayDelay = 80;
        }

        public virtual void Dispose() {
            _matrix.Dispose();
        }

        public Thread Run() {
            _thread = new Thread(LoopOverLoop);
            _thread.Start();
            return _thread;
        }

        public void Stop() {
            _thread.Abort();
        }

        private void LoopOverLoop() {
            while (true) {
                Loop();
                _matrix.Display(World.GetFrame(0, 0));
                Thread.Sleep(DisplayDelay);
            }
        }
    }
}
