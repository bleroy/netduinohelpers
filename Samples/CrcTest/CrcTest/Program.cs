using System;
using System.Text;
using Microsoft.SPOT;
using netduino.helpers.Helpers;

namespace CrcTest {
    public class Program {
        public static void Main() {
            var encoding = new UTF8Encoding();
            ushort crc = CrcCCITT.Initialize();
            crc = GenerateCrc(encoding.GetBytes("123456789"), crc);
            if (crc != 0x29B1) throw new ApplicationException("invalid");

            Debug.Print("crc(DejaVuSans9)" + GenerateCrc(encoding.GetBytes("DejaVuSans9"), CrcCCITT.Initialize()));
            Debug.Print("crc(DejaVuSansBold9)" + GenerateCrc(encoding.GetBytes("DejaVuSansBold9"), CrcCCITT.Initialize()));
            Debug.Print("crc(DejaVuSansCondensed9)" + GenerateCrc(encoding.GetBytes("DejaVuSansCondensed9"), CrcCCITT.Initialize()));
            Debug.Print("crc(DejaVuSansMono8)" + GenerateCrc(encoding.GetBytes("DejaVuSansMono8"), CrcCCITT.Initialize()));
            Debug.Print("crc(DejaVuSansMonoBold8)" + GenerateCrc(encoding.GetBytes("DejaVuSansMonoBold8"), CrcCCITT.Initialize()));
            Debug.Print("crc(Verdana14)" + GenerateCrc(encoding.GetBytes("Verdana14"), CrcCCITT.Initialize()));
            Debug.Print("crc(Verdana9)" + GenerateCrc(encoding.GetBytes("Verdana9"), CrcCCITT.Initialize()));
            Debug.Print("crc(VerdanaBold14)" + GenerateCrc(encoding.GetBytes("VerdanaBold14"), CrcCCITT.Initialize()));
        }
        public static ushort GenerateCrc(byte[] bytes, ushort crc) {
            foreach (var b in bytes) {
                crc = CrcCCITT.Update(crc, b);
            }
            return crc;
        }
    }
}
