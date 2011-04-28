namespace netduino.helpers.Imaging {
    public class PlayerMissile {
        private int _x;
        private int _y;
        private Composition _owner;

        public PlayerMissile() : this(null, 0, 0, null) {}

        public PlayerMissile(string name, int x, int y, Composition owner = null) {
            Name = name;
            _x = x;
            _y = y;
            IsVisible = true;
            _owner = owner;
        }

        public bool IsVisible { get; set; }
        public string Name { get; set; }

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

        public Composition Owner {
            get {
                return _owner;
            }
            set {
                _owner = value;
                if (_owner != null) {
                    _owner.AddMissile(this);
                }
            }
        }
    }
}
