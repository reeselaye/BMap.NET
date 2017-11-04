using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMap.NET.WindowsForm {
    public class LatLngUtils {
        public const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d) {
            return d * Math.PI / 180.0;
        }

        public static double GetDistanceByLatLng(LatLngPoint p1, LatLngPoint p2) {
            double radLat1 = rad(p1.Lat);
            double radLat2 = rad(p2.Lat);
            double a = radLat1 - radLat2;
            double b = rad(p1.Lng) - rad(p2.Lng);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
    }
}
