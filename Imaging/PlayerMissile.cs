namespace netduino.helpers.Imaging {
    public class PlayerMissile {
        private int _x;
        private int _y;

        public PlayerMissile(string name, int x, int y, Composition owner = null) {
            Name = name;
            _x = x;
            _y = y;
            Owner = owner;
            if (Owner != null) {
                Owner.AddMissile(this);
            }
        }

        public string Name { get; private set; }

        public int X {
            get { return _x; }
            set {
                _x = value;
                if (Owner != null) {
                    Owner.NotifyChange();
                }
            }
        }
        
        public int Y {
            get { return _y; }
            set {
                _y = value;
                 if (Owner != null) {
                    Owner.NotifyChange();
                }
           }
        }

        internal Composition Owner { get; set; }
    }
}
