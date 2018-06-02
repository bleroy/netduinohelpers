# netduinohelpers

Salvaged from the ruins of CodePlex

## Project Description

The 'netduino Helpers' is a C# library providing hardware drivers and utility classes supporting the following:

* Analog joysticks
* Interrupt-driven push-button
* Real-time clock (DS1307)
* 8*8 LED matrix
* AdaFruit ILI932x TFT display
* AdaFruit SSD1306 OLED display
* AdaFruit ST7735 TFT display
* AdaFruit VC0706 Camera
* AdaFruit ST7565 Negative LCD display
* AdaFruit LPD8806 LED strip driver
* Sharp GP2Y0A21YK0F Distance Measuring Sensor
* Shift registers (74hc595)
* Runtime assembly &amp; resource loader
* Bitmaps &amp;&nbsp;Bitmap Compositions
* HiTec HS6635HB servo driver
* Serial Interface Builder
* LED matrix driver (Maxim7219 or compatible)
* RTTL sound support (ringtones)
* Variable / Fixed type font rendering
* 16x16 icon rendering
* Basic type serialization / deserialization
* Trigonometry library (exMath-based)
* Lightweight JSON parser (parsing from stream &amp; string)
* Virtual Memory support
* PIX-6T4 console bootloader and games

The library is available as source-code only.

## Samples

Check the \netduino.helpers\Samples folder for the samples and demos. The project called 'Samples' references all the others and will allowing building everything in one shot.

### Sample descriptions

* **\netduino.helpers\Samples\PIX6T4** contains the console boot loader and games.
  The complete project page, with schematics, source code, documentation and ready-to-play games are available from <http://www.pix6t4.com/>.

* **\netduino.helpers\Samples\ASSM** is a simple class used to illustrate [dynamic assembly loading of little-endian PE files on the netduino](http://fabienroyer.wordpress.com/2010/12/29/loading-assemblies-dynamically-from-an-sd-card-with-a-netduino/).

* **\netduino.helpers\Samples\BeyondHelloWorld** is a large sample making use of:
  * an [SD card driver](http://fabienroyer.wordpress.com/2010/12/23/connecting-an-sd-card-reader-to-a-netduino/)
    Be sure to place the content of the **\BeyondHelloWorld\SD Card Resources** folder at the root of the SD card before running the sample
  * [loads bitmaps, strings and assemblies dynamically](http://fabienroyer.wordpress.com/2010/12/29/loading-assemblies-dynamically-from-an-sd-card-with-a-netduino/)
  * [drives an 8x8 LED matrix using Persistence Of Vision](http://fabienroyer.wordpress.com/2010/12/03/driving-an-8x8-led-matrix-with-a-netduino-using-persistence-of-vision/)
  * [uses an analog joystick for input](http://fabienroyer.wordpress.com/2011/01/09/connecting-an-analog-joystick-to-a-netduino/)
  
  For a complete project using these drivers, see the [PIX-6T4 project](http://www.pix6t4.com/source).

* **\netduino.helpers\Samples\DS1307** tests all the [functions of the Maxim DS1307 real-time clock](http://fabienroyer.wordpress.com/2011/01/03/keeping-track-of-time-on-a-netduino-using-a-maxim-ds1307-real-time-clock/).

* **\netduino.helpers\Samples\AdaFruitSSD1306** [sample driving an AdaFruit SSD1306 monochrome OLED display](http://fabienroyer.wordpress.com/2011/01/16/driving-an-adafruit-ssd1306-oled-display-with-a-netduino/)

* **\netduino.helpers\Samples\AdaFruitST7735Test** [sample driving an AdaFruit ST7735 TFT display](http://fabienroyer.wordpress.com/2011/05/29/driving-an-adafruit-st7735-tft-display-with-a-netduino/)

* **\netduino.helpers\Samples\AdaFruit7565Test** [sample driving an AdaFruit ST7565 Negative LCD display](http://fabienroyer.wordpress.com/2011/09/14/driving-an-adafruit-st7565-negative-lcd-display-with-a-netduino/)

* **\netduino.helpers\Samples\AdaFruitVC0706Test** [sample driving an AdaFruit VC0706 TTL serial camera](http://fabienroyer.wordpress.com/2011/08/12/driving-an-adafruit-vc0706-ttl-serial-jpeg-camera-with-a-netduino/)

* **\netduino.helpers\Samples\AdaFruitLPD8806Test** [demo
 driving an AdaFruit LPD8806 RGB LED strip as a 16x10 display](http://www.pix6t4.com/blog/Building-a-mini-RGB-LED-video-wall-using-a-Netduino-and-an-Adafruit-LPD8806-LED-strip)

* **\netduino.helpers\Samples\SharpDistanceSensorTest** sample using the Sharp GP2Y0A21YK0F Distance Measuring Sensor. Not documented.

* **\netduino.helpers\Samples\ImagingSamples** show how to build multi-layered bitmap compositions.
  [Used as part of the PIX-6T4 project](http://www.pix6t4.com/source).

* **\netduino.helpers\Samples\HiTecServoTest** simple unit test for the HiTec servo driver moving the arm back and forth from 0 to 180 degrees. This project is documented [here](http://fabienroyer.wordpress.com/2011/02/22/saving-energy-with-a-netduino/).

* **\netduino.helpers\Samples\Max72197221Test** unit tests exercising all the functions of the Maxim 7219 / 7221 LED driver.
  [This driver is documented here](http://fabienroyer.wordpress.com/2011/03/13/using-a-max7219max7221-led-display-driver-with-a-netduino/) and is used in the [PIX-6T4 project](http://www.pix6t4.com/source).

* **\netduino.helpers\Samples\RingToneTest** unit tests for the RTTL sound support. This driver is not documented.

* **\netduino.helpers\Samples\PIPBoy3000** Geiger counter and Twitter client sample.
  [This project is documented here](http://fabienroyer.wordpress.com/2011/05/08/build-a-twitter-enabled-geiger-counter-with-a-netduino/).

* **\netduino.helpers\Samples\JSONParserTest** JSON parser unit test samples.
  This project is documented as part of the **Klout Klock** sample application.
  [This project is documented here](http://fabienroyer.wordpress.com/2011/07/18/build-a-klout-klock-track-your-influence-and-time/).

* **\netduino.helpers\Samples\KloutKlock** network-enabled clock using NTP time to display the date and time and Klout web services to show the user's influence within Social Networks. Requires a Netduino Plus and an AdaFruit ST7735 TFT screen.
  [This project is documented here](http://fabienroyer.wordpress.com/2011/07/18/build-a-klout-klock-track-your-influence-and-time/).

* **\netduino.helpers\Samples\WaterHeaterController** water heater controller designed to save energy.
  Makes use of the Serial Interface Builder, the Hitec HS6635HB servo and the DS1307 clock. This project is documented [here](http://fabienroyer.wordpress.com/2011/02/22/saving-energy-with-a-netduino/).

### Dependencies

Be sure to check out the [netduino Beta forum](http://forums.netduino.com/index.php?/forum/18-netduino-beta/) for the latest flash version required to support SD card I/Os.

All applications have been tested under firmware 4.1.1 Beta 1 only. .Net MF v4.2 is not yet supported as some features were taken out since 4.1.1 unfortunately.

### Memory constraints

The library is large and will need to be trimmed to meet the needs of your application. This is especially relevant to the Netduino Plus. You can do this two ways: either take out the drivers and classes that you don't need / want and recompile the library (be mindful of inter-dependencies between some of the classes) or you can create you own library using a subset of the files making the Netduino Helpers library.

### Tools

**\netduino.helpers\Tools** contains `.bmp` conversion programs. Expect 1-bit or 24-bit depth horizontal bitmaps.
