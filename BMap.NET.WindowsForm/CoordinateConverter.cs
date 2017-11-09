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
        /// WGS84 原始数据（十进制度、十进制分、六十进制秒）转换为 WGS84 标准数据（度、分、秒均为十进制）。
        /// </summary>
        /// <param name="wgs84"></param>
        /// <returns></returns>
        public static LatLngPoint WGS84RawToWGS84Standard(LatLngPoint wgs84raw) {
            return new LatLngPoint(t2(wgs84raw.Lng), t2(wgs84raw.Lat));
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

        private static double t2(double d) {
            string s = d.ToString();
            if (!s.Contains(".")) {
                s += ".0";
            }
            string[] split = s.Split('.');
            if (split.Length != 2) {
                throw new ApplicationException("WGS84 坐标转换错误");
            }
            return double.Parse(split[0]) + double.Parse("0." + split[1])*100.0/60.0;
        }
    }
}
