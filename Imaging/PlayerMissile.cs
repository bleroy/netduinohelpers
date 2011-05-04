namespace netduino.helpers.Imaging {
    public class PlayerMissile {
        private float _x;
        private float _y;
        private Composition _owner;

        public PlayerMissile() : this(null, 0, 0) {}

        public PlayerMissile(string name, int x, int y, Composition owner = null) {
            Name = name;
            _x = x;
            _y = y;
            HorizontalSpeed = 0;
            VerticalSpeed = 0;
            IsVisible = true;
            Owner = owner;
        }

        public bool IsVisible { get; set; }
        public string Name { get; set; }

        public int X {
            get { return (int)_x; }
            set {
                _x = value;
                if (Owner != null) {
                    Owner.NotifyChange();
                }
            }
        }
        
        public int Y {
            get { return (int)_y; }
            set {
                _y = value;
                 if (Owner != null) {
                    Owner.NotifyChange();
                }
           }
        }

        public float HorizontalSpeed { get; set; }
        
        public float VerticalSpeed { get;set; }

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

        public void Move() {
            _x += HorizontalSpeed;
            _y += VerticalSpeed;
            if (Owner != null) {
                Owner.NotifyChange();
            }
        }
    }
}
