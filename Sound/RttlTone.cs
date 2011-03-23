using System;
using Microsoft.SPOT;

namespace netduino.helpers.Sound {
    public class RttlTone {
        
        public uint Note { get; private set; }
        public uint Period { get; private set; }

        private int _duration;

        /// <summary>
        /// Calculate the duration of the note based on the song's tempo
        /// </summary>
        /// <param name="tempo">Song's tempo variable</param>
        /// <returns>Duration of the note in milliseconds</returns>
        public int GetDelay(int tempo) {
            return tempo / _duration;
        }

        /// <summary>
        /// Build a PWN tone from an RTTL note and duration
        /// </summary>
        /// <param name="note"></param>
        /// <param name="duration"></param>
        public RttlTone(uint note, uint duration) {
            Note = note;
            _duration = (int) duration;
            if (note > 0) {
                Period = 1000000 / note; // 1000000 = 1 sec. Period = 1 sec / frequency
            } else {
                Period = 0;
            }
        }
    }
}
