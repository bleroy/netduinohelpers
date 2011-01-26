namespace netduino.helpers.Imaging {
    public class PlayerMissile {
        private int _x;
        private int _y;
        private byte _brightness;

        public PlayerMissile(string name, int x, int y, byte brightness = (byte)255, Composition owner = null) {
            Name = name;
            _x = x;
            _y = y;
            _brightness = brightness;
            Owner = owner;
        }

        public string Name { get; private set; }

        public int X {
            get { return _x; }
            set {
                if (Owner != null) {
                    Owner.NotifyChange();
                }
                _x = value;
            }
        }
        
        public int Y {
            get { return _y; }
            set {
                if (Owner != null) {
                    Owner.NotifyChange();
                }
                _y = value;
            }
        }

        public byte Brightness {
            get { return _brightness; }
            set {
                if (Owner != null) {
                    Owner.NotifyChange();
                }
                _brightness = value;
            }
        }

        internal Composition Owner { get; set; }
    }
}
