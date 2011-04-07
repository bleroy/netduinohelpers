using System;
using System.IO;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.IO;
using SecretLabs.NETMF.Hardware.Netduino;
using netduino.helpers.Hardware;
using netduino.helpers.Imaging;
using netduino.helpers.Helpers;

namespace BeyondHelloWorld2
{
    public class Program
    {
        protected static PushButton JoystickButton;
        protected static AnalogJoystick LeftJoystick;
        protected static AnalogJoystick RightJoystick;

        public static void Main()
        {
            SDResourceLoader rsc = null;

            LeftJoystick = new AnalogJoystick(Pins.GPIO_PIN_A0, Pins.GPIO_PIN_A1);
            RightJoystick = new AnalogJoystick(Pins.GPIO_PIN_A2, Pins.GPIO_PIN_A3);

            // I'm being lazy here and using the default on-board switch instead of the actual joystick button :)
            JoystickButton = new PushButton(Pins.GPIO_PIN_D0, InterruptModes.InterruptEdgeLevelLow, ButtonEventHandler);

            var matrix = new Max72197221(chipSelect: Pins.GPIO_PIN_D10);

            matrix.Shutdown(Max72197221.ShutdownRegister.NormalOperation);
            matrix.SetDecodeMode(Max72197221.DecodeModeRegister.NoDecodeMode);
            matrix.SetDigitScanLimit(7);
            matrix.SetIntensity(8);

            //try {
            //    // Load the resources from the SD card 
            //    // Place the content of the "SD Card Resources" folder at the root of an SD card
            //    rsc = new SDResourceLoader(Pins.GPIO_PIN_D10);
            //}
            //catch (IOException) {
            //    ShowNoSDPresent(matrix);
            //}

            // Using the space invaders bitmap in this example
            //var Invaders = (Bitmap) rsc.Bitmaps["spaceinvaders.bmp.bin"];

            //rsc.Dispose();

            var comp = new Composition(new byte[8], 8, 8);
            var leftBall = comp.AddMissile("leftBall");
            var rightBall = comp.AddMissile("rightBall");

            while (true)
            {
                // Read the current direction of the joystick
                //X = LeftJoystick.x;
                //Y = LeftJoystick.y;

                // Validate the position of the coordinates to prevent out-of-bound exceptions.
                //if (X < 0)
                //{
                //    X = 0;
                //}
                //else if (X >= Invaders.Width - Bitmap.FrameSize)
                //{
                //    X = Invaders.Width - Bitmap.FrameSize;
                //}

                //if (Y < 0)
                //{
                //    Y = 0;
                //}
                //else if (Y >= Invaders.Height)
                //{
                //    Y = Invaders.Height - 1;
                //}

                leftBall.X = LeftJoystick.X / 128;
                leftBall.Y = LeftJoystick.Y / 128;
                rightBall.X = RightJoystick.X / 128;
                rightBall.Y = RightJoystick.Y / 128;

                Debug.Print("X=" + LeftJoystick.X.ToString() + " (" + LeftJoystick.XDirection.ToString() + ")" + ", Y=" + LeftJoystick.Y.ToString() + " (" + LeftJoystick.YDirection.ToString() + ")");

                // move the bitmap according to the direction of the joystick
                matrix.Display(comp.GetFrame(0, 0));
                //matrix.Display(Invaders.GetFrame(X, Y));

                Thread.Sleep(80);
            }
        }

        // When the button is pushed, bring back the bitmap to the starting point.
        public static void ButtonEventHandler(UInt32 port, UInt32 state, DateTime time)
        {
            JoystickButton.Input.DisableInterrupt();

            if (state == 1)
            {
                //X = 0;
                //Y = 0;
                Debug.Print("Click!");
            }

            JoystickButton.Input.EnableInterrupt();
        }

        // Shows and SD icon on the matrix and wait for a reset
        private static void ShowNoSDPresent(Max72197221 matrix)
        {
            var SD = new Bitmap(new byte[] { 0x7e, 0x42, 0x42, 0x42, 0x42, 0x42, 0x22, 0x1e }, 8, 8);
            matrix.Display(SD.GetFrame(0, 0));
            while (true) { 
                Thread.Sleep(1000); 
            }
        }
    }
}