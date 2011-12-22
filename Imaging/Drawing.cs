using System;
using Microsoft.SPOT;
using netduino.helpers.Hardware;

namespace netduino.helpers.Imaging {
    public class RGBColor24 {
        public byte Red;
        public byte Green;
        public byte Blue;
    }
    /// <summary>
    /// Based on MicroBuilder's code: http://www.microbuilder.eu/Projects/LPC1343ReferenceDesign/TFTLCDAPI.aspx
    /// </summary>
    public class Drawing {
        public enum RoundedCornerStyle {
            NONE,
            ALL,
            TOP,
            BOTTOM,
            LEFT,
            RIGHT
        }
        public enum CornerPosition {
            TOPLEFT,
            TOPRIGHT,
            BOTTOMLEFT,
            BOTTOMRIGHT
        }
        public enum DrawingDirection {
            LEFT,
            RIGHT,
            UP,
            DOWN
        }
        
        private LCD _display;

        public Drawing(LCD display) {
            _display = display;
        }
        public void DrawTestPattern() {
        }
        public void DrawPixel(ushort x, ushort y, ushort color) {
        }
        public void DrawFill(ushort color) {
        }
        public void DrawLine(ushort x0, ushort y0, ushort x1, ushort y1, ushort color) {
        }
        public void DrawLineDotted(ushort x0, ushort y0, ushort x1, ushort y1, ushort space, ushort solid, ushort color) {
        }
        public void DrawCircle(ushort xCenter, ushort yCenter, ushort radius, ushort color) {
        }
        public void DrawCircleFilled(ushort xCenter, ushort yCenter, ushort radius, ushort color) {
        }
        public void DrawCornerFilled(ushort xCenter, ushort yCenter, ushort radius, CornerPosition position, ushort color) {
        }
        public void DrawArrow(ushort x, ushort y, ushort size, DrawingDirection direction, ushort color) {
        }
        public void DrawRectangle(ushort x0, ushort y0, ushort x1, ushort y1, ushort color) {
        }
        public void DrawRectangleFilled(ushort x0, ushort y0, ushort x1, ushort y1, ushort color) {
        }
        public void DrawRectangleRounded(ushort x0, ushort y0, ushort x1, ushort y1, ushort color, ushort radius, RoundedCornerStyle corners) {
        }
        public void DrawTriangle(ushort x0, ushort y0, ushort x1, ushort y1, ushort x2, ushort y2, ushort color) {
        }
        public void DrawTriangleFilled(ushort x0, ushort y0, ushort x1, ushort y1, ushort x2, ushort y2, ushort color) {
        }
        //public void DrawString( ushort x, ushort y, ushort color, const FONT_INFO *fontInfo, char *str );
        //public void DrawGetStringWidth( const FONT_INFO *fontInfo, char *str );
        public void DrawProgressBar(ushort x, ushort y, ushort width, ushort height, RoundedCornerStyle borderCorners, RoundedCornerStyle progressCorners, ushort borderColor, ushort borderFillColor, ushort progressBorderColor, ushort progressFillColor, byte progress) {
        }
        //public void DrawButton( ushort x, ushort y, ushort width, ushort height, const FONT_INFO *fontInfo, ushort fontHeight, ushort borderclr, ushort fillclr, ushort fontclr, char* text );
        public void DrawIcon16(ushort x, ushort y, ushort color, ushort[] icon) {
        }
        public void DrawRGB24toRGB565(byte r, byte g, byte b) {
        }
        public void DrawRGB565toBGRA32(ushort color) {
        }
        public void DrawBGR2RGB(ushort color) {
        }
        //public void DrawStringSmall( ushort x, ushort y, ushort color, char* text, struct FONT_DEF font );
        public void DrawBitmapImage(ushort x, ushort y, string filename) {
        }
    }
}
