using System;
using System.Collections;
using netduino.helpers.Fun;
using netduino.helpers.Imaging;

namespace MeteorsFromOuterSpace {
    public class GameOfMeteors : Game {
        private const int WorldSize = 24;
        private const int NumberOfMeteors = 10;
        private const float EnginePower = 0.001f;

        public ArrayList Meteors { get; private set; }
        public ArrayList Figs { get; private set; }
        public PlayerMissile Ship { get; private set; }
        public float HorizontalSpeed { get; set; }
        public float VerticalSpeed { get; set; }
        public float ShipX { get; set; }
        public float ShipY { get; set; }

        public GameOfMeteors(ConsoleHardwareConfig config)
            : base(config) {
            Meteors = new ArrayList();
            Figs = new ArrayList();
            World = new Composition(new byte[72], 24, 24);
            Ship = new PlayerMissile("ship", 12, 12, World);
        }

        public override void Loop() {
            HorizontalSpeed += (Hardware.JoystickLeft.X - 512) * EnginePower;
            VerticalSpeed += (Hardware.JoystickLeft.Y - 512) * EnginePower;

            ShipX += HorizontalSpeed;
            ShipY += VerticalSpeed;

            Ship.X = (int)ShipX;
            Ship.Y = (int)ShipY;

            Hardware.Matrix.Display(World.GetFrame(Ship.X - 3, Ship.Y - 3));
        }
    }
}
