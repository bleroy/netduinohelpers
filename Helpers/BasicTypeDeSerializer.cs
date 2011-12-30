using System;
using System.IO;
using System.Text;
using Microsoft.SPOT;
namespace netduino.helpers.Helpers {
    public class BasicTypeDeSerializer : IDisposable {
        public int ContentSize {
            get {
                if (_buffer != null) {
                    return _contentSize;
                } else {
                    return (int)_file.Length;
                }
            }
        }
        public bool MoreData {
            get {
                return (_currentIndex < _contentSize) ? true : false;
            }
        }
        public BasicTypeDeSerializer(byte[] buffer, int contentSize) {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (contentSize == 0) throw new ArgumentNullException("contentSize");
            _buffer = buffer;
            _currentIndex = 0;
            _contentSize = contentSize;
            _readFunction = ReadFromBuffer;
        }
        public BasicTypeDeSerializer(FileStream file) {
            if (file == null) throw new ArgumentNullException("file");
            _file = file;
            _currentIndex = 0;
            _contentSize = (int)file.Length;
            _readFunction = ReadFromFile;
        }
        public UInt16 Get(UInt16 data) {
            data = Get();
            data <<= 8;
            data |= Get();
            return data;
        }
        public Int16 Get(Int16 data) {
            UInt16 temp;
            temp = Get();
            temp <<= 8;
            temp |= Get();
            return (Int16)temp;
        }
        public UInt32 Get(UInt32 data) {
            data = Get();
            data <<= 8;
            data |= Get();
            data <<= 8;
            data |= Get();
            data <<= 8;
            data |= Get();
            return data;
        }
        public Int32 Get(Int32 data) {
            data = Get();
            data <<= 8;
            data |= Get();
            data <<= 8;
            data |= Get();
            data <<= 8;
            data |= Get();
            return data;
        }
        public UInt64 Get(UInt64 data) {
            data = Get();
            data <<= 8;
            data |= Get();
            data <<= 8;
            data |= Get();
            data <<= 8;
            data |= Get();
            data <<= 8;
            data |= Get();
            data <<= 8;
            data |= Get();
            data <<= 8;
            data |= Get();
            data <<= 8;
            data |= Get();
            return data;
        }
        public Int64 Get(Int64 data) {
            return (Int64)Get((UInt64)data);
        }
        public string Get(string text) {
            byte IsASCII = 0;
            IsASCII = Get();
            ushort length = 0;
            length = Get(length);
            if (length != 0) {
                if (IsASCII == 1) {
                    var bytes = new byte[length];
                    var index = 0;
                    while (length-- != 0) {
                        bytes[index++] = Get();
                    }
                    return new string(Encoding.UTF8.GetChars(bytes));
                } else {
                    var unicodeChars = new char[length];
                    var index = 0;
                    ushort unicodeChar = 0;
                    while (length-- != 0) {
                        unicodeChars[index++] = (char)Get(unicodeChar);
                    }
                    return new string(unicodeChars);
                }
            }
            return "";
        }
        public byte[] Get(byte[] bytes) {
            ushort length = 0;
            length = Get(length);
            if (length != 0) {
                var buffer = new byte[length];
                var index = 0;
                while (length-- != 0) {
                    buffer[index++] = Get();
                }
                return buffer;
            }
            return null;
        }
        public ushort[] Get(ushort[] array) {
            ushort length = 0;
            length = Get(length);
            if (length != 0) {
                var buffer = new ushort[length];
                var index = 0;
                UInt16 data = 0;
                while (length-- != 0) {
                    buffer[index++] = Get(data);
                }
                return buffer;
            }
            return null;
        }
        public UInt32[] Get(UInt32[] array) {
            ushort length = 0;
            length = Get(length);
            if (length != 0) {
                var buffer = new UInt32[length];
                var index = 0;
                UInt32 data = 0;
                while (length-- != 0) {
                    buffer[index++] = Get(data);
                }
                return buffer;
            }
            return null;
        }
        public UInt64[] Get(UInt64[] array) {
            ushort length = 0;
            length = Get(length);
            if (length != 0) {
                var buffer = new UInt64[length];
                var index = 0;
                UInt64 data = 0;
                while (length-- != 0) {
                    buffer[index++] = Get(data);
                }
                return buffer;
            }
            return null;
        }
        public byte Get() {
            return _readFunction();
        }
        private byte ReadFromBuffer() {
            return _buffer[_currentIndex++];
        }
        private byte ReadFromFile() {
            _currentIndex++;
            return (byte)_file.ReadByte();
        }
        public void Dispose() {
            _encoding = null;
            _file = null;
            _buffer = null;
            _readFunction = null;
        }
        private delegate byte ReadByte();
        private UTF8Encoding _encoding = new UTF8Encoding();
        private FileStream _file;
        private byte[] _buffer;
        private int _currentIndex;
        private int _contentSize;
        private ReadByte _readFunction; 
    }
}
