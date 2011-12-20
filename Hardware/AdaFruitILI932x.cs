using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace netduino.helpers.Hardware {
    /// <summary>
    /// Graphics library by ladyada/adafruit with init code from Rossum (MIT license)
    /// https://github.com/adafruit/TFTLCD-Library/
    /// </summary>
    public class AdaFruitILI932x
    {
        private enum Register {
            START_OSC            = 0x00,
            DRIV_OUT_CTRL		 = 0x01,
            DRIV_WAV_CTRL		 = 0x02,
            ENTRY_MOD			 = 0x03,
            RESIZE_CTRL			 = 0x04,
            DISP_CTRL1			 = 0x07,
            DISP_CTRL2			 = 0x08,
            DISP_CTRL3			 = 0x09,
            DISP_CTRL4			 = 0x0A,
            RGB_DISP_IF_CTRL1	 = 0x0C,
            FRM_MARKER_POS		 = 0x0D,
            RGB_DISP_IF_CTRL2	 = 0x0F,
            POW_CTRL1			 = 0x10,
            POW_CTRL2			 = 0x11,
            POW_CTRL3			 = 0x12,
            POW_CTRL4			 = 0x13,
            GRAM_HOR_AD			 = 0x20,
            GRAM_VER_AD			 = 0x21,
            RW_GRAM				 = 0x22,
            POW_CTRL7			 = 0x29,
            FRM_RATE_COL_CTRL	 = 0x2B,
            GAMMA_CTRL1			 = 0x30,
            GAMMA_CTRL2			 = 0x31,
            GAMMA_CTRL3			 = 0x32,
            GAMMA_CTRL4			 = 0x35,
            GAMMA_CTRL5			 = 0x36,
            GAMMA_CTRL6			 = 0x37,
            GAMMA_CTRL7			 = 0x38,
            GAMMA_CTRL8			 = 0x39,
            GAMMA_CTRL9			 = 0x3C,
            GAMMA_CTRL10		 = 0x3D,
            HOR_START_AD		 = 0x50,
            HOR_END_AD			 = 0x51,
            VER_START_AD		 = 0x52,
            VER_END_AD			 = 0x53,
            GATE_SCAN_CTRL1		 = 0x60,
            GATE_SCAN_CTRL2		 = 0x61,
            GATE_SCAN_CTRL3		 = 0x6A,
            PART_IMG1_DISP_POS	 = 0x80,
            PART_IMG1_START_AD	 = 0x81,
            PART_IMG1_END_AD	 = 0x82,
            PART_IMG2_DISP_POS	 = 0x83,
            PART_IMG2_START_AD	 = 0x84,
            PART_IMG2_END_AD	 = 0x85,
            PANEL_IF_CTRL1		 = 0x90,
            PANEL_IF_CTRL2		 = 0x92,
            PANEL_IF_CTRL3		 = 0x93,
            PANEL_IF_CTRL4		 = 0x95,
            PANEL_IF_CTRL5		 = 0x97,
            PANEL_IF_CTRL6		 = 0x98
        }

        private OutputPort _chipSelect;
        private OutputPort _commandData;
        private OutputPort _write;
        //private OutputPort _read;
        private OutputPort _reset;
        private ShiftRegister74HC595 _parallelDataOut;

        public AdaFruitILI932x(
            ShiftRegister74HC595 shiftRegister,
            Cpu.Pin tftChipSelect,
            Cpu.Pin tftCommandData,
            Cpu.Pin tftWrite,
            Cpu.Pin tftRead,
            Cpu.Pin tftReset) {
                _parallelDataOut = shiftRegister;
                _chipSelect = new OutputPort(tftChipSelect, true);
                _commandData = new OutputPort(tftCommandData, true);
                _write = new OutputPort(tftWrite, true);
                //_read = new OutputPort(tftRead, false);
                _reset = new OutputPort(tftReset, true);
        }
        public void Initialize() {
            Reset();
            ushort _delay = 0xFF;
            ushort[] _initializationSequence = new ushort[] {
            (ushort) Register.START_OSC, 0x0001,     // start oscillator
            _delay, 50,                           // this will make a delay of 50 milliseconds
            (ushort) Register.DRIV_OUT_CTRL, 0x0100, 
            (ushort) Register.DRIV_WAV_CTRL, 0x0700,
            (ushort) Register.ENTRY_MOD, 0x1030,
            (ushort) Register.RESIZE_CTRL, 0x0000,
            (ushort) Register.DISP_CTRL2, 0x0202,
            (ushort) Register.DISP_CTRL3, 0x0000,
            (ushort) Register.DISP_CTRL4, 0x0000,
            (ushort) Register.RGB_DISP_IF_CTRL1, 0x0,
            (ushort) Register.FRM_MARKER_POS, 0x0,
            (ushort) Register.RGB_DISP_IF_CTRL2, 0x0,
            (ushort) Register.POW_CTRL1, 0x0000,
            (ushort) Register.POW_CTRL2, 0x0007,
            (ushort) Register.POW_CTRL3, 0x0000,
            (ushort) Register.POW_CTRL4, 0x0000,
            _delay, 200,
            (ushort) Register.POW_CTRL1, 0x1690,
            (ushort) Register.POW_CTRL2, 0x0227,
            _delay, 50,
            (ushort) Register.POW_CTRL3, 0x001A,
            _delay, 50,
            (ushort) Register.POW_CTRL4, 0x1800,
            (ushort) Register.POW_CTRL7, 0x002A,
            _delay,50,
            (ushort) Register.GAMMA_CTRL1, 0x0000,    
            (ushort) Register.GAMMA_CTRL2, 0x0000, 
            (ushort) Register.GAMMA_CTRL3, 0x0000,
            (ushort) Register.GAMMA_CTRL4, 0x0206,   
            (ushort) Register.GAMMA_CTRL5, 0x0808,  
            (ushort) Register.GAMMA_CTRL6, 0x0007,  
            (ushort) Register.GAMMA_CTRL7, 0x0201,
            (ushort) Register.GAMMA_CTRL8, 0x0000,  
            (ushort) Register.GAMMA_CTRL9, 0x0000,  
            (ushort) Register.GAMMA_CTRL10, 0x0000,  
            (ushort) Register.GRAM_HOR_AD, 0x0000,  
            (ushort) Register.GRAM_VER_AD, 0x0000,  
            (ushort) Register.HOR_START_AD, 0x0000,
            (ushort) Register.HOR_END_AD, 0x00EF,
            (ushort) Register.VER_START_AD, 0X0000,
            (ushort) Register.VER_END_AD, 0x013F,
            (ushort) Register.GATE_SCAN_CTRL1, 0xA700,     // Driver Output Control (R60h)
            (ushort) Register.GATE_SCAN_CTRL2, 0x0003,     // Driver Output Control (R61h)
            (ushort) Register.GATE_SCAN_CTRL3, 0x0000,     // Driver Output Control (R62h)
            (ushort) Register.PANEL_IF_CTRL1, 0X0010,     // Panel Interface Control 1 (R90h)
            (ushort) Register.PANEL_IF_CTRL2, 0X0000,
            (ushort) Register.PANEL_IF_CTRL3, 0X0003,
            (ushort) Register.PANEL_IF_CTRL4, 0X1100,
            (ushort) Register.PANEL_IF_CTRL5, 0X0000,
            (ushort) Register.PANEL_IF_CTRL6, 0X0000,
            (ushort) Register.DISP_CTRL1, 0x0133     // Display Control (R07h) - Display ON
            };
            for (var i = 0; i < _initializationSequence.Length; i+=2) {
                var register = (ushort) _initializationSequence[i];
                var data = (ushort) _initializationSequence[i + 1];
                if (register == _delay) {
                    Thread.Sleep(data);
                    Debug.Print("Delay: " + data);
                } else {
                    WriteRegister(register, data);
                    Debug.Print("Register: " + register + "(" + data + ")");
                }
            }
        }
        public void Reset() {
          _reset.Write(false);
          Thread.Sleep(2);
          _reset.Write(true);

          // resync
          WriteData(0);
          WriteData(0);
          WriteData(0);
          WriteData(0);
        }
        protected void WriteRegister(ushort address, ushort data) {
            WriteCommand(address);
            WriteData(data);
        }
        protected void WriteCommand(ushort command) {
            _chipSelect.Write(false);   //digitalWrite(_cs, LOW);
            _commandData.Write(false);  //digitalWrite(_cd, LOW);
            //_read.Write(true);          //digitalWrite(_rd, HIGH);
            _write.Write(true);         //digitalWrite(_wr, HIGH);
            _parallelDataOut.Write((byte) (command >> 8));
            _write.Write(false); //digitalWrite(_wr, LOW);
            _write.Write(true); //digitalWrite(_wr, HIGH);
            _parallelDataOut.Write((byte)(command));
            _write.Write(false); //digitalWrite(_wr, LOW);
            _write.Write(true); //digitalWrite(_wr, HIGH);
            _chipSelect.Write(true);   //digitalWrite(_cs, HIGH);
        }
        protected void WriteData(ushort data) {
            _chipSelect.Write(false);   //digitalWrite(_cs, LOW);
            _commandData.Write(true);  //digitalWrite(_cd, HIGH);
            //_read.Write(true);          //digitalWrite(_rd, HIGH);
            _write.Write(true);         //digitalWrite(_wr, HIGH);
            _parallelDataOut.Write((byte)(data >> 8));
            _write.Write(false); //digitalWrite(_wr, LOW);
            _write.Write(true); //digitalWrite(_wr, HIGH);
            _parallelDataOut.Write((byte)(data));
            _write.Write(false); //digitalWrite(_wr, LOW);
            _write.Write(true); //digitalWrite(_wr, HIGH);
            _chipSelect.Write(true);   //digitalWrite(_cs, HIGH);
        }
        protected void WriteDataUnsafe(ushort data) {
            _parallelDataOut.Write((byte)(data >> 8));
            _write.Write(false); //digitalWrite(_wr, LOW);
            _write.Write(true); //digitalWrite(_wr, HIGH);
            _parallelDataOut.Write((byte)(data));
            _write.Write(false); //digitalWrite(_wr, LOW);
            _write.Write(true); //digitalWrite(_wr, HIGH);
        }
        public void DrawPixel(ushort x, ushort y, ushort color) {
          if (x >= 320 || y >= 240) return;
          WriteRegister((ushort)Register.GRAM_HOR_AD, x); // GRAM Address Set (Horizontal Address) (R20h)
          WriteRegister((ushort)Register.GRAM_VER_AD, y); // GRAM Address Set (Vertical Address) (R21h)
          WriteCommand((ushort)Register.RW_GRAM);  // Write Data to GRAM (R22h)
          WriteData(color);
        }
    }
}
