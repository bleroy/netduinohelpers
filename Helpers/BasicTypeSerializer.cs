using System;
using System.IO;
using System.Text;
using Microsoft.SPOT;
namespace netduino.helpers.Helpers {
    public class BasicTypeSerializer : IDisposable {
        public int ContentSize {
            get { return _currentIndex; }
        }
        public const int MinimumBufferSize = 100;
        public BasicTypeSerializer(int defaultBufferSize = 1024) {
            if (defaultBufferSize < MinimumBufferSize) throw new ArgumentOutOfRangeException("defaultBufferSize");
            _serializeBuffer = new byte[defaultBufferSize];
            _currentIndex = 0;
            _storeFunction = StoreToBuffer;
        }
        public BasicTypeSerializer(FileStream file) {
            if (file == null) throw new ArgumentNullException("file");
            _file = file;
            if (_file.CanWrite == false) throw new ApplicationException("file");
            _currentIndex = 0;
            _storeFunction = StoreToFile;
        }
        public void Put(UInt16 data) {
            Put((Int16) data);
        }
        public void Put(Int16 data) {
            Put((byte)(data >> 8));
            Put((byte)data);
        }
        public void Put(UInt32 data) {
            Put((byte)(data >> 24));
            Put((byte)(data >> 16));
            Put((byte)(data >> 8));
            Put((byte)data);
        }
        public void Put(Int32 data) {
            Put((UInt32)data);
        }
        public void Put(UInt64 data) {
            Put((byte)(data >> 56));
            Put((byte)(data >> 48));
            Put((byte)(data >> 40));
            Put((byte)(data >> 32));
            Put((byte)(data >> 24));
            Put((byte)(data >> 16));
            Put((byte)(data >> 8));
            Put((byte)data);
        }
        public void Put(Int64 data) {
            Put((UInt64)data);
        }
        public void Put(string text, bool ConvertToASCII = false) {
            Put((byte)(ConvertToASCII ? 1 : 0));
            if (ConvertToASCII) {
                Put(Encoding.UTF8.GetBytes(text));
            } else {
                Put((ushort)text.Length);
                foreach (var c in text) {
                    Put(c);
                }
            }
        }
        public void Put(byte[] bytes) {
            Put((ushort)bytes.Length);
            foreach (var b in bytes) {
                Put(b);
            }
        }
        public void Put(ushort[] array) {
            Put((ushort)array.Length);
            foreach (var e in array) {
                Put(e);
            }
        }
        public void Put(UInt32[] array) {
            Put((ushort)array.Length);
            foreach (var e in array) {
                Put(e);
            }
        }
        public void Put(UInt64[] array) {
            Put((ushort)array.Length);
            foreach (var e in array) {
                Put(e);
            }
        }
        public void Put(byte data) {
            _storeFunction(data);
        }
        private void StoreToBuffer(byte data) {
            if (_currentIndex < _serializeBuffer.Length) {
                _serializeBuffer[_currentIndex++] = data;
                return;
            } else { // Attempt to grow the buffer...
                var buffer = new byte[_serializeBuffer.Length * 2];
                Array.Copy(_serializeBuffer, buffer, _serializeBuffer.Length);
                _serializeBuffer = buffer;
                _serializeBuffer[_currentIndex++] = data;
                buffer = null;
                Debug.GC(true);
            }
        }
        private void StoreToFile(byte data) {
            _file.WriteByte(data);
            _currentIndex++;
        }
        public void Dispose() {
            _encoding = null;
            _serializeBuffer = null;
            _file = null;
            _storeFunction = null;
        }
        public byte[] GetBuffer() {
            return _serializeBuffer;
        }
        private delegate void StoreByte(byte data);
        private UTF8Encoding _encoding = new UTF8Encoding();
        private FileStream _file;
        private byte[] _serializeBuffer;
        private int _currentIndex;
        private StoreByte _storeFunction;
    }
}
