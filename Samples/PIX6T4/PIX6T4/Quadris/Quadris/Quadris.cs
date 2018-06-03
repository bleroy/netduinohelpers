using System.Threading;
using netduino.helpers.Fun;
using netduino.helpers.Hardware;
using netduino.helpers.Imaging;
using System;

namespace Quadris
{
    public class Quadris : Game
    {
        public const int FieldWidth = 8;
        public const int FieldHeight = 8;
        public const long TetrominoFallInterval = 500 * TimeSpan.TicksPerMillisecond;
        public const float AccelarationRate = 101; // Percent
        private static readonly bool[] _emptyRow = new bool[FieldWidth];

        private Timer _timeToFallTimer;
        private long _currentTimeToFallTicks = TetrominoFallInterval;
        private AnalogJoystick.Direction _previousLeftDirection = AnalogJoystick.Direction.Center;
        private AnalogJoystick.Direction _previousRightDirection = AnalogJoystick.Direction.Center;
        private bool _rightJoystickPreviouslyDown = false;

        public Quadris(ConsoleHardwareConfig config)
            : base(config)
        {
            World = new Composition(new byte[FieldHeight * FieldWidth / 8], FieldWidth, FieldHeight);
            ResetPlayfield();
            Tetromino = new Tetromino(this);
            RenderFrame();
            TimeToFall = false;
            _timeToFallTimer = new Timer(OnTimerTick, null, TimeSpan.Zero, TimeSpan.FromTicks(TetrominoFallInterval));
            DisplayDelay = 0;
        }

        public Tetromino Tetromino { get; private set; }
        public bool[][] Playfield { get; private set; }
        public bool TimeToFall { get; private set; }

        public void ResetPlayfield()
        {
            // It's micro-framework, which means more work to init multi-dimensional arrays
            Playfield = new bool[FieldHeight][];
            for (var r = 0; r < FieldHeight; r++) {
                Playfield[r] = new bool[FieldWidth];
            }
        }

        public void RenderFrame()
        {
            Bitmap field = World.Background;
            // Copy the playfield
            for (var r = 0; r < FieldHeight; r++)
            {
                for (var c = 0; c < FieldWidth; c++)
                {
                    field.SetPixel(c, r, Playfield[r][c]);
                }
            }
            // Copy the tetromino
            var pixels = Tetromino.Pixels;
            for (var r = 0; r < pixels.Length; r++)
            {
                var row = Tetromino.Y + r;
                if (row < 0) continue;
                for (var c = 0; c < pixels[r].Length; c++)
                {
                    var column = Tetromino.X + c;
                    if (!pixels[r][c]) continue;
                    field.SetPixel(column, row, true);
                }
            }
            Hardware.Matrix.Display(World.Background.GetFrame(0, 0));
        }

        public void MakeExplosionSound()
        {
            Beep(3000, 10);
            Beep(1000, 10);
        }

        public void MakeHitSound()
        {
            Beep(3000, 10);
        }

        protected override void OnGameStart()
        {
            ScrollMessage(" Quadris!");
        }

        protected override void OnGameEnd()
        {
            _timeToFallTimer.Dispose();
            _timeToFallTimer = null;
            ScrollMessage(" Game over!");
        }

        public void OnTimerTick(object state)
        {
            TimeToFall = true;
        }

        public override void Loop()
        {
            var leftStick = Hardware.JoystickLeft.XDirection;
            if (leftStick != _previousLeftDirection)
            {
                switch (leftStick)
                {
                    case AnalogJoystick.Direction.Negative:
                        Tetromino.MoveLeft();
                        break;
                    case AnalogJoystick.Direction.Positive:
                        Tetromino.MoveRight();
                        break;
                }
                _previousLeftDirection = leftStick;
            }
            var rightStick = Hardware.JoystickRight.XDirection;
            if (rightStick != _previousRightDirection)
            {
                switch (rightStick)
                {
                    case AnalogJoystick.Direction.Negative:
                        Tetromino.RotateLeft();
                        break;
                    case AnalogJoystick.Direction.Positive:
                        Tetromino.RotateRight();
                        break;
                }
                _previousRightDirection = rightStick;
            }
            var stickDown = Hardware.JoystickLeft.YDirection == AnalogJoystick.Direction.Positive;
            if (TimeToFall || (stickDown && !_rightJoystickPreviouslyDown))
            {
                TimeToFall = false;
                if (!Tetromino.MoveDown())
                {
                    // The tetromino hit the bottom
                    // We need to copy the tetromino onto the playfield
                    CopyTetrominoOntoPlayfield();
                    // Did it have enough space to render entirely?
                    if (Tetromino.Y < 0)
                    {
                        // Nope -> Game over
                        RenderFrame();
                        MakeExplosionSound();
                        Stop();
                    }
                    else
                    {
                        MakeHitSound();
                        // Check for and remove full lines
                        RemoveFullLines();
                        // Spawn a new tetromino
                        Tetromino = new Tetromino(this);
                        // Accelerate the game
                        _currentTimeToFallTicks = (long)(_currentTimeToFallTicks * 100 / AccelarationRate);
                        _timeToFallTimer.Change(TimeSpan.Zero, TimeSpan.FromTicks(_currentTimeToFallTicks));
                    }
                }
            }
            _rightJoystickPreviouslyDown = stickDown;
            RenderFrame();
        }

        private void CopyTetrominoOntoPlayfield()
        {
            var pixels = Tetromino.Pixels;
            for (var r = 0; r < pixels.Length; r++)
            {
                var row = Tetromino.Y + r;
                if (row < 0) continue;
                for (var c = 0; c < pixels[r].Length; c++)
                {
                    if (!pixels[r][c]) continue;
                    Playfield[row][Tetromino.X + c] = true;
                }
            }
        }

        private bool IsRowFull(bool[] pixelRow)
        {
            for (var c = 0; c < pixelRow.Length; c++)
            {
                if (!pixelRow[c]) return false;
            }
            return true;
        }

        private void RemoveFullLines()
        {
            int offset = 0;
            for (var r = FieldHeight - 1; r >= -1; r--)
            {
                bool[] pixelRow = r >= 0 ? Playfield[r] : _emptyRow;
                if (IsRowFull(pixelRow))
                {
                    offset++;
                    continue;
                }
                if (offset > 0) {
                    for (var c = 0; c < FieldWidth; c++)
                    {
                        Playfield[r + offset][c] = pixelRow[c];
                    }
                }
            }
        }
    }
}
