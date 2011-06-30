using System.Threading;
using netduino.helpers.Fun;
using netduino.helpers.Hardware;
using netduino.helpers.Imaging;

namespace Meteors {
    public class GameOfMeteors : Game {
        public const int WorldSize = 8;
        public const int StartingNumberOfMeteors = 1;
        public const int WinningNumberOfMeteors = 8;
        private const float EnginePower = 0.3f;
        private const float PruneauSpeed = 1f;

        public Meteor[] Meteors { get; private set; }
        public PlayerMissile Pruneau { get; private set; }
        public PlayerMissile Ship { get; private set; }
        public int NumberOfMeteors { get; private set; }
        public int RemainingRocks { get; private set; }

        public GameOfMeteors(ConsoleHardwareConfig config) : base(config) {
            World = new Composition(new byte[WorldSize * WorldSize / 8], WorldSize, WorldSize);
            World.Coinc += WorldCoinc;
            Ship = new PlayerMissile("ship", WorldSize / 2, WorldSize / 2, World);
            Ship.X = WorldSize / 2;
            Ship.Y = WorldSize / 2;
            Pruneau = new PlayerMissile {
                Name = "Pruneau",
                Owner = World,
                IsVisible = false
            };
            Meteors = new Meteor[WinningNumberOfMeteors];
            for (var i = 0; i < Meteors.Length; i++) {
                Meteors[i] = new Meteor(this, i);
            }
            NumberOfMeteors = StartingNumberOfMeteors;
            SpawnMeteors();
            DisplayDelay = 0;
        }

        private void SpawnMeteors() {
            for (int i = 0; i < NumberOfMeteors; i++) {
                Meteors[i].Respawn(new[] {0, WorldSize - 2, 0, WorldSize - 2}[i%4],
                                   new[] {0, 0, WorldSize - 2, WorldSize - 2}[i%4]);
            }
            RemainingRocks = NumberOfMeteors*2;
        }

        void WorldCoinc(object sender, CoincEventArgs e) {
            if (e.Missile1 == Pruneau || e.Missile2 == Pruneau) {
                // Explode rock or meteor
                var rock = e.Missile1 == Pruneau ? e.Missile2 : e.Missile1;
                var meteor = GetOwner(rock);
                // Does this rock belong to a meteor?
                if (meteor != null) {
                    if (meteor.IsExploded) {
                        rock.IsVisible = false;
                        RemainingRocks--;
                        if (RemainingRocks == 0) {
                            Hardware.Matrix.Display(SmallChars.ToBitmap(0, NumberOfMeteors));
                            Thread.Sleep(1000);
                            NumberOfMeteors++;
                            if (NumberOfMeteors >= WinningNumberOfMeteors) {
                                Stop();
                            }
                            else {
                                SpawnMeteors();
                                e.CancelCollisionDetection = true;
                            }
                        }
                    }
                    else {
                        meteor.Explode();
                        MakeExplosionSound();
                    }
                    Pruneau.IsVisible = false;
                    return;
                }
            }
            else if (e.Missile1 == Ship || e.Missile2 == Ship) {
                MakeExplosionSound();
                Stop();
            }
        }

        protected virtual void OnGameStart() {
            ScrollMessage(" Asteroids", 50, ScrollStopFunction);
        }
        
        private void OnGameOver() {
            ScrollMessage(" Game Over!", 50, ScrollStopFunction);
        }

        private Meteor GetOwner(PlayerMissile rock) {
            foreach (var meteor in Meteors) {
                if (meteor.Has(rock)) return meteor;
            }
            return null;
        }

        public void MakeExplosionSound() {
            Beep(3000, 10);
            Beep(1000, 10);
        }

        public override void Loop() {
            // Ship
            Ship.HorizontalSpeed = (float)Hardware.JoystickLeft.XDirection * EnginePower;
            Ship.VerticalSpeed = (float)Hardware.JoystickLeft.YDirection * EnginePower;
            Ship.Move();

            var ship = Ship;
            ApplyToreGeometry(ship);

            // Meteors
            foreach (var meteor in Meteors) {
                meteor.Move();
            }

            // Pruneau
            if (Pruneau.IsVisible) {
                Pruneau.Move();
                if (Pruneau.ExactX < 0 ||
                    Pruneau.ExactY < 0 ||
                    Pruneau.ExactX >= WorldSize ||
                    Pruneau.ExactY >= WorldSize) {

                    Pruneau.IsVisible = false;
                }
            }
            else {
                var shootXDir = Hardware.JoystickRight.XDirection;
                var shootYDir = Hardware.JoystickRight.YDirection;
                if (shootXDir != AnalogJoystick.Direction.Center ||
                    shootYDir != AnalogJoystick.Direction.Center) {

                    Pruneau.ExactX = Ship.ExactX;
                    Pruneau.ExactY = Ship.ExactY;
                    Pruneau.HorizontalSpeed = (float) shootXDir*PruneauSpeed;
                    Pruneau.VerticalSpeed = (float) shootYDir*PruneauSpeed;
                    Pruneau.IsVisible = true;
                    Beep(2000, 20);
                    Beep(1000, 20);
                }
            }

            // Display
            Hardware.Matrix.Display(World.GetFrame(0, 0));
        }

        public static void ApplyToreGeometry(PlayerMissile missile) {
            if (missile.ExactX < 0) missile.ExactX += WorldSize;
            if (missile.ExactY < 0) missile.ExactY += WorldSize;
            if (missile.ExactX >= WorldSize) missile.ExactX -= WorldSize;
            if (missile.ExactY >= WorldSize) missile.ExactY -= WorldSize;
        }
    }
}
