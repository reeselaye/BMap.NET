using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BMap.NET.WindowsForm.DrawingObjects;

namespace BMap.NET.WindowsForm {
    public partial class BInjectedMarker : UserControl,  DrawingObject{
        // 绑定的地图控件
        private BMapControl _mapControl;

        // 位置
        private LatLngPoint _position = new LatLngPoint(0.0, 0.0);

        // 相对定位的目标点
        private BInjectedMarker _origin;

        // 走过的路线
        private BLine _route;
        public bool EnableRouteRecording {
            get;
            set;
        }
        public bool EnableRouteShow {
            get;
            set;
        }
        private Color _routeShowColor = Color.Yellow;
        public Color RouteColor {
            set {
                _routeShowColor = value;
            }
        }
        
        public BInjectedMarker(): this(new LatLngPoint(0.0, 0.0)) {
        }

        public BInjectedMarker(LatLngPoint position) {
            InitializeComponent();

            this.Position = position;

            _route = new BLine();
        }

        public BInjectedMarker InjectedMarkerOrigin {
            set {
                _origin = value;
            }
            get {
                return _origin;
            }
        }

        public BMapControl MapControl {
            set {
                _mapControl = value;
                NoticeMapControl();
            }
        }

        public LatLngPoint Position {
            set {
                _position = value;
                NoticeMapControl();
                if (EnableRouteRecording) {
                    _route.Points.Add(value);
                }
            }
            get {
                return _position;
            }
        }

        public void Draw(Graphics g, LatLngPoint center, int zoom, Size screen_size) {
            if (EnableRouteShow) {
                _route.Draw(g, center, zoom, screen_size);
            }
        }

        private void NoticeMapControl() {
            if(_mapControl != null) {
                _mapControl.UpdateInjectedMarker(this);
            }
        }
    }
}
