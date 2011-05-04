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

        public Meteor(GameOfMeteors game, int index, int x, int y) {
            var rnd = new Random();
            var dir = (float)rnd.NextDouble() * 2 * Trigo.Pi;
            var hSpeed = Trigo.Cos(dir) * MeteorSpeed;
            var vSpeed = Trigo.Sin(dir) * MeteorSpeed;
            var j = 0;
            var skip = rnd.Next(4);
            for (var i = 0; i < 4; i++) {
                if (i == skip) continue;
                _rockXOffsets[j] = _rockOffsets[i*2];
                _rockYOffsets[j] = _rockOffsets[i*2 + 1];
                _rocks[j] = new PlayerMissile {
                                                  Name = "Meteor" + index + ":" + j,
                                                  X = x + _rockXOffsets[j],
                                                  Y = y + _rockYOffsets[j],
                                                  HorizontalSpeed = hSpeed,
                                                  VerticalSpeed = vSpeed,
                                                  Owner = game.World
                                              };
                j++;
            }
        }

        public void Move() {
            for(var i = 0; i < _rocks.Length; i++) {
                _rocks[i].Move();
                if (_rocks[i].X < 0) _rocks[i].X += GameOfMeteors.WorldSize;
                if (_rocks[i].Y < 0) _rocks[i].Y += GameOfMeteors.WorldSize;
                if (_rocks[i].X >= GameOfMeteors.WorldSize) _rocks[i].X -= GameOfMeteors.WorldSize;
                if (_rocks[i].Y >= GameOfMeteors.WorldSize) _rocks[i].Y -= GameOfMeteors.WorldSize;
            }
        }
    }
}
