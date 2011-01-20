using Microsoft.SPOT;

namespace netduino.helpers.Imaging {
    public class CoincEventArgs : EventArgs {
        public CoincEventArgs(string cls, Layer projectile, int x, int y) {
            Class = cls;
            Projectile = projectile;
            X = x;
            Y = y;
        }

        public string Class { get; private set; }
        public Layer Projectile { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
    }
}
