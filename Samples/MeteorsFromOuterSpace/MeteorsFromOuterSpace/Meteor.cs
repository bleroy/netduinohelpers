using System;
using netduino.helpers.Imaging;
using netduino.helpers.Math;

namespace MeteorsFromOuterSpace {
    public class Meteor {
        public const float MeteorSpeed = 0.1f;

        private readonly byte[] _rockOffsets =
            new byte[] {
                           0, 0,
                           1, 0,
                           1, 1,
                           0, 1
                       };
        private readonly PlayerMissile[] _rocks = new PlayerMissile[3];
        private readonly byte[] _rockXOffsets = new byte[3];
        private readonly byte[] _rockYOffsets = new byte[3];
        private readonly float[] _horizontalSpeed = new float[3];
        private readonly float[] _verticalSpeed = new float[3];
        private readonly float[] _x = new float[3];
        private readonly float[] _y = new float[3];

        public Meteor(GameOfMeteors game, int index, int x, int y) {
            var rnd = new Random();
            var dir = (float)rnd.NextDouble() * 2 * Trigo.Pi;
            var hSpeed = Trigo.Cos(dir) * MeteorSpeed;
            var vSpeed = Trigo.Sin(dir) * MeteorSpeed;
            var j = 0;
            var skip = rnd.Next(4);
            for (var i = 0; i < 4; i++) {
                if (i == skip) continue;
                _horizontalSpeed[j] = hSpeed;
                _verticalSpeed[j] = vSpeed;
                _rockXOffsets[j] = _rockOffsets[i*2];
                _rockYOffsets[j] = _rockOffsets[i*2 + 1];
                _x[j] = x + _rockXOffsets[j];
                _y[j] = y + _rockYOffsets[j];
                _rocks[j] = new PlayerMissile(
                    "Meteor" + index + ":" + j,
                    (int)_x[j],
                    (int)_y[j],
                    game.World);
                j++;
            }
        }

        public void Move() {
            for(var i = 0; i < _rocks.Length; i++) {
                _x[i] += _horizontalSpeed[i];
                _y[i] += _verticalSpeed[i];
                if (_x[i] < 0) _x[i] += GameOfMeteors.WorldSize;
                if (_y[i] < 0) _y[i] += GameOfMeteors.WorldSize;
                if (_x[i] >= GameOfMeteors.WorldSize) _x[i] -= GameOfMeteors.WorldSize;
                if (_y[i] >= GameOfMeteors.WorldSize) _y[i] -= GameOfMeteors.WorldSize;
                _rocks[i].X = (int)_x[i];
                _rocks[i].Y = (int)_y[i];
            }
        }
    }
}
