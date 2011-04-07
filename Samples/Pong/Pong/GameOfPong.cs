using netduino.helpers.Fun;
using netduino.helpers.Imaging;

namespace Pong {
    public class GameOfPong : Game {
        bool _ballGoingDown;

        public int LeftScore;
        public int RightScore;

        public bool BallGoingRight { get; set; }

        public PlayerMissile Ball { get; private set; }
        public Paddle LeftPaddle { get; private set; }
        public Paddle RightPaddle { get; private set; }

        public GameOfPong() {
            World = new Composition(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 8, 8);
            Ball = new PlayerMissile("ball", 0, 0, World);
            LeftPaddle = new Paddle(Side.Left, this);
            RightPaddle = new Paddle(Side.Right, this);
            BallGoingRight = true;
            ResetBall();
        }

        public override void Loop() {
            // LeftPaddle.Y = get from stick
            // RightPaddle.Y = get from stick
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
            }
            if (Ball.Y >= 8) {
                Ball.Y = 7;
                _ballGoingDown = false;
            }
         }

        public void ResetBall() {
            Ball.X = 0;
            Ball.Y = Random.Next(8);
            BallGoingRight = true;
            _ballGoingDown = Random.Next(2) == 0;
        }
    }
}
