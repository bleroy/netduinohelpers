using System;
using System.Collections;
using netduino.helpers.Helpers;
using netduino.helpers.Hardware;
using netduino.helpers.Fonts;
namespace netduino.helpers.Imaging {
    public class VirtualCanvas : Canvas {
        protected BasicTypeSerializer Serializer;
        public VirtualCanvas() {
        }
        public VirtualCanvas(BasicTypeSerializer serializer) {
            Serializer = serializer;
        }
        override public void DrawTestPattern() {
            Serializer.Put((byte)Command.DrawTestPattern);
        }
        override public void DrawPixel(int x, int y, BasicColor color) {
            Serializer.Put((byte)Command.DrawPixel);
            Serializer.Put((ushort)x);
            Serializer.Put((ushort)y);
            Serializer.Put((ushort)color);
        }
        override public void DrawFill(BasicColor color) {
            Serializer.Put((byte)Command.DrawFill);
            Serializer.Put((ushort)color);
        }
        override public void DrawLine(int x0, int y0, int x1, int y1, BasicColor color) {
            Serializer.Put((byte)Command.DrawLine);
            Serializer.Put((ushort)x0);
            Serializer.Put((ushort)y0);
            Serializer.Put((ushort)x1);
            Serializer.Put((ushort)y1);
            Serializer.Put((ushort)color);
        }
        override public void DrawLineDotted(int x0, int y0, int x1, int y1, int empty, int solid, BasicColor color) {
            Serializer.Put((byte)Command.DrawLineDotted);
            Serializer.Put((ushort)x0);
            Serializer.Put((ushort)y0);
            Serializer.Put((ushort)x1);
            Serializer.Put((ushort)y1);
            Serializer.Put((ushort)empty);
            Serializer.Put((ushort)solid);
            Serializer.Put((ushort)color);
        }
        override public void DrawCircle(int xCenter, int yCenter, int radius, BasicColor color) {
            Serializer.Put((byte)Command.DrawCircle);
            Serializer.Put((ushort)xCenter);
            Serializer.Put((ushort)yCenter);
            Serializer.Put((ushort)radius);
            Serializer.Put((ushort)color);
        }
        override public void DrawCircleFilled(int xCenter, int yCenter, int radius, BasicColor color) {
            Serializer.Put((byte)Command.DrawCircleFilled);
            Serializer.Put((ushort)xCenter);
            Serializer.Put((ushort)yCenter);
            Serializer.Put((ushort)radius);
            Serializer.Put((ushort)color);
        }
        override public void DrawCornerFilled(int xCenter, int yCenter, int radius, CornerPosition position, BasicColor color) {
            Serializer.Put((byte)Command.DrawCornerFilled);
            Serializer.Put((ushort)xCenter);
            Serializer.Put((ushort)yCenter);
            Serializer.Put((ushort)radius);
            Serializer.Put((ushort)position);
            Serializer.Put((ushort)color);
        }
        override public void DrawArrow(int x, int y, int size, DrawingDirection direction, BasicColor color) {
            Serializer.Put((byte)Command.DrawArrow);
            Serializer.Put((ushort)x);
            Serializer.Put((ushort)y);
            Serializer.Put((ushort)size);
            Serializer.Put((ushort)direction);
            Serializer.Put((ushort)color);
        }
        override public void DrawRectangle(int x0, int y0, int x1, int y1, BasicColor color) {
            Serializer.Put((byte)Command.DrawRectangle);
            Serializer.Put((ushort)x0);
            Serializer.Put((ushort)y0);
            Serializer.Put((ushort)x1);
            Serializer.Put((ushort)y1);
            Serializer.Put((ushort)color);
        }
        override public void DrawRectangleFilled(int x0, int y0, int x1, int y1, BasicColor color) {
            Serializer.Put((byte)Command.DrawRectangleFilled);
            Serializer.Put((ushort)x0);
            Serializer.Put((ushort)y0);
            Serializer.Put((ushort)x1);
            Serializer.Put((ushort)y1);
            Serializer.Put((ushort)color);
        }
        override public void DrawRectangleRounded(int x0, int y0, int x1, int y1, BasicColor color, int radius, RoundedCornerStyle corners) {
            Serializer.Put((byte)Command.DrawRectangleRounded);
            Serializer.Put((ushort)x0);
            Serializer.Put((ushort)y0);
            Serializer.Put((ushort)x1);
            Serializer.Put((ushort)y1);
            Serializer.Put((ushort)color);
            Serializer.Put((ushort)radius);
            Serializer.Put((ushort)corners);
        }
        override public void DrawTriangle(int x0, int y0, int x1, int y1, int x2, int y2, BasicColor color) {
            Serializer.Put((byte)Command.DrawTriangle);
            Serializer.Put((ushort)x0);
            Serializer.Put((ushort)y0);
            Serializer.Put((ushort)x1);
            Serializer.Put((ushort)y1);
            Serializer.Put((ushort)x2);
            Serializer.Put((ushort)y2);
            Serializer.Put((ushort)color);
        }
        override public void DrawTriangleFilled(int x0, int y0, int x1, int y1, int x2, int y2, BasicColor color) {
            Serializer.Put((byte)Command.DrawTriangleFilled);
            Serializer.Put((ushort)x0);
            Serializer.Put((ushort)y0);
            Serializer.Put((ushort)x1);
            Serializer.Put((ushort)y1);
            Serializer.Put((ushort)x2);
            Serializer.Put((ushort)y2);
            Serializer.Put((ushort)color);
        }
        override public void DrawProgressBar(
            int x, int y,
            int width, int height,
            RoundedCornerStyle borderCorners,
            RoundedCornerStyle progressCorners,
            BasicColor borderColor, BasicColor borderFillColor,
            BasicColor progressBorderColor, BasicColor progressFillColor,
            int progress) {
            Serializer.Put((byte)Command.DrawProgressBar);
            Serializer.Put((ushort)x);
            Serializer.Put((ushort)y);
            Serializer.Put((ushort)width);
            Serializer.Put((ushort)height);
            Serializer.Put((ushort)borderCorners);
            Serializer.Put((ushort)progressCorners);
            Serializer.Put((ushort)borderColor);
            Serializer.Put((ushort)borderFillColor);
            Serializer.Put((ushort)progressBorderColor);
            Serializer.Put((ushort)progressFillColor);
            Serializer.Put((ushort)progress);
        }
        override public void DrawButton(
            int x, int y,
            int width, int height,
            FontInfo fontInfo,
            int fontHeight,
            BasicColor borderColor,
            BasicColor fillColor,
            BasicColor fontColor,
            string text,
            Canvas.RoundedCornerStyle cornerStyle = RoundedCornerStyle.All) {
            Serializer.Put((byte)Command.DrawButton);
            Serializer.Put((ushort)x);
            Serializer.Put((ushort)y);
            Serializer.Put((ushort)width);
            Serializer.Put((ushort)height);
            Serializer.Put(fontInfo.ID);
            Serializer.Put((ushort)fontHeight);
            Serializer.Put((ushort)borderColor);
            Serializer.Put((ushort)fillColor);
            Serializer.Put((ushort)fontColor);
            Serializer.Put(text, true);
            Serializer.Put((ushort)cornerStyle);
        }
        override public void DrawIcon16(int x, int y, BasicColor color, ushort[] icon) {
            Serializer.Put((byte)Command.DrawIcon16);
            Serializer.Put((ushort)x);
            Serializer.Put((ushort)y);
            Serializer.Put((ushort)color);
            Serializer.Put(icon);
        }
        override public void DrawString(int x, int y, BasicColor color, FontInfo fontInfo, string text) {
            Serializer.Put((byte)Command.DrawString);
            Serializer.Put((ushort)x);
            Serializer.Put((ushort)y);
            Serializer.Put((ushort)color);
            Serializer.Put(fontInfo.ID);
            Serializer.Put(text, true);
        }
        override public void DrawBitmapImage(int x, int y, string filename) {
            Serializer.Put((byte)Command.DrawBitmapImage);
            Serializer.Put((ushort)x);
            Serializer.Put((ushort)y);
            Serializer.Put(filename);
        }
        override protected void DrawCirclePoints(int cx, int cy, int x, int y, BasicColor color) {
            Serializer.Put((byte)Command.DrawCirclePoints);
            Serializer.Put((ushort)cx);
            Serializer.Put((ushort)cy);
            Serializer.Put((ushort)x);
            Serializer.Put((ushort)y);
            Serializer.Put((ushort)color);
        }
        override public void SetOrientation(LCD.Orientation orientation) {
            Serializer.Put((byte)Command.SetOrientation);
            Serializer.Put((ushort)orientation);
        }
    }
}
