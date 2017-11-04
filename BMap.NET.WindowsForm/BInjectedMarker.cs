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
                _route.Color = value;
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
            Point pCenter = MapHelper.GetScreenLocationByLatLng(Position, center, zoom, screen_size);
            Point pLeftTop = new Point(pCenter.X - Width / 2, pCenter.Y - Height / 2);
            Invoke(new MethodInvoker(() => {
                Location = pLeftTop;
            }));

            BInjectedMarker origin = InjectedMarkerOrigin;
            if (origin != null) {
                LatLngPoint me = Position;
                LatLngPoint origin_ = origin.Position;
                LatLngPoint med = new LatLngPoint(me.Lng, origin_.Lat);
                Point pMe = MapHelper.GetScreenLocationByLatLng(me, center, zoom, screen_size);
                Point pOrigin = MapHelper.GetScreenLocationByLatLng(origin_, center, zoom, screen_size);
                Point pMed = MapHelper.GetScreenLocationByLatLng(med, center, zoom, screen_size);

                Font font = this.Font;
                g.DrawLine(new Pen(Color.Red, 2), pMe, pOrigin);
                g.DrawLine(new Pen(Color.Fuchsia, 2), pMe, pMed);
                g.DrawLine(new Pen(Color.Fuchsia, 2), pOrigin, pMed);
                g.DrawString((LatLngUtils.GetDistanceByLatLng(me, origin_) * 1000).ToString("0.0") + " m", font, new SolidBrush(Color.Red), new PointF((pMe.X+pOrigin.X)/2, (pMe.Y+pOrigin.Y)/2));
                g.DrawString((LatLngUtils.GetDistanceByLatLng(me, med) * 1000).ToString("0.0") + " m", font, new SolidBrush(Color.Fuchsia), new PointF((pMe.X+pMed.X) / 2, (pMe.Y+pMed.Y) / 2));
                g.DrawString((LatLngUtils.GetDistanceByLatLng(origin_, med) * 1000).ToString("0.0") + " m", font, new SolidBrush(Color.Fuchsia), new PointF((pOrigin.X+pMed.X) / 2, (pOrigin.Y+pMed.Y) / 2));
            }

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
