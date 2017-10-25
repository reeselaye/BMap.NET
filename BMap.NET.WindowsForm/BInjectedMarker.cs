using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BMap.NET.WindowsForm {
    public partial class BInjectedMarker : UserControl {
        private BMapControl _mapControl;
        private LatLngPoint _position = new LatLngPoint(0.0, 0.0);
        private BInjectedMarker _origin;
        
        public BInjectedMarker() {
            InitializeComponent();
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
            }
            get {
                return _position;
            }
        }

        private void NoticeMapControl() {
            if(_mapControl != null) {
                _mapControl.UpdateInjectedMarker(this);
            }
        }
    }
}
