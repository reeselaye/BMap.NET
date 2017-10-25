using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BMap.NET.WindowsForm.DrawingObjects {
    abstract class ColoredDrawingObject : DrawingObject {
        private Color _color = Color.Yellow;
        public Color Color {
            get {
                return _color;
            }
            set {
                _color = value;
            }
        }

        private float _lineWidth = 2.0F;
        public float LineWidth {
            set {
                _lineWidth = value;
            }
            get {
                return _lineWidth;
            }
        }

        public abstract void Draw(Graphics g, LatLngPoint center, int zoom, Size screen_size);
    }
}
