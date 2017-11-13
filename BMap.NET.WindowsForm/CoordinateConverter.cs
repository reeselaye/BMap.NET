using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BMap.NET.HTTPService;

namespace BMap.NET.WindowsForm {
    public class CoordinateConverter {
        /// <summary>
        /// WGS84 标准数据转换为 BD09 数据。
        /// </summary>
        /// <param name="wgs84"></param>
        /// <returns></returns>
        public static LatLngPoint WGS84ToBD09(LatLngPoint wgs84) {
            return t(wgs84, 1, 5);
        }

        /// <summary>
        /// GCJ02 数据转换为 BD09 数据。
        /// </summary>
        /// <param name="gcj02"></param>
        /// <returns></returns>
        public static LatLngPoint GCJ02ToBD09(LatLngPoint gcj02) {
            return t(gcj02, 3, 5);
        }

        /// <summary>
        /// WGS84 数据转换为 GCJ02 数据。
        /// </summary>
        /// <param name="wgs84"></param>
        /// <returns></returns>
        public static LatLngPoint WGS84ToGCJ02(LatLngPoint wgs84) {
            string fmt = "0.000000";
            string lng = wgs84.Lng.ToString(fmt);
            string lat = wgs84.Lat.ToString(fmt);
            string url = "http://restapi.amap.com/v3/assistant/coordinate/convert?key=8817aa4e8b2094fb15a6e381ef76e5c2&locations=" + lng + "," + lat + "&coordsys=gps";
            ServiceBase srv = new ServiceBase();
            string json = srv.DownloadString(url);
            if (json == null) {
                throw new ApplicationException("高德地图 API 没有响应");
            }
            JObject result = JsonConvert.DeserializeObject(json) as JObject;
            if((int)result["status"] != 1) {
                throw new ApplicationException("高德地图 API 返回异常: " + (string)result["info"]);
            }
            string t = (string)result["locations"];
            string[] t1 = t.Split(',');
            return new LatLngPoint(double.Parse(t1[0]), double.Parse(t1[1]));
        }

        /// <summary>
        /// WGS84 原始数据（十进制度、十进制分、六十进制秒）转换为 WGS84 标准数据（度、分、秒均为十进制）。
        /// </summary>
        /// <param name="wgs84"></param>
        /// <returns></returns>
        public static LatLngPoint WGS84RawToWGS84Standard(LatLngPoint wgs84raw) {
            return new LatLngPoint(t2(wgs84raw.Lng, true), t2(wgs84raw.Lat, true));
        }

        public static LatLngPoint WGS84StandardToWGS84Raw(LatLngPoint wgs84) {
            return new LatLngPoint(t2(wgs84.Lng, false), t2(wgs84.Lat, false));
        }

        private static LatLngPoint t(LatLngPoint p, int from, int to) {

            CoordinateTransService service = new CoordinateTransService();
            JObject result = service.CoordinateTransform((p.Lng).ToString(), (p.Lat).ToString(), from, to);
            if(result == null) {
                throw new ApplicationException("API 木有响应");
            }
            if((int)result["status"] != 0) {
                throw new ApplicationException((string)result["message"]);
            }
            return new LatLngPoint((double)result["result"][0]["x"], (double)result["result"][0]["y"]);
        }

        private static double t2(double d, bool positive) {
            string s = d.ToString();
            if (!s.Contains(".")) {
                s += ".0";
            }
            string[] split = s.Split('.');
            if (split.Length != 2) {
                throw new ApplicationException("WGS84 坐标转换错误");
            }
            double r = positive ? 100.0 / 60.0 : 60.0 / 100.0;
            return double.Parse(split[0]) + double.Parse("0." + split[1])*r;
        }
    }
}
