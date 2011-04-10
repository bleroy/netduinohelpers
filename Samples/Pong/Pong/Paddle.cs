using netduino.helpers.Imaging;

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
    public class Paddle {
        public int Size {
            get { return _pixels.Length; }
        }
        
        public int Y {
            get { return _pixels[0].Y; }
            set {
                for (var i = 0; i < Size; i++) {
                    _pixels[i].Y = value + i;
                }
            }
        }

        private readonly Side _side;
        private readonly GameOfPong _game;
        private readonly Composition _world;
        private readonly PlayerMissile _ball;
        private readonly PlayerMissile[] _pixels;

        public Paddle(Side side, GameOfPong game) {
            const int size = 3;
            _side = side;
            _game = game;
            _world = game.World;
            _ball = _world["ball"];
            _pixels = new PlayerMissile[size];
            for(var i = 0; i < size; i++) {
                _pixels[i] = new PlayerMissile(
                    "paddle" +
                    (_side == Side.Right ? 'R' : 'L') +
                    i,
                    _side == Side.Right ? 7 : 0,
                    i,
                    _world);
            }
            _world.Coinc +=
                (s, a) => {
                    if (_ball.X == 0 && _side == Side.Left) {
                        _game.BallGoingRight = true;
                    }
                    if (_ball.X == 7 && _side == Side.Right) {
                        _game.BallGoingRight = false;
                    }
                };
        }
    }

    public enum Side {
        Left,
        Right
    }
}
