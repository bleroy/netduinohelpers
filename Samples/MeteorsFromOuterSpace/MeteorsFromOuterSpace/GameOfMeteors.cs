using System.Collections;
using netduino.helpers.Fun;
using netduino.helpers.Hardware;
using netduino.helpers.Imaging;

namespace MeteorsFromOuterSpace {
    public class GameOfMeteors : Game {
        public const int WorldSize = 8;
        public const int NumberOfMeteors = 3;
        private const float EnginePower = 0.3f;

        public Meteor[] Meteors { get; private set; }
        public PlayerMissile Pruneau { get; private set; }
        public PlayerMissile Ship { get; private set; }
        public float ShipX { get; set; }
        public float ShipY { get; set; }
        public float PruneauX { get; set; }
        public float PruneauY { get; set; }
        public AnalogJoystick.Direction PruneauDirX { get; set; }
        public AnalogJoystick.Direction PruneauDirY { get; set; }

        public GameOfMeteors(ConsoleHardwareConfig config)
            : base(config) {
            World = new Composition(new byte[WorldSize * WorldSize / 8], WorldSize, WorldSize);
            Ship = new PlayerMissile("ship", WorldSize / 2, WorldSize / 2, World);
            ShipX = WorldSize / 2;
            ShipY = WorldSize / 2;
            Pruneau = new PlayerMissile() {
                Name = "Pruneau",
                Owner = World,
                IsVisible = false
            };
            Meteors = new Meteor[NumberOfMeteors];
            for (var i = 0; i < NumberOfMeteors; i++) {
                Meteors[i] = new Meteor(this, i,
                    new[] {0, WorldSize - 2, 0, WorldSize - 2} [i],
                    new[] {0, 0, WorldSize - 2, WorldSize - 2} [i]);
            }
            DisplayDelay = 0;
        }

        public override void Loop() {
            ShipX += (float)Hardware.JoystickLeft.XDirection * EnginePower;
            ShipY += (float)Hardware.JoystickLeft.YDirection * EnginePower;

            if (ShipX < 0) ShipX += WorldSize;
            if (ShipY < 0) ShipY += WorldSize;
            if (ShipX >= WorldSize) ShipX -= WorldSize;
            if (ShipY >= WorldSize) ShipY -= WorldSize;

            Ship.X = (int)ShipX;
            Ship.Y = (int)ShipY;

            foreach (var meteor in Meteors) {
                meteor.Move();
            }

            

            Hardware.Matrix.Display(World.GetFrame(0, 0));
        }
    }
}
