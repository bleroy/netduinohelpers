using System.Collections;

namespace netduino.helpers.Imaging {
    public class Layer {
        internal readonly Hashtable Coincs = new Hashtable();

        public Composition Owner {
            get;
            internal set;
        }

        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int ZIndex { get; set; }
        public Bitmap Bitmap { get; set; }
        public Bitmap Mask { get; set; }
        public string[] Classes { get; set; }
        public Brightness Brightness { get; set; }

        public void CallCoinc(string className, Coinc handler) {
            Coincs.Add(className, handler);
        }

        public void RemoveCoinc(string className) {
            Coincs.Remove(className);
        }
    }
}
