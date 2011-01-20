using System.Collections;

namespace netduino.helpers.Imaging {
    public class Composition {
        private readonly Hashtable _layers = new Hashtable();
        private byte[] _oddFrame;
        private byte[] _evenFrame;
        private int _frameCacheX;
        private int _frameCacheY;
        private Hashtable _collisionClasses;

        public void AddBitmap(
            string name,
            Bitmap bitmap,
            Bitmap mask = null,
            int offsetX = 0,
            int offsetY = 0,
            int zIndex = 0,
            Brightness brightness = Brightness.Full) {
            _layers.Add(name, new Layer {
                                            Bitmap = bitmap,
                                            Mask = mask,
                                            OffsetX = offsetX,
                                            OffsetY = offsetY,
                                            ZIndex = zIndex,
                                            Brightness = brightness,
                                            Owner = this
                                        });
            ClearCache();
        }

        private void ClearCache() {
            _oddFrame = _evenFrame = null;
            _collisionClasses = null;
        }

        public void AddLayer(string name, Layer layer) {
            _layers.Add(name, layer);
            ClearCache();
        }

        public void RemoveLayer(string name) {
            _layers.Remove(name);
            ClearCache();
        }

        public Layer this[string name] {
            get { return (Layer)_layers[name]; }
            set {
                _layers[name] = value;
                ClearCache();
            }
        }

        public byte[] GetFrame(int offsetX, int offsetY, bool odd) {
            if (offsetX == _frameCacheX && offsetY == _frameCacheY) {
                if (odd && _oddFrame != null) return _oddFrame;
                if (!odd && _evenFrame != null) return _evenFrame;
            }

            // .NET Micro does not support bidimensional arrays.
            // Behold the CLR_E_FAIL error! Use a jagged array.
            var zBuffer = new int[Bitmap.FrameSize][];
            for (var i = 0; i < zBuffer.Length; i++) {
                zBuffer[i] = new int[Bitmap.FrameSize];
            }
            var frame = new byte[Bitmap.FrameSize];

            BuildCollisionClasses();

            for (var x = 0; x < Bitmap.FrameSize; x++) {
                for (var y = 0; y < Bitmap.FrameSize; y++) {

                    foreach(Layer layer in _layers.Values) {

                        if (zBuffer[x][y] > layer.ZIndex) continue;
                        var layerX = offsetX + x - layer.OffsetX;
                        var layerY = offsetY + y - layer.OffsetY;

                        if (layer.Brightness == Brightness.Hidden ||
                            layerX > layer.Bitmap.Width ||
                            layerY > layer.Bitmap.Height ||
                            layerX < 0 ||
                            layerY < 0) continue;

                        if (layer.Mask != null && !layer.Mask.GetPixel(layerX, layerY)) continue;

                        var layerPixelLit = layer.Bitmap.GetPixel(layerX, layerY);
                        
                        if (layerPixelLit && (odd || layer.Brightness == Brightness.Full)) {
                            zBuffer[x][y] = layer.ZIndex;
                            frame[y] |= Bitmap.ShiftMasks[x];
                        }
                        else if (layer.Mask != null || layerPixelLit) {
                            zBuffer[x][y] = layer.ZIndex;
                            frame[y] &= Bitmap.ReverseShiftMasks[x];
                        }

                        //if ((layer.Mask == null && layerPixelLit) ||
                        //    (layer.Mask != null)) {
                            
                        //    collisions.Add(layer);
                        //}
                    }
                }
            }
            
            if (odd) _oddFrame = frame;
            else _evenFrame = frame;
            _frameCacheX = offsetX;
            _frameCacheY = offsetY;

            return frame;
        }

        private void BuildCollisionClasses() {
            if (_collisionClasses == null) {
                _collisionClasses = new Hashtable();
                foreach (Layer layer in _layers.Values) {
                    if (layer.Classes != null) {
                        foreach (string cls in layer.Classes) {
                            if (!_collisionClasses.Contains(cls)) {
                                _collisionClasses.Add(cls, new ArrayList());
                            }
                            ((ArrayList) _collisionClasses[cls]).Add(layer);
                        }
                    }
                }
            }
        }

        public void NotifyChange() {
            ClearCache();
        }
    }
}
