using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace netduino.helpers.Hardware
{
    class AdaFruitILI9328
    {
        private OutputPort _chipSelect;
        private OutputPort _commandData;

        public AdaFruitILI9328(ShiftRegister74HC595 data, Cpu.Pin chipSelect, Cpu.Pin commandData, Cpu.Pin write, Cpu.Pin read, Cpu.Pin reset) {
            _chipSelect = new OutputPort(chipSelect, false);
            _commandData = new OutputPort(commandData, false);
        }
    }
}
