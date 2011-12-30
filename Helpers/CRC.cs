using System;
using Microsoft.SPOT;

namespace netduino.helpers.Helpers {
    // Based on http://www.lammertbies.nl/comm/info/crc-calculation.html
    public static class CrcCCITT {
        private static ushort[] _crcTable;
        private const ushort Polynomial = 0x1021;
        public static ushort Initialize() {
            if (_crcTable == null) {
                _crcTable = new ushort[256];
                ushort crc = 0;
                ushort c = 0;
                for (var tableIndex = 0; tableIndex < 256; tableIndex++) {
                    crc = 0;
                    c = (ushort)(tableIndex << 8);
                    for (var bitIndex = 0; bitIndex < 8; bitIndex++) {
                        if (((crc ^ c) & 0x8000) != 0)
                            crc = (ushort)((crc << 1) ^ Polynomial);
                        else
                            crc <<= 1;
                        c <<= 1;
                    }
                    _crcTable[tableIndex] = crc;
                }
            }
            return 0xFFFF;
        }
        public static ushort Update(ushort crc, byte data) {
            ushort shortData = (ushort)(0x00ff & (ushort)data);
            ushort crcLookup = (ushort)((crc >> 8) ^ shortData);
            crc = (ushort)((crc << 8) ^ _crcTable[crcLookup]);
            return crc;
        }
    }
}
