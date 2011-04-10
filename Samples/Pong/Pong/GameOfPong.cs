using netduino.helpers.Fun;
using netduino.helpers.Imaging;
using netduino.helpers.Sound;
using System.Threading;

namespace Pong {
    /*
    Copyright (C) 2011 by Fabien Royer & Bertrand Le Roy

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
    public class GameOfPong : Game {
        bool _ballGoingDown;

        public int LeftScore;
        public int RightScore;

        public bool BallGoingRight { get; set; }

        public PlayerMissile Ball { get; private set; }
        public Paddle LeftPaddle { get; private set; }
        public Paddle RightPaddle { get; private set; }

        public GameOfPong(ConsoleHardwareConfig config) : base(config) {
            World = new Composition(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 8, 8);
            Ball = new PlayerMissile("ball", 0, 0, World);
            LeftPaddle = new Paddle(Side.Left, this);
            RightPaddle = new Paddle(Side.Right, this);
            BallGoingRight = true;
            ResetBall();
        }

        public override void Loop() {
            LeftPaddle.Y = Hardware.JoystickLeft.Y / 128;
            RightPaddle.Y = Hardware.JoystickRight.Y / 128;

            Ball.X += BallGoingRight ? 1 : -1;
            if (Ball.X < 0) {
                RightScore++;
                ResetBall();
            }
            if (Ball.X >= 8) {
                LeftScore++;
                ResetBall();
            }
            Ball.Y += _ballGoingDown ? 1 : -1;
            if (Ball.Y < 0) {
                Ball.Y = 1;
                _ballGoingDown = true;
                Beep(10000);
            }
            if (Ball.Y >= 8) {
                Ball.Y = 7;
                _ballGoingDown = false;
                Beep(10000);
            }
         }

        public void ResetBall() {
            Ball.X = 0;
            Ball.Y = Random.Next(8);
            BallGoingRight = true;
            _ballGoingDown = Random.Next(2) == 0;
            Beep(3000);
        }

        public void Beep(uint frequency) {
            var period = (uint)(1000000 / frequency); 
            Hardware.Speaker.SetPulse(period, period / 2);
            Thread.Sleep(50);
            Hardware.Speaker.SetPulse(0, 0);
        }
    }
}
