using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BMap.NET.WindowsForm {
    public enum RelativeDirection {
        NorthEast,
        NorthWest,
        SouthEast,
        SouthWest,
        Same, // 重合
        Invalid, // 无效
    }

    public class LatLngUtils {
        public const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d) {
            return d * Math.PI / 180.0;
        }

        private static double degree(double r) {
            return r * 180.0 / Math.PI;
        }

        /// <summary>
        /// 根据 WGS-84 坐标计算相对距离。
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据 WGS-84 坐标计算相对位置。（仅适用于东北半球）
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pRef"></param>
        /// <returns></returns>
        public static RelativeDirection GetRelativeDirection(LatLngPoint p, LatLngPoint pRef) {
            double precision = 0.000001;
            if (Math.Abs(p.Lat - pRef.Lat) < precision && Math.Abs(p.Lng - pRef.Lng) < precision) {
                return RelativeDirection.Same;
            }

            bool north = p.Lat > pRef.Lat;
            bool east = p.Lng > pRef.Lng;
            if(north && east) {
                return RelativeDirection.NorthEast;
            } else if(north && !east) {
                return RelativeDirection.NorthWest;
            } else if(!north && east) {
                return RelativeDirection.SouthEast;
            } else if(!north && !east) {
                return RelativeDirection.SouthWest;
            }

            return RelativeDirection.Invalid;
        }

        /// <summary>
        /// 根据 WGS-84 坐标计算相对距离。（仅适用于东北半球）
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pRef"></param>
        /// <returns></returns>
        public static PointF GetRelativeDistance(LatLngPoint p, LatLngPoint pRef) {
            LatLngPoint pMed = new LatLngPoint(p.Lng, pRef.Lat);
            float x = Convert.ToSingle(LatLngUtils.GetDistanceByLatLng(pRef, pMed));
            float y = Convert.ToSingle(LatLngUtils.GetDistanceByLatLng(p, pMed));
            RelativeDirection relativeDirection = LatLngUtils.GetRelativeDirection(p, pRef);
            switch (relativeDirection) {
                case RelativeDirection.NorthEast:
                    break;
                case RelativeDirection.NorthWest:
                    x = -x;
                    break;
                case RelativeDirection.SouthWest:
                    x = -x;
                    y = -y;
                    break;
                case RelativeDirection.SouthEast:
                    y = -y;
                    break;
                case RelativeDirection.Same:
                    break;
                default:
                    throw new ApplicationException("无法判断两点的相对距离");
            }
            return new PointF(x, y);
        }

        /// <summary>
        /// 根据参考点 WGS-84 坐标和相对距离计算另一点 WGS-84 坐标。（仅适用于东北半球）
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static LatLngPoint GetLatLngByRelativeDistance(LatLngPoint origin, PointF distance) {
            double radx = distance.X / EARTH_RADIUS;
            double rady = distance.Y / EARTH_RADIUS;
            return new LatLngPoint(origin.Lng + degree(radx), origin.Lat + degree(rady));
        }
    }
}
