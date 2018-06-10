using netduino.helpers.Imaging;
using netduino.helpers.Math;
using System;

namespace Quadris
{
    public class Tetromino
    {
        private bool[][][] _shapes;

        public const float TetrominoSpeed = 0.1f;

        public Tetromino(Quadris game, TetrominoShape shape)
        {
            Owner = game;
            Shape = shape;
            _shapes = Tetrominos[(int)shape];
            var rnd = new Random();
            Orientation = (Orientation)rnd.Next(4);
            bool hasPixelsOnFirstColumn = HasPixelsOnFirstColumn();
            int width = Pixels[0].Length - (hasPixelsOnFirstColumn ? 0 : 1);
            X = rnd.Next(Quadris.FieldWidth - width) - (hasPixelsOnFirstColumn ? 0 : 1);
            Y = -Pixels.Length;
        }

        public Tetromino(Quadris game): this(game, (TetrominoShape)new Random().Next(7))
        {
        }

        public Quadris Owner { get; private set; }
        public TetrominoShape Shape { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Orientation Orientation { get; private set; }

        public bool[][] Pixels
        {
            get { return _shapes[(int)Orientation]; }
        }

        public bool CheckMoveIsValid(int x, int y, Orientation orientation)
        {
            var pixels = _shapes[(int)orientation];
            for (var r = 0; r < pixels.Length; r++)
            {
                var row = y + r;
                for (var c = 0; c < pixels[0].Length; c++)
                {
                    // No pixel, no trouble
                    if (!pixels[r][c]) continue;
                    // A pixel below the bottom edge of the world isn't valid
                    if (row >= Quadris.FieldHeight) return false;
                    var column = x + c;
                    // Pixels left of the left edge, right of the right edge, or hitting a playfield pixel are invalid
                    if (column < 0 || column >= Quadris.FieldWidth || (row > 0&& Owner.Playfield[row][column])) return false;
                }
            }
            return true;
        }

        public bool MoveDown()
        {
            var height = Pixels.Length;

            // If the tetromino is on the bottom, stop
            if (Y + height >= Quadris.FieldHeight) return false;

            // Test the pixel one row down
            if (!CheckMoveIsValid(X, Y + 1, Orientation)) return false;

            // It's fine, we can move down
            Y++;
            return true;
        }

        public bool MoveLeft()
        {
            if (!CheckMoveIsValid(X - 1, Y, Orientation)) return false;
            X--;
            return true;
        }

        public bool MoveRight()
        {
            if (!CheckMoveIsValid(X + 1, Y, Orientation)) return false;
            X++;
            return true;
        }

        public bool RotateLeft()
        {
            var leftOrientation = (Orientation)(((int)Orientation + 1) % 4);
            if (!CheckMoveIsValid(X, Y, leftOrientation)) return false;
            Orientation = leftOrientation;
            return true;
        }

        public bool RotateRight()
        {
            var rightOrientation = (Orientation)(((int)Orientation + 3) % 4);
            if (!CheckMoveIsValid(X, Y, rightOrientation)) return false;
            Orientation = rightOrientation;
            return true;
        }

        private bool HasPixelsOnFirstColumn()
        {
            for (var r = 0; r < Pixels.Length; r++) {
                if (Pixels[r][0]) return true;
            }
            return false;
        }

        private const bool O = true;
        private const bool _ = false;

        // Representations of the tetraminoes is offset so rotations appear more natural, with a center
        // close to the center of the piece.
     public static readonly bool[][][] Shapes = new[] {
            /* I */
            new[] {
                new[] {_, O},
                new[] {_, O},
                new[] {_, O},
                new[] {_, O}
            },
            new[] {
                new[] {_, _, _, _},
                new[] {O, O, O, O}
            },
            /* L */
            new[] {
                new[] {_, O, _},
                new[] {_, O, _},
                new[] {_, O, O}
            },
            new[] {
                new[] {_, _, O},
                new[] {O, O, O}
            },
            new[] {
                new[] {O, O},
                new[] {_, O},
                new[] {_, O}
            },
            new[] {
                new[] {_, _, _},
                new[] {O, O, O},
                new[] {O, _, _}
            },
            /* J */
            new[] {
                new[] {_, O},
                new[] {_, O},
                new[] {O, O}
            },
            new[] {
                new[] {_, _, _},
                new[] {O, O, O},
                new[] {_, _, O}
            },
            new[] {
                new[] {_, O, O},
                new[] {_, O, _},
                new[] {_, O, _}
            },
            new[] {
                new[] {O, _, _},
                new[] {O, O, O}
            },
            /* T */
            new[] {
                new[] {_, O, _},
                new[] {_, O, O},
                new[] {_, O, _}
            },
            new[] {
                new[] {_, O, _},
                new[] {O, O, O}
            },
            new[] {
                new[] {_, O},
                new[] {O, O},
                new[] {_, O}
            },
            new[] {
                new[] {_, _, _},
                new[] {O, O, O},
                new[] {_, O, _}
            },
            /* O */
            new[] {
                new[] {O, O},
                new[] {O, O}
            },
            /* S */
            new[] {
                new[] {_, O, _},
                new[] {_, O, O},
                new[] {_, _, O}
            },
            new[] {
                new[] {_, O, O},
                new[] {O, O, _},
            },
            /* Z */
            new[] {
                new[] {_, _, O},
                new[] {_, O, O},
                new[] {_, O, _}
            },
            new[] {
                new[] {O, O, _},
                new[] {_, O, O}
            }
        };

        private static readonly bool[][][][] Tetrominos = new[] {
            /* I */ new[] {Shapes[0], Shapes[1], Shapes[0], Shapes[1]},
            /* O */ new[] {Shapes[14], Shapes[14], Shapes[14], Shapes[14]},
            /* T */ new[] {Shapes[10], Shapes[11], Shapes[12], Shapes[13]},
            /* S */ new[] {Shapes[15], Shapes[16], Shapes[15], Shapes[16]},
            /* Z */ new[] {Shapes[17], Shapes[18], Shapes[17], Shapes[18]},
            /* J */ new[] {Shapes[6], Shapes[7], Shapes[8], Shapes[9]},
            /* L */ new[] {Shapes[2], Shapes[3], Shapes[4], Shapes[5]}
        };
    }

    public enum TetrominoShape
    {
        I = 0, O, T, S, Z, J, L
    }

    public enum Orientation
    {
        Zero, Ninety, OneEighty, TwoSeventy
    }
}
